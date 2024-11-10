﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using SequoiaEngine.Utilities;


namespace SequoiaEngine
{
    public class PhysicsSystem : System
    {


        private World world;


        public PhysicsSystem(SystemManager systemManager) : base(systemManager, typeof(Transform), typeof(Rigidbody), typeof(Collider))
        {
            world = new World();
            world.Gravity = new Vector2(0f, 150f);
        }

        public PhysicsSystem(SystemManager systemManager, Vector2 dimensions, Vector2 gridSize) : base(systemManager, typeof(Transform), typeof(Rigidbody), typeof(Collider))
        {
            world = new World();
            world.Gravity = new Vector2(0f, 150f);

        }

        public PhysicsSystem(SystemManager systemManager, Vector2 dimensions, Vector2 gridSize, Vector2 gridStartPos) : base(systemManager, typeof(Transform), typeof(Rigidbody), typeof(Collider))
        {
            world = new World();
            world.Gravity = new Vector2(0f, 150f);

        }


        protected override void Add(GameObject gameObject)
        {
            base.Add(gameObject);

            if (gameObject.ContainsComponentOfParentType<Collider>())
            {
                Transform position = gameObject.GetComponent<Transform>();
                Collider collider = gameObject.GetComponent<Collider>();
                collider.CreateBody(world, position);
            }
        }


        /// <summary>
        /// This section updates all the rigidbody positions, and calls the Collision events from a component's script, if it has one
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (gameObjects.Count == 0) return; // i.e. we don't want to have to do the work to clear the grid everytime if we don't have to

            world.Step(GameManager.Instance.ElapsedSeconds);


            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                if (!gameObject.IsEnabled() || gameObject.GetComponent<Collider>().isStatic) continue;

                Transform transform = gameObject.GetComponent<Transform>();

