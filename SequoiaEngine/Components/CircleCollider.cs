using System;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace SequoiaEngine
{
    public class CircleCollider : Collider
    {
        public float radius;
        public CircleCollider(float radius, bool isStatic, float xOffset = 0, float yOffset = 0, float density = 1f, BodyType bodyType = BodyType.Static)
        {
            offset = new Vector2(xOffset, yOffset);
            this.isStatic = isStatic;
            this.radius = radius;
            this.density = density;
            this.bodyType = bodyType;

        }

        public override void CreateBody(World world, Transform position)
        {
            this.Body = world.CreateCircle(this.radius, this.density, position.position + this.offset, this.bodyType);
        }


    }
}
