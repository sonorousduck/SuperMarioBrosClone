using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SequoiaEngine.Utilities;

namespace SequoiaEngine
{
    public class LightRenderingSystem : System
    {
        private Texture2D blackBackground;
        private Texture2D lightTexture;
        private Texture2D whiteBackground;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private float lightScaleFactor;
        private GameObject camera;
        public float globalLightLevel;

        public static BlendState multiplyBlend = new BlendState
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero
        };

        public LightRenderingSystem(SystemManager systemManager, GameObject camera, GraphicsDevice graphicsDevice) : base(systemManager, typeof(Transform), typeof(Light))
        {
            systemManager.UpdateSystem -= Update;
            //ResourceManager.Load<Texture2D>("Images/light", "light");
            //ResourceManager.Load<Texture2D>("Images/black", "black");
            //ResourceManager.Load<Texture2D>("Images/white", "white");

            //blackBackground = ResourceManager.Get<Texture2D>("black");
            //lightTexture = ResourceManager.Get<Texture2D>("light");
            //whiteBackground = ResourceManager.Get<Texture2D>("white");
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.graphicsDevice = graphicsDevice;


            lightScaleFactor = 2f / lightTexture.Width;
            this.camera = camera;
        }
        protected override void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, RenderTarget2D renderTarget)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            spriteBatch.Begin(blendState: BlendState.Additive);

            spriteBatch.Draw(blackBackground, Vector2.One * 500, null, Color.White, 0, Vector2.One, 40, SpriteEffects.None, 1);
            spriteBatch.Draw(whiteBackground, new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight) / 2f, null, new Color(Color.White, globalLightLevel), 0, new Vector2(whiteBackground.Width, whiteBackground.Height) / 2f, 40, SpriteEffects.None, 1);

            foreach (uint id in gameObjects.Keys)
            {
                Light light = gameObjects[id].GetComponent<Light>();
/*                Vector2 distanceFromCenter = gameObjects[id].GetComponent<Transform>().position - camera.GetComponent<Transform>().position;

                Vector2 renderDistanceFromCenter = distanceFromCenter;
                Vector2 trueRenderingPosition = renderDistanceFromCenter;*/

                spriteBatch.Draw(lightTexture, gameObjects[id].GetComponent<Transform>().position, null, new Color(light.color, light.intensity), 0, new Vector2(lightTexture.Width, lightTexture.Height) / 2f, lightScaleFactor * light.range, SpriteEffects.None, 1);
            }
            spriteBatch.End();
        }
    }
}