using System;
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    public class RectangleCollider : Collider
    {
        // Centered at object's position
        public Vector2 size;
        public Vector2 Half;
        public bool IsColliding;

        public bool IsTrigger;

        public Action<GameObject> OnCollisionStart;
        public Action<GameObject> OnCollision;
        public Action<GameObject> OnCollisionEnd;


        public RectangleCollider(Vector2 size, 
            bool isStatic, 
            CollisionLayer layer = CollisionLayer.All, 
            CollisionLayer layersToCollideWith = CollisionLayer.All, 
            float xOffset = 0, 
            float yOffset = 0, 
            Action<GameObject> onCollisionStart = null,
            Action<GameObject> onCollision = null,
            Action<GameObject> onCollisionEnd = null,
            bool IsTrigger = false
            )
        {
            offset = new Vector2(xOffset, yOffset);
            this.size = size;
            this.Half = size / 2;
            this.isStatic = isStatic;

            this.Layer = layer;
            this.LayersToCollideWith = layersToCollideWith;

            this.OnCollisionStart = onCollisionStart;
            this.OnCollision = onCollision;
            this.OnCollisionEnd = onCollisionEnd;
            this.IsTrigger = IsTrigger;
        }


        public Vector2 CalculateOverlap(Transform selfTransform, Transform otherTransform, RectangleCollider otherCollider)
        {
            // Calculate the horizontal overlap

            float startX = selfTransform.position.X + this.offset.X - this.size.X / 2;
            float endX = startX + this.size.X;

            float startY = selfTransform.position.Y + this.offset.Y - this.size.Y / 2;
            float endY = startY + this.size.Y;

            float otherStartX = otherTransform.position.X + otherCollider.offset.X - otherCollider.size.X / 2;
            float otherEndX = startX + otherCollider.size.X;

            float otherStartY = otherTransform.position.Y + otherCollider.offset.Y - otherCollider.size.Y / 2;
            float otherEndY = startY + otherCollider.size.Y;



            // Calculate the overlap on the X-axis
            float xOverlap = Math.Min(endX, otherEndX) - Math.Max(startX, otherStartX);

            // Calculate the overlap on the Y-axis
            float yOverlap = Math.Min(endY, otherEndY) - Math.Max(startY, otherStartY);




            return new Vector2(xOverlap, yOverlap);
        }
    }
}