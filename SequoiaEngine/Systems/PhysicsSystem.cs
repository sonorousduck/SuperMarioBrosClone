using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using SequoiaEngine.Utilities;

namespace SequoiaEngine
{
    public class PhysicsSystem : System
    {
        /*        public static int PHYSICS_DIMENSION_WIDTH = 2000;
                public static int PHYSICS_DIMENSION_HEIGHT = 2000; // These should match up with rendering
                public static float GRAVITY = -9.81f;*/

        /*        private Quadtree quadtree;
                private Quadtree staticTree;*/


        public const float GRAVITY_CONST = 9.81f * 20;

        private Grid grid;
        private Grid staticGrid;
        private Grid hudGrid;

        public Vector2 Dimensions;
        public Vector2 GridSize;
        public Vector2 GridStartPosition;


        public PhysicsSystem(SystemManager systemManager) : base(systemManager, typeof(Transform), typeof(Rigidbody), typeof(Collider))
        {
            //staticTree = new Quadtree(PHYSICS_DIMENSION_WIDTH, PHYSICS_DIMENSION_HEIGHT);
            Dimensions = new Vector2(2000, 2000);
            GridSize = new Vector2(16, 16);
            GridStartPosition = new Vector2(-500, -500);

            grid = new Grid(GridStartPosition, Dimensions, GridSize, false);
            staticGrid = new Grid(GridStartPosition, Dimensions, GridSize, true);
            hudGrid = new Grid(Vector2.Zero, new Vector2(GameManager.Instance.RenderWidth, GameManager.Instance.RenderHeight), GridSize, true);

            staticGrid.ShouldRebuild = true;
            hudGrid.ShouldRebuild = true;
        }

        public PhysicsSystem(SystemManager systemManager, Vector2 dimensions, Vector2 gridSize) : base(systemManager, typeof(Transform), typeof(Rigidbody), typeof(Collider))
        {
            //staticTree = new Quadtree(PHYSICS_DIMENSION_WIDTH, PHYSICS_DIMENSION_HEIGHT);
            Dimensions = dimensions;
            GridSize = gridSize;
            GridStartPosition = new Vector2(-10, -10);

            grid = new Grid(GridStartPosition, Dimensions, GridSize, false);
            staticGrid = new Grid(GridStartPosition, Dimensions, GridSize, true);
            hudGrid = new Grid(Vector2.Zero, new Vector2(GameManager.Instance.RenderWidth, GameManager.Instance.RenderHeight), GridSize, true);

            
            staticGrid.ShouldRebuild = true;
            hudGrid.ShouldRebuild = true;
        }

        public PhysicsSystem(SystemManager systemManager, Vector2 dimensions, Vector2 gridSize, Vector2 gridStartPos) : base(systemManager, typeof(Transform), typeof(Rigidbody), typeof(Collider))
        {
            //staticTree = new Quadtree(PHYSICS_DIMENSION_WIDTH, PHYSICS_DIMENSION_HEIGHT);
            Dimensions = dimensions;
            GridSize = gridSize;
            GridStartPosition = gridStartPos;

            grid = new Grid(GridStartPosition, Dimensions, GridSize, false);
            staticGrid = new Grid(GridStartPosition, Dimensions, GridSize, true);
            hudGrid = new Grid(Vector2.Zero, new Vector2(GameManager.Instance.RenderWidth, GameManager.Instance.RenderHeight), GridSize, true);


            staticGrid.ShouldRebuild = true;
            hudGrid.ShouldRebuild = true;
        }


        protected override void Add(GameObject gameObject)
        {
            base.Add(gameObject);

            if (gameObject.ContainsComponentOfParentType<Collider>())
            {
                if (gameObject.GetComponent<Collider>().isStatic)
                {
                    staticGrid.Insert(gameObject);
                }
                else if (gameObject.GetComponent<Transform>().IsHUD)
                {
                    hudGrid.Insert(gameObject);
                }

                else
                {
                    grid.Insert(gameObject);
                }
            }
        }


