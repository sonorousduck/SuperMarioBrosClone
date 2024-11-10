using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

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
        public OnCollisionEventHandler OnCollision;
        public OnSeparationEventHandler OnCollisionEnd;

        public HashSet<Body> ActiveCollisions;


        public RectangleCollider(Vector2 size, 
            bool isStatic, 
            CollisionLayer layer = CollisionLayer.All, 
            CollisionLayer layersToCollideWith = CollisionLayer.All, 
            float xOffset = 0, 
            float yOffset = 0, 
            Action<GameObject> onCollisionStart = null,
            OnCollisionEventHandler onCollision = null,
            OnSeparationEventHandler onCollisionEnd = null,
            bool IsTrigger = false,
            float density = 1f, 
            BodyType bodyType = BodyType.Static
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
            this.bodyType = bodyType;
            this.density = density;

            this.ActiveCollisions = new();
        }

        private bool DefaultCollisionEventHandler(Fixture fixture, Fixture other, Contact contact)
        {
            IsColliding = true;

            this.ActiveCollisions.Add(other.Body);

            return true;
        }

        private void DefaultSeparationEventHandler(Fixture fixture, Fixture other, Contact contact)
        {
            this.ActiveCollisions.Remove(other.Body);

            if (this.ActiveCollisions.Count == 0)
            {
                IsColliding = false;
            }
        }

        

        public override void CreateBody(World world, Transform position)
        {
            this.Body = world.CreateRectangle(this.size.X, this.size.Y, this.density, position.position + this.offset, position.rotation, this.bodyType);
            this.Body.OnCollision += this.OnCollision;
            this.Body.OnCollision += DefaultCollisionEventHandler;

            this.Body.OnSeparation += this.OnCollisionEnd;
            this.Body.OnSeparation += DefaultSeparationEventHandler;
        }
    }
}