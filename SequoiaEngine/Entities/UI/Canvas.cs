using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class Canvas
    {
        public Texture2D BackgroundTexture;
        public Vector2 Position;
        public Vector2 Size;
        public float Rotation;
        public Color SpriteColor = Color.White;
        public AnchorLocation AnchorLocation;
        public ScaleSize Scale;
        public GameObject GameObject { get; private set; }



        /// <summary>
        /// Instantiated differently because it is a entity instead of a prefab. I could have technically done it the same, but I think I like this way better..?
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="backgroundTexture"></param>
        public Canvas(Vector2 position, float rotation, Vector2 size, Texture2D backgroundTexture = null, AnchorLocation anchorLocation = AnchorLocation.None, ScaleSize scale = ScaleSize.None, GameObject parent = null)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Size = size;
            this.BackgroundTexture = backgroundTexture;
            this.AnchorLocation = anchorLocation;
            this.Scale = scale;

            GameObject = new(new Transform(this.Position, this.Rotation, this.Size), parent);
            Setup();
        }


        public Canvas(Vector2 position, float rotation, Vector2 size, string backgroundTextureName = "", AnchorLocation anchorLocation = AnchorLocation.None, ScaleSize scale = ScaleSize.None, GameObject parent = null)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Size = size;
            if (backgroundTextureName != "")
            {
                this.BackgroundTexture = ResourceManager.Get<Texture2D>(backgroundTextureName);
            }
            this.AnchorLocation = anchorLocation;
            this.Scale = scale;

            GameObject = new(new Transform(this.Position, this.Rotation, this.Size, isHUD: true), parent);
            Setup();
        }




        private void Setup()
        {
            // If an anchor is set, it will override the base position by a factor. So if it is "centered" it will find the overall width / height, center it there, then apply the
            // Position as an offset from that location
            // This is the same for the scale component too

            float spriteDrawLocationModification = 1.0f;

            GameObject parent = this.GameObject.GetParent();


            if (parent != null && parent.TryGetComponent(out Sprite parentBackground))
            {
                spriteDrawLocationModification = parentBackground.renderDepth - 0.001f;
            }

            if (this.BackgroundTexture != null)
            {
                GameObject.Add(new Sprite(this.BackgroundTexture, this.SpriteColor, spriteDrawLocationModification));
            }
            if (!this.AnchorLocation.Equals(AnchorLocation.None))
            {
                Anchor anchor = new(this.AnchorLocation);
                GameObject.Add(anchor);


                GameObject.GetComponent<Transform>().position += anchor.GetAnchorPoint(GameObject);
            }

            if (!this.Scale.Equals(ScaleSize.None))
            {
                Scale scale = new(this.Scale);
                GameObject.Add(scale);

                GameObject.GetComponent<Transform>().scale *= scale.GetScaleModifier(GameObject);
            }
        }

    }
}
