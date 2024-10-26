using System;
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    public class RectangleCollider : Collider
    {
        // Centered at object's position
        public Vector2 size;
        public bool IsColliding;

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
            Action<GameObject> onCollisionEnd = null
            )
        {
            offset = new Vector2(xOffset, yOffset);
            this.size = size;
            this.isStatic = isStatic;

            this.Layer = layer;
            this.LayersToCollideWith = layersToCollideWith;

            this.OnCollisionStart = onCollisionStart;
            this.OnCollision = onCollision;
            this.OnCollisionEnd = onCollisionEnd;
        }
    }
}