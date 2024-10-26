using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace SequoiaEngine
{


    public class GameManager
    {
        public static GameManager Instance;

        private readonly GraphicsDeviceManager _graphics;
        public readonly GameWindow GameWindow;
        public GraphicsDevice GraphicsDevice { get; private set; }

        public GameTime ElapsedTime;
        public float ElapsedMilliseconds;
        public float ElapsedMicroseconds { get => ElapsedMilliseconds * 1000.0f; }
        public float ElapsedSeconds { get => ElapsedMilliseconds / 1000.0f; }

        public int RenderWidth;
        public int RenderHeight;

        public Vector2 RenderWindowSize;

        public int ActualWidth = 0;
        public int ActualHeight = 0;

        public Vector2 ActualWindowSize;

        public Rectangle DestinationRectangle { get; private set; }


        public OrthographicCamera Camera;


        public GameManager(GraphicsDeviceManager graphics, GameWindow window, int renderWidth, int renderHeight)
        {
            Instance = this;
            this._graphics = graphics;
            this.GameWindow = window;
            this.RenderWidth = renderWidth;
            this.RenderHeight = renderHeight;

            RenderWindowSize = new Vector2(RenderWidth, RenderHeight);
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            var viewportAdapter = new BoxingViewportAdapter(this.GameWindow, this.GraphicsDevice, this.RenderWidth, this.RenderHeight);
            Camera = new OrthographicCamera(viewportAdapter);
        }

        public void UpdateWidthHeight(int width, int height)
        {
            this.ActualWidth = width;
            this.ActualHeight = height;

            ActualWindowSize = new Vector2(width, height);

            float scaleX = (float)width / RenderWidth;
            float scaleY = (float)height / RenderHeight;

            float scale = MathF.Min(scaleX, scaleY);

            int newWidth = (int)(RenderWidth * scale);
            int newHeight = (int)(RenderHeight * scale);

            int posX = (width - newWidth) / 2;
            int posY = (height - newHeight) / 2;

            this.DestinationRectangle = new Rectangle(posX, posY, newWidth, newHeight);

            
        }

        public void Update(GameTime gameTime)
        {
            this.ElapsedTime = gameTime;
            this.ElapsedMilliseconds = gameTime.ElapsedGameTime.Milliseconds;
        }

    }
}
