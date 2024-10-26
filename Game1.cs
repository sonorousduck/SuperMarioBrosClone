using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens.Transitions;
using SequoiaEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace MarioClone
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager screenManager;

        const int VIRTUAL_WIDTH = 640;
        const int VIRTUAL_HEIGHT = 360; // Aspect ratio of 16:9

        InputManager inputManager;
        InputConfig inputConfig;
        GameManager gameManager;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            inputManager = new();
            inputConfig = new();
            gameManager = new(_graphics, Window, VIRTUAL_WIDTH, VIRTUAL_HEIGHT);

            screenManager = new();

            inputConfig.LoadControls();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            gameManager.UpdateWidthHeight(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            ResourceManager.Manager = Content;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnWindowResize;

            screenManager.Screens.Add(ScreenEnum.Test, new TestScreen(this, ScreenEnum.Test));
            screenManager.Screens.Add(ScreenEnum.MainMenu, new MainMenuScreen(this, ScreenEnum.MainMenu));

            IsMouseVisible = false;
            GameManager.Instance.Initialize(GraphicsDevice);


            base.Initialize();

            screenManager.SetNextScreen(ScreenEnum.Test);
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            gameManager.UpdateWidthHeight(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            foreach (ScreenEnum screen in screenManager.Screens.Keys)
            {
                screenManager.Screens[screen].Initialize(GraphicsDevice, _graphics, Window);
            }

            foreach (ScreenEnum screen in screenManager.Screens.Keys)
            {
                screenManager.Screens[screen].LoadContent();
                screenManager.Screens[screen].SetupGameObjects();
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                screenManager.SetNextScreen(ScreenEnum.MainMenu, new FadeTransition(GameManager.Instance.GraphicsDevice, Color.Black, 1));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                screenManager.SetNextScreen(ScreenEnum.Test, new FadeTransition(GameManager.Instance.GraphicsDevice, Color.Black, 1));
            }

            if (screenManager.NextScreen.Equals(ScreenEnum.Quit))
            {
                Exit();
            }

            gameManager.Update(gameTime);
            inputManager.Update();
            // Diplays FPS
            //Debug.WriteLine(1 / gameTime.ElapsedGameTime.TotalSeconds);


            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