        /// <summary>
        /// This section updates all the rigidbody positions, and calls the Collision events from a component's script, if it has one
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (gameObjects.Count == 0) return; // i.e. we don't want to have to do the work to clear the grid everytime if we don't have to


            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                if (!gameObject.IsEnabled() || gameObject.GetComponent<Collider>().isStatic) continue;


                bool moved = false;

                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                Transform transform = gameObject.GetComponent<Transform>();
                Collider genericCollider = gameObject.GetComponent<Collider>();

                if (rb.usesGravity)
                {
                    rb.velocity += new Vector2(0, GRAVITY_CONST * (GameManager.Instance.ElapsedSeconds) * rb.gravityScale);
                }

                rb.velocity += new Vector2(rb.acceleration.X * GameManager.Instance.ElapsedSeconds * GameManager.Instance.ElapsedSeconds, rb.acceleration.Y * GameManager.Instance.ElapsedSeconds * GameManager.Instance.ElapsedSeconds);

                transform.position += (rb.velocity * GameManager.Instance.ElapsedSeconds).ToInt();


                // This is where I will check the sweep? Well, before applying any movement at all, I should check?



                while (rb.ScriptedMovementLength() > 0)
                {
                    Vector2 scriptedMovement = rb.GetNextScriptedMovement();
                    transform.position += scriptedMovement;
                }

                foreach (var child in gameObject.GetChildren())
                {
                    child.GetComponent<Transform>().position = transform.position + child.GetComponent<Transform>().Offset;
                }


                if (moved)
                {
                    transform.previousPosition = transform.position;
                }
            }


            grid.Clear();
            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                if (!gameObject.IsEnabled()) continue;

                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                Transform transform = gameObject.GetComponent<Transform>();
                Collider genericCollider = gameObject.GetComponent<Collider>();
                
                if (!genericCollider.isStatic && !transform.IsHUD)
                {
                    grid.Insert(gameObject);
                }
                else if (staticGrid.ShouldRebuild && genericCollider.isStatic)
                {
                    staticGrid.Insert(gameObject);
                }

                if (hudGrid.ShouldRebuild && transform.IsHUD)
                {
                    hudGrid.Insert(gameObject);
                }
            }
            

            staticGrid.ShouldRebuild = false;
            hudGrid.ShouldRebuild = false;


            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                if (!gameObject.IsEnabled()) continue;

                UpdateGameObject(gameObject);
            }

        }


        private void UpdateGameObject(GameObject gameObject)
        {
            HashSet<GameObject> possibleCollisions = grid.GetPossibleCollisions(ref gameObject);
            possibleCollisions.UnionWith(staticGrid.GetPossibleCollisions(ref gameObject));
            HashSet<GameObject> hudCollisions = hudGrid.GetPossibleCollisions(ref gameObject, true);

            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            List<uint> collisionsThisFrame = new();



            foreach (GameObject possibleCollision in possibleCollisions)
            {
                RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();

                if (HasCollision(gameObject, possibleCollision))
                {
                    collisionsThisFrame.Add(possibleCollision.Id);
                    // On Collision Start
                    if (!rb.currentlyCollidingWith.Contains(possibleCollision.Id))
                    {
                        if (gameObject.ContainsComponentOfParentType<Script>())
                        {
                            gameObject.GetComponent<Script>().OnCollisionStart(possibleCollision);
                        }
                        rectangleCollider.OnCollisionStart?.Invoke(possibleCollision);


                    }
                    else // On Collision
                    {
                        if (gameObject.ContainsComponentOfParentType<Script>())
                        {
                            gameObject.GetComponent<Script>().OnCollision(possibleCollision);
                        }

                        rectangleCollider.OnCollision?.Invoke(possibleCollision);

                    }
                }
                else // On Collision End
                {
                    if (rb.currentlyCollidingWith.Contains(possibleCollision.Id))
                    {
                        if (gameObject.ContainsComponentOfParentType<Script>())
                        {
                            gameObject.GetComponent<Script>().OnCollisionEnd(possibleCollision);
                        }
                        rectangleCollider.OnCollisionEnd?.Invoke(possibleCollision);

                    }
                }
            }

            foreach (GameObject possibleCollision in hudCollisions) 
            {
                RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();

                if (HasCollision(gameObject, possibleCollision, true))
                {

                    collisionsThisFrame.Add(possibleCollision.Id);
                    // On Collision Start
                    if (!rb.currentlyCollidingWith.Contains(possibleCollision.Id))
                    {
                        if (gameObject.ContainsComponentOfParentType<Script>())
                        {
                            gameObject.GetComponent<Script>().OnCollisionStart(possibleCollision);
                        }
                        rectangleCollider.OnCollisionStart?.Invoke(possibleCollision);

                    }
                    else // On Collision
                    {
                        if (gameObject.ContainsComponentOfParentType<Script>())
                        {
                            gameObject.GetComponent<Script>().OnCollision(possibleCollision);
                        }
                        rectangleCollider.OnCollision?.Invoke(possibleCollision);

                    }
                }
                else // On Collision End
                {
                    if (rb.currentlyCollidingWith.Contains(possibleCollision.Id))
                    {
                        if (gameObject.ContainsComponentOfParentType<Script>())
                        {
                            gameObject.GetComponent<Script>().OnCollisionEnd(possibleCollision);
                        }

                        rectangleCollider.OnCollisionStart?.Invoke(possibleCollision);

                    }
                }
            }

            rb.currentlyCollidingWith = collisionsThisFrame;

        }


        private bool HasCollision(GameObject one, GameObject two, bool isHud = false)
        {
            if (one == two)
            {
                return false;
            }

            return SquareOnSquare(one, two, isHud);


            //if (one.ContainsComponent<CircleCollider>())
            //{
            //    if (two.ContainsComponent<CircleCollider>())
            //    {
            //        return CircleOnCircle(one, two, isHud);
            //    }
            //    else
            //    {
            //        return CircleOnSquare(one, two, isHud);
            //    }
            //}
            //else
            //{
            //    if (two.ContainsComponent<CircleCollider>())
            //    {
            //        return CircleOnSquare(two, one, isHud);
            //    }
            //    else
            //    {
            //        return SquareOnSquare(one, two, isHud);
            //    }
            //}
        }


        private bool CircleOnCircle(GameObject circle1, GameObject circle2, bool isHud)
        {
            // Squared distance is less than the summed squared radius
            // TODO: IS THIS RIGHT? I DON'T THINK SO

            if (isHud)
            {
                Vector2 circleOnePosition = circle1.GetComponent<Transform>().position;
                Vector2 circleTwoPosition = circle2.GetComponent<Transform>().position;

                if (!circle1.GetComponent<Transform>().IsHUD)
                {
                    circleOnePosition = (GameManager.Instance.Camera.WorldToScreen(circleOnePosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
                }
                if (!circle2.GetComponent<Transform>().IsHUD)
                {
                    circleTwoPosition = (GameManager.Instance.Camera.WorldToScreen(circleTwoPosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
                }
                return (Vector2.DistanceSquared(circleOnePosition + circle1.GetComponent<Collider>().offset, circleTwoPosition + circle2.GetComponent<Collider>().offset) < MathF.Pow(circle1.GetComponent<CircleCollider>().radius + circle2.GetComponent<CircleCollider>().radius, 2));
            }


            return (Vector2.DistanceSquared(circle1.GetComponent<Transform>().position + circle1.GetComponent<Collider>().offset, circle2.GetComponent<Transform>().position + circle2.GetComponent<Collider>().offset) < MathF.Pow(circle1.GetComponent<CircleCollider>().radius + circle2.GetComponent<CircleCollider>().radius, 2));
        }

        // Used http://jeffreythompson.org/collision-detection/circle-rect.php
        private bool CircleOnSquare(GameObject circle, GameObject square, bool isHud)
        {


            Transform squareTransform = square.GetComponent<Transform>();
            Transform circleTransform = circle.GetComponent<Transform>();
            RectangleCollider squareCollider = square.GetComponent<RectangleCollider>();
            CircleCollider circleCollider = circle.GetComponent<CircleCollider>();

            Vector2 circleLocation = circleTransform.position;
            Vector2 rectangleLocation = squareTransform.position;

            if (isHud)
            {
                if (!circleTransform.IsHUD)
                {
                    circleLocation = (GameManager.Instance.Camera.WorldToScreen(circleLocation) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
                }

                else if (!squareTransform.IsHUD)
                {
                    rectangleLocation = (GameManager.Instance.Camera.WorldToScreen(rectangleLocation) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
                }
            }



            if (circleTransform.position.X < rectangleLocation.X - squareCollider.size.X / 2)
            {
                circleLocation.X = rectangleLocation.X - squareCollider.size.X / 2;
            }
            else if (circleTransform.position.X > rectangleLocation.X + squareCollider.size.X / 2)
            {
                circleLocation.X = rectangleLocation.X + squareCollider.size.X / 2;
            }

            if (circleTransform.position.Y < rectangleLocation.Y - squareCollider.size.Y / 2)
            {
                circleLocation.Y = rectangleLocation.Y - squareCollider.size.Y / 2;
            }
            else if (circleTransform.position.Y > rectangleLocation.Y + squareCollider.size.Y / 2)
            {
                circleLocation.Y = rectangleLocation.Y + squareCollider.size.Y / 2;
            }

            float squaredDistance = Vector2.DistanceSquared(circleTransform.position, circleLocation);

            return (squaredDistance <= MathF.Pow(circle.GetComponent<CircleCollider>().radius, 2));


        }

        private bool SquareOnSquare(GameObject square1, GameObject square2, bool isHud)
        {


            RectangleCollider square1Collider = square1.GetComponent<RectangleCollider>();
            RectangleCollider square2Collider = square2.GetComponent<RectangleCollider>();

            Transform square1Transform = new Transform(square1.GetComponent<Transform>().position + square1Collider.offset, square1.GetComponent<Transform>().rotation, square1.GetComponent<Transform>().scale);
            Transform square2Transform = new Transform(square2.GetComponent<Transform>().position + square2Collider.offset, square2.GetComponent<Transform>().rotation, square2.GetComponent<Transform>().scale);


            Vector2 squareOnePosition = square1Transform.position;
            Vector2 squareTwoPosition = square2Transform.position;


            if (isHud)
            {
                if (!square1Transform.IsHUD)
                {
                    squareOnePosition = (GameManager.Instance.Camera.WorldToScreen(squareOnePosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
                }

                else if (!square2Transform.IsHUD)
                {
                    squareTwoPosition = (GameManager.Instance.Camera.WorldToScreen(squareTwoPosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
                }
            }


            return !(
                    squareOnePosition.X - square1Collider.size.X / 2f > squareTwoPosition.X + square2Collider.size.X / 2f || // sq1 left is greater than sq2 right
                    squareOnePosition.X + square1Collider.size.X / 2f < squareTwoPosition.X - square2Collider.size.X / 2f || // sq1 right is less than sq2 left
                    squareOnePosition.Y - square1Collider.size.Y / 2f > squareTwoPosition.Y + square2Collider.size.Y / 2f || // sq1 top is below sq2 bottom
                    squareOnePosition.Y + square1Collider.size.Y / 2f < squareTwoPosition.Y - square2Collider.size.Y / 2f // sq1 bottom is above sq1 top
                    );
        }
    }
}