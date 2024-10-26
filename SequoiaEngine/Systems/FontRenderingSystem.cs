using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace SequoiaEngine
{
    public class FontRenderingSystem : System
    {
        private GameObject camera;

        public FontRenderingSystem(SystemManager systemManager, GameObject camera) : base(systemManager, typeof(Text), typeof(Transform))
        {

            systemManager.UpdateSystem -= Update;
            this.camera = camera;
        }

        private void DrawBackground(Text text, Transform transform, Vector2 trueRenderPosition, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(text.spriteFont, text.text, new Vector2(trueRenderPosition.X + 1, trueRenderPosition.Y), text.outlineColor, transform.rotation, text.centerOfRotation, transform.scale, text.spriteEffect, text.layerDepth + 1);
            spriteBatch.DrawString(text.spriteFont, text.text, new Vector2(trueRenderPosition.X - 1, trueRenderPosition.Y), text.outlineColor, transform.rotation, text.centerOfRotation, transform.scale, text.spriteEffect, text.layerDepth + 1);
            spriteBatch.DrawString(text.spriteFont, text.text, new Vector2(trueRenderPosition.X, trueRenderPosition.Y + 1), text.outlineColor, transform.rotation, text.centerOfRotation, transform.scale, text.spriteEffect, text.layerDepth + 1);
            spriteBatch.DrawString(text.spriteFont, text.text, new Vector2(trueRenderPosition.X, trueRenderPosition.Y - 1), text.outlineColor, transform.rotation, text.centerOfRotation, transform.scale, text.spriteEffect, text.layerDepth + 1);
        }

        protected override void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool isDrawingHud = false)
        {
            foreach (uint id in gameObjects.Keys)
            {
                Text text = gameObjects[id].GetComponent<Text>();
                Transform transform = gameObjects[id].GetComponent<Transform>();

                if (isDrawingHud != transform.IsHUD)
                {
                    return;
                }

                if (text.bitmapFont != null)
                {
                    spriteBatch.DrawString(text.bitmapFont, text.text, gameObjects[id].GetComponent<Transform>().position, text.color, transform.rotation, text.centerOfRotation, transform.scale, text.spriteEffect, text.layerDepth);
                }
                else
                {
                    if (text.renderOutline)
                    {
                        DrawBackground(text, transform, gameObjects[id].GetComponent<Transform>().position, spriteBatch);
                    }

                    spriteBatch.DrawString(text.spriteFont, text.text, gameObjects[id].GetComponent<Transform>().position, text.color, transform.rotation, text.centerOfRotation, transform.scale, text.spriteEffect, text.layerDepth);
                }
            }
        }
    }
}