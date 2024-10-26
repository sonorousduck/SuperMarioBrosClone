using System;
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    public class CircleCollider : Collider
    {
        public float radius;

        public CircleCollider(float radius, bool isStatic, float xOffset = 0, float yOffset = 0)
        {
            offset = new Vector2(xOffset, yOffset);
            this.isStatic = isStatic;
            this.radius = radius;

        }
    }
}