                if (gameObject.TryGetComponent(out RectangleCollider rectangleCollider))
                {
                    transform.position = rectangleCollider.Body.Position;

                    if (rectangleCollider.Body.BodyType != BodyType.Static)
                    {
                        Debug.WriteLine(rectangleCollider.Body.LinearVelocity);
                    }
                }
                else if (gameObject.TryGetComponent(out CircleCollider circleCollider))
                {
                    transform.position = rectangleCollider.Body.Position;
                }


            }


        }


        //    private void UpdateGameObject(GameObject gameObject)
        //    {
        //        HashSet<GameObject> possibleCollisions = grid.GetPossibleCollisions(ref gameObject);
        //        possibleCollisions.UnionWith(staticGrid.GetPossibleCollisions(ref gameObject));
        //        HashSet<GameObject> hudCollisions = hudGrid.GetPossibleCollisions(ref gameObject, true);

        //        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        //        List<uint> collisionsThisFrame = new();



        //        foreach (GameObject possibleCollision in possibleCollisions)
        //        {
        //            RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();

        //            if (HasCollision(gameObject, possibleCollision))
        //            {
        //                collisionsThisFrame.Add(possibleCollision.Id);
        //                // On Collision Start
        //                if (!rb.currentlyCollidingWith.Contains(possibleCollision.Id))
        //                {
        //                    if (gameObject.ContainsComponentOfParentType<Script>())
        //                    {
        //                        gameObject.GetComponent<Script>().OnCollisionStart(possibleCollision);
        //                    }
        //                    rectangleCollider.OnCollisionStart?.Invoke(possibleCollision);


        //                }
        //                else // On Collision
        //                {
        //                    if (gameObject.ContainsComponentOfParentType<Script>())
        //                    {
        //                        gameObject.GetComponent<Script>().OnCollision(possibleCollision);
        //                    }

        //                    rectangleCollider.OnCollision?.Invoke(possibleCollision);

        //                }
        //            }
        //            else // On Collision End
        //            {
        //                if (rb.currentlyCollidingWith.Contains(possibleCollision.Id))
        //                {
        //                    if (gameObject.ContainsComponentOfParentType<Script>())
        //                    {
        //                        gameObject.GetComponent<Script>().OnCollisionEnd(possibleCollision);
        //                    }
        //                    rectangleCollider.OnCollisionEnd?.Invoke(possibleCollision);

        //                }
        //            }
        //        }

        //        foreach (GameObject possibleCollision in hudCollisions) 
        //        {
        //            RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();

        //            if (HasCollision(gameObject, possibleCollision, true))
        //            {

        //                collisionsThisFrame.Add(possibleCollision.Id);
        //                // On Collision Start
        //                if (!rb.currentlyCollidingWith.Contains(possibleCollision.Id))
        //                {
        //                    if (gameObject.ContainsComponentOfParentType<Script>())
        //                    {
        //                        gameObject.GetComponent<Script>().OnCollisionStart(possibleCollision);
        //                    }
        //                    rectangleCollider.OnCollisionStart?.Invoke(possibleCollision);

        //                }
        //                else // On Collision
        //                {
        //                    if (gameObject.ContainsComponentOfParentType<Script>())
        //                    {
        //                        gameObject.GetComponent<Script>().OnCollision(possibleCollision);
        //                    }
        //                    rectangleCollider.OnCollision?.Invoke(possibleCollision);

        //                }
        //            }
        //            else // On Collision End
        //            {
        //                if (rb.currentlyCollidingWith.Contains(possibleCollision.Id))
        //                {
        //                    if (gameObject.ContainsComponentOfParentType<Script>())
        //                    {
        //                        gameObject.GetComponent<Script>().OnCollisionEnd(possibleCollision);
        //                    }

        //                    rectangleCollider.OnCollisionStart?.Invoke(possibleCollision);

        //                }
        //            }
        //        }

        //        rb.currentlyCollidingWith = collisionsThisFrame;

        //    }


        //    private bool HasCollision(GameObject one, GameObject two, bool isHud = false)
        //    {
        //        if (one == two)
        //        {
        //            return false;
        //        }

        //        return SquareOnSquare(one, two, isHud);


        //        //if (one.ContainsComponent<CircleCollider>())
        //        //{
        //        //    if (two.ContainsComponent<CircleCollider>())
        //        //    {
        //        //        return CircleOnCircle(one, two, isHud);
        //        //    }
        //        //    else
        //        //    {
        //        //        return CircleOnSquare(one, two, isHud);
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    if (two.ContainsComponent<CircleCollider>())
        //        //    {
        //        //        return CircleOnSquare(two, one, isHud);
        //        //    }
        //        //    else
        //        //    {
        //        //        return SquareOnSquare(one, two, isHud);
        //        //    }
        //        //}
        //    }


        //    private bool CircleOnCircle(GameObject circle1, GameObject circle2, bool isHud)
        //    {
        //        // Squared distance is less than the summed squared radius
        //        // TODO: IS THIS RIGHT? I DON'T THINK SO

        //        if (isHud)
        //        {
        //            Vector2 circleOnePosition = circle1.GetComponent<Transform>().position;
        //            Vector2 circleTwoPosition = circle2.GetComponent<Transform>().position;

        //            if (!circle1.GetComponent<Transform>().IsHUD)
        //            {
        //                circleOnePosition = (GameManager.Instance.Camera.WorldToScreen(circleOnePosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
        //            }
        //            if (!circle2.GetComponent<Transform>().IsHUD)
        //            {
        //                circleTwoPosition = (GameManager.Instance.Camera.WorldToScreen(circleTwoPosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
        //            }
        //            return (Vector2.DistanceSquared(circleOnePosition + circle1.GetComponent<Collider>().offset, circleTwoPosition + circle2.GetComponent<Collider>().offset) < MathF.Pow(circle1.GetComponent<CircleCollider>().radius + circle2.GetComponent<CircleCollider>().radius, 2));
        //        }


        //        return (Vector2.DistanceSquared(circle1.GetComponent<Transform>().position + circle1.GetComponent<Collider>().offset, circle2.GetComponent<Transform>().position + circle2.GetComponent<Collider>().offset) < MathF.Pow(circle1.GetComponent<CircleCollider>().radius + circle2.GetComponent<CircleCollider>().radius, 2));
        //    }

        //    // Used http://jeffreythompson.org/collision-detection/circle-rect.php
        //    private bool CircleOnSquare(GameObject circle, GameObject square, bool isHud)
        //    {


        //        Transform squareTransform = square.GetComponent<Transform>();
        //        Transform circleTransform = circle.GetComponent<Transform>();
        //        RectangleCollider squareCollider = square.GetComponent<RectangleCollider>();
        //        CircleCollider circleCollider = circle.GetComponent<CircleCollider>();

        //        Vector2 circleLocation = circleTransform.position;
        //        Vector2 rectangleLocation = squareTransform.position;

        //        if (isHud)
        //        {
        //            if (!circleTransform.IsHUD)
        //            {
        //                circleLocation = (GameManager.Instance.Camera.WorldToScreen(circleLocation) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
        //            }

        //            else if (!squareTransform.IsHUD)
        //            {
        //                rectangleLocation = (GameManager.Instance.Camera.WorldToScreen(rectangleLocation) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
        //            }
        //        }



        //        if (circleTransform.position.X < rectangleLocation.X - squareCollider.size.X / 2)
        //        {
        //            circleLocation.X = rectangleLocation.X - squareCollider.size.X / 2;
        //        }
        //        else if (circleTransform.position.X > rectangleLocation.X + squareCollider.size.X / 2)
        //        {
        //            circleLocation.X = rectangleLocation.X + squareCollider.size.X / 2;
        //        }

        //        if (circleTransform.position.Y < rectangleLocation.Y - squareCollider.size.Y / 2)
        //        {
        //            circleLocation.Y = rectangleLocation.Y - squareCollider.size.Y / 2;
        //        }
        //        else if (circleTransform.position.Y > rectangleLocation.Y + squareCollider.size.Y / 2)
        //        {
        //            circleLocation.Y = rectangleLocation.Y + squareCollider.size.Y / 2;
        //        }

        //        float squaredDistance = Vector2.DistanceSquared(circleTransform.position, circleLocation);

        //        return (squaredDistance <= MathF.Pow(circle.GetComponent<CircleCollider>().radius, 2));


        //    }

        //    private bool SquareOnSquare(GameObject square1, GameObject square2, bool isHud)
        //    {


        //        RectangleCollider square1Collider = square1.GetComponent<RectangleCollider>();
        //        RectangleCollider square2Collider = square2.GetComponent<RectangleCollider>();

        //        Transform square1Transform = new Transform(square1.GetComponent<Transform>().position + square1Collider.offset, square1.GetComponent<Transform>().rotation, square1.GetComponent<Transform>().scale);
        //        Transform square2Transform = new Transform(square2.GetComponent<Transform>().position + square2Collider.offset, square2.GetComponent<Transform>().rotation, square2.GetComponent<Transform>().scale);


        //        Vector2 squareOnePosition = square1Transform.position;
        //        Vector2 squareTwoPosition = square2Transform.position;


        //        if (isHud)
        //        {
        //            if (!square1Transform.IsHUD)
        //            {
        //                squareOnePosition = (GameManager.Instance.Camera.WorldToScreen(squareOnePosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
        //            }

        //            else if (!square2Transform.IsHUD)
        //            {
        //                squareTwoPosition = (GameManager.Instance.Camera.WorldToScreen(squareTwoPosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
        //            }
        //        }


        //        return !(
        //                squareOnePosition.X - square1Collider.size.X / 2f > squareTwoPosition.X + square2Collider.size.X / 2f || // sq1 left is greater than sq2 right
        //                squareOnePosition.X + square1Collider.size.X / 2f < squareTwoPosition.X - square2Collider.size.X / 2f || // sq1 right is less than sq2 left
        //                squareOnePosition.Y - square1Collider.size.Y / 2f > squareTwoPosition.Y + square2Collider.size.Y / 2f || // sq1 top is below sq2 bottom
        //                squareOnePosition.Y + square1Collider.size.Y / 2f < squareTwoPosition.Y - square2Collider.size.Y / 2f // sq1 bottom is above sq1 top
        //                );
        //    }
        //}
    }
}