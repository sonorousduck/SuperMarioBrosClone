using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SequoiaEngine
{
    public class Grid
    {
        public Grid(Vector2 position, Vector2 size, Vector2 sizeOfGridUnit, bool isStatic)
        {
            this.Position = position;
            this.Size = size;
            this.SizeOfGridUnit = sizeOfGridUnit;
            this.IsStatic = isStatic;
            this.ShouldRebuild = true;

            this.GridAmount = (int)((size.X / sizeOfGridUnit.X) * (size.Y / sizeOfGridUnit.Y)); 
            this.AmountPerRow = (int)(size.X / sizeOfGridUnit.X);

            this.FirstIndex = (int)((this.Position.Y / 2.0f) * AmountPerRow + this.Position.X);

            this.grid = Resize(this.GridAmount);
        }


        public List<List<GameObject>> Resize(int gridAmount)
        {
            List<List<GameObject>> gameObjects = new List<List<GameObject>>();

            for (int i = 0; i < gridAmount; i++)
            {
                gameObjects.Add(new List<GameObject>());
            }
            return gameObjects;
        }

        public void Insert(GameObject entity)
        {
            Transform transform = entity.GetComponent<Transform>();
            Vector2 colliderSize = Vector2.Zero;
            Vector2 centerOfCollider = Vector2.Zero;



            if (entity.TryGetComponent(out RectangleCollider rectangleCollider))
            {
                colliderSize = rectangleCollider.size;
                centerOfCollider = rectangleCollider.offset;
            }
            else if (entity.TryGetComponent(out CircleCollider circleCollider))
            {
                colliderSize = new Vector2(circleCollider.radius, circleCollider.radius);
                centerOfCollider = circleCollider.offset;
            }

            Vector2 position = transform.position - colliderSize / 2;
            Vector2 endPosition = position + colliderSize;

            int startColumn = (int)((position.X - this.Position.X) / SizeOfGridUnit.X);
            int endColumn = (int)((endPosition.X - this.Position.X) / SizeOfGridUnit.X );

            int startRow = (int)((position.Y - this.Position.Y) / SizeOfGridUnit.Y);
            int endRow = (int)((endPosition.Y - this.Position.Y) / SizeOfGridUnit.Y);

            for (int y = startRow; y <= endRow; y++)
            {
                for (int x = startColumn; x <= endColumn; x++)
                {
                    if (AmountPerRow * y + x < grid.Capacity)
                    {
                        try
                        {
                            grid[AmountPerRow * y + x].Add(entity);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Entity out of physics bounds. Removing!");
                            ScreenManager.Instance.SystemManager.Remove(entity);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Caution! Out of the physics Domain! (Logging from Grid.cs)");
                    }
                }
            }
        }

        public void Clear()
        {
            grid.Clear();
            grid = Resize(GridAmount);
        }

        public HashSet<GameObject> GetPossibleCollisions(ref GameObject entity, bool isHud = false, float deltaX = 0, float deltaY = 0)
        {
            HashSet<GameObject> results = new HashSet<GameObject>();

            GetPossibleCollisions(entity, ref results, isHud, deltaX, deltaY);

            return results;
        }

        private void GetPossibleCollisions(GameObject entity, ref HashSet<GameObject> results, bool isHud, float deltaX, float deltaY)
        {
            Transform transform = entity.GetComponent<Transform>();
            Vector2 colliderSize = Vector2.Zero;
            Vector2 centerOfCollider = Vector2.Zero;
            CollisionLayer entityLayer = CollisionLayer.None;



            if (entity.TryGetComponent(out RectangleCollider rectangleCollider))
            {
                colliderSize = rectangleCollider.size;
                centerOfCollider = rectangleCollider.offset;
                entityLayer = rectangleCollider.LayersToCollideWith;
            }
            else if (entity.TryGetComponent(out CircleCollider circleCollider))
            {
                colliderSize = new Vector2(circleCollider.radius, circleCollider.radius);
                centerOfCollider = circleCollider.offset;
                entityLayer = circleCollider.LayersToCollideWith;
            }


            // If we have a deltaX or a deltaY, we will move the endPosition to just be where it would have ended. Essentially, we have a really big box
            Vector2 position = transform.position + centerOfCollider;
            Vector2 endPosition = position + colliderSize + new Vector2(deltaX, deltaY);


            // If the collider is not HUD, but we are looking for HUD elements, we need to transform it from world space to screen space
            if (!transform.IsHUD && isHud)
            {
                position = (GameManager.Instance.Camera.WorldToScreen(position) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
                endPosition = (GameManager.Instance.Camera.WorldToScreen(endPosition) / GameManager.Instance.ActualWindowSize) * GameManager.Instance.RenderWindowSize;
            }


            int startColumn = (int)((position.X - this.Position.X) / SizeOfGridUnit.X);
            int endColumn = (int)((endPosition.X - this.Position.X) / SizeOfGridUnit.X);

            int startRow = (int)((position.Y - this.Position.Y) / SizeOfGridUnit.Y);
            int endRow = (int)((endPosition.Y - this.Position.Y) / SizeOfGridUnit.Y);

            for (int y = startRow; y <= endRow; y++)
            {
                for (int x = startColumn; x <= endColumn; x++)
                {
                    int indexPosition = AmountPerRow * y + x;

                    if (indexPosition < 0 || indexPosition >= grid.Count)
                    {
                        return;
                    }


                    for (int i = 0; i < grid[indexPosition].Count; i++)
                    {
                        GameObject possibleCollisions = grid[indexPosition][i];
                        if (possibleCollisions != entity)
                        {
                            CollisionLayer possibleCollisionLayer = possibleCollisions.GetComponent<RectangleCollider>().Layer;

                            if ((possibleCollisionLayer & entityLayer) != 0)
                            {
                                results.Add(possibleCollisions);
                            }
                        }
                        
                    }

                }
            }
        }



        public Vector2 Position;
        public int AmountPerRow;
        public int GridAmount;
        public int FirstIndex;
        public Vector2 Size;
        public Vector2 SizeOfGridUnit;

        public bool IsStatic;
        public bool ShouldRebuild;

        public List<List<GameObject>> grid = new();



    }
}
