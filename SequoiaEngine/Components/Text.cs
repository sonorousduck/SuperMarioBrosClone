using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace SequoiaEngine
{
    public class Text : Component
    {
        public string text;
        public Color color;
        public Color outlineColor;
        public Vector2 centerOfRotation;
        public SpriteFont spriteFont;
        public BitmapFont bitmapFont;
        public SpriteEffects spriteEffect;
        public float layerDepth;
        public bool renderOutline;

        public Text(string text, Color color, Color outlineColor, BitmapFont bitmapFont = null, SpriteFont spriteFont = null, bool renderOutline = false, float layerDepth = 0f)
        {
            this.text = text;
            this.spriteFont = spriteFont;
            this.bitmapFont = bitmapFont;
            this.color = color;
            this.outlineColor = outlineColor;
            this.renderOutline = renderOutline;
            this.layerDepth = layerDepth;
        }

    }
}