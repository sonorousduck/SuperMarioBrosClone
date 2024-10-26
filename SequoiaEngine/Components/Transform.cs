using System;
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    public class Transform : Component
    {
        public Vector2 position;
        public float rotation;
        public Vector2 scale;
        public Vector2 previousPosition;

        /// <summary>
        /// This is only used if the transform is a child. Position is kept static using it.
        /// </summary>
        public Vector2 Offset;

        public bool IsHUD = false;

        public Transform()
        {
            this.position = Vector2.Zero;
            this.rotation = 0;
            this.scale = Vector2.One;
            this.previousPosition = Vector2.Zero;
        }

        public Transform(bool isHUD)
        {
            this.position = Vector2.Zero;
            this.rotation = 0;
            this.scale = Vector2.One;
            this.previousPosition = Vector2.Zero;
            this.IsHUD = isHUD;
        }

        public Transform(Vector2 position, float rotation, Vector2 scale, bool isHUD = false)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.previousPosition = position;
            this.IsHUD = isHUD;
        }
    }
}