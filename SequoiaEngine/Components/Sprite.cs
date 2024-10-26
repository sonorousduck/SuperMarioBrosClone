using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace SequoiaEngine
{
    public class Sprite : RenderedComponent
    {
        public Texture2D sprite;
        public Color color;
        public Vector2 center;
        public Vector2 size;
        public float renderDepth;
        public Rectangle? SpriteRectangle = null;

        public Texture2DRegion region;


        /// <summary>
        /// Creates a Sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="center"></param>
        /// <param name="renderDepth"></param>
        public Sprite(Texture2D sprite, Color color, Vector2 center, Vector2 spriteSize, float renderDepth = 1)
        {
            this.sprite = sprite;
            this.color = color;
            this.center = center;
            this.renderDepth = renderDepth;
            this.size = spriteSize;
        }


        /// <summary>
        /// Sprite creation convenience method. Only takes the string, does the loading and assumes sprite color of white. For other detailed creation
        /// use the other constructors
        /// </summary>
        public Sprite(string spriteString)
        {
            this.sprite = ResourceManager.Get<Texture2D>(spriteString);
            this.color = Color.White;
            this.center = new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2);
            this.renderDepth = 1;
            this.size = new Vector2(sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Computes the center automatically for you if you use this based on the sprite width and height
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="renderDepth"></param>
        public Sprite(Texture2D sprite) : this(sprite, Color.White, new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2), new Vector2(sprite.Width, sprite.Height), 1)
        {
        }

        public Sprite(Texture2DRegion region)
        {
            this.color = Color.White;
            this.sprite = region.Texture;
            this.region = region;
        }


        /// <summary>
        /// Computes the center automatically for you if you use this based on the sprite width and height
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="renderDepth"></param>
        public Sprite(Texture2D sprite, Color color, float renderDepth = 1) : this(sprite, color, new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2), new Vector2(sprite.Width, sprite.Height), renderDepth)
        {
        }
    }
}