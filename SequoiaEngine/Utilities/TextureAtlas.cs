using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{ 
    public class TextureAtlas
    {

        public string Name { get; private set; }
        public Texture2D Texture { get; private set; }

        public Vector2 SpriteSize;


        private readonly List<Rectangle> frameRegions = new List<Rectangle>();

        public TextureAtlas(string name, Texture2D texture2D) 
        { 
            this.Name = name;
            this.Texture = texture2D;
        }

        public Rectangle GetRegion(int frameIndex)
        {
            return frameRegions[frameIndex];
        }

        public Rectangle this[int index] => GetRegion(index);



        public void AddRegion(Rectangle region)
        {
            frameRegions.Add(region);
        }

        /// <summary>
        /// Create a Texture Atlas
        /// </summary>
        /// <param name="name">Name of the Texture Atlas</param>
        /// <param name="texture">Texture2D of the spritesheet</param>
        /// <param name="regionWidth">Width of each individual sprite</param>
        /// <param name="regionHeight">Height of each individual sprite</param>
        /// <param name="maxRegionCount">Max number of sprites to pull from the spritesheet</param>
        /// <param name="margin">Padding between the x and y points and the edge of the spritesheet</param>
        /// <param name="spacing">Padding between each sprite in the spritesheet</param>
        public static TextureAtlas CreateUsingSize(string name, Texture2D texture, int regionWidth, int regionHeight, int maxRegionCount = int.MaxValue, Vector2? margin = null, Vector2? spacing = null)
        {
            TextureAtlas textureAtlas = new(name, texture);

            Vector2 correctedMargin = margin ?? Vector2.Zero;
            Vector2 correctedSpacing = spacing ?? Vector2.Zero;
            int numberOfSprites = 0;

            textureAtlas.SpriteSize = new Vector2(regionWidth, regionHeight);

            Vector2 startLocation = correctedMargin;

            float endXLocation = texture.Width - correctedMargin.X;
            float endYLocation = texture.Height - correctedMargin.Y;

            float textureSizeX = regionWidth + correctedSpacing.X;
            float textureSizeY = regionHeight + correctedSpacing.Y;


            int numSpritesInRow = (int)((endXLocation - startLocation.X) / textureSizeX);
            int numSpritesInColumn = (int)((endYLocation - startLocation.Y) / textureSizeY);

            int totalSpriteCount = numSpritesInColumn * numSpritesInRow;

            for (int i = 0; i < totalSpriteCount; i++)
            {
                int startX = (int)(correctedMargin.X + i % numSpritesInRow * textureSizeX);
                int startY = (int)(correctedMargin.Y + i / numSpritesInRow * textureSizeY);

                if (startX >= endXLocation || startY >= endYLocation)
                {
                    break;
                }

                textureAtlas.AddRegion(new Rectangle(startX, startY, regionWidth, regionHeight));
                numberOfSprites++;

                if (numberOfSprites >= maxRegionCount)
                {
                    return textureAtlas;
                }
            }

            return textureAtlas;
        }

        public static TextureAtlas Create(string name, Texture2D texture, int numInColumn, int numInRow, Vector2? margin = null, Vector2? spacing = null)
        {
            return (CreateUsingSize(name, texture, (int)(texture.Width / numInColumn), (int)(texture.Height / numInRow)));
        }


    }
}
