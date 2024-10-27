using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using SequoiaEngine;
using MonoGame.Extended.Particles;

using MonoGame.Extended.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework.Audio;



namespace MarioClone
{
    class World1_1 : Screen
    {

        private RenderingSystem renderingSystem;
        private SpritesheetAnimationSystem animationSystem;
        private PhysicsSystem physicsEngine;
        private ParticleRenderingSystem particleRenderer;
        private ParticleSystem particleSystem;
        private InputSystem inputSystem;
        private ScriptSystem scriptSystem;
        private AnimatedSystem animatedSystem;
        private CooldownSystem cooldownSystem;
        private GravitySystem gravitySystem; 

        private AudioSystem audioSystem;

        private TiledRenderingSystem tiledRenderingSystem;
        private int TILESIZE = 16;


        private RenderTarget2D mainRenderTarget;
        // private LightRenderingSystem lightRenderer;

        private GameObject camera;
        private FontRenderingSystem fontRenderingSystem;


        private Texture2D tilesetTexture;

        private RenderTarget2D renderTarget;
        private RenderTarget2D lightRenderTarget;
        private RenderTarget2D tileRenderTarget;
        private RenderTarget2D hudRenderTarget;

        private GameObject player;

        public World1_1(Game game, ScreenEnum screenEnum) : base(game, screenEnum) { }

        public override void Initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, GameWindow window)
        {
            base.Initialize(graphicsDevice, graphics, window);

            physicsEngine = new PhysicsSystem(systemManager);
            inputSystem = new InputSystem(systemManager);
            scriptSystem = new ScriptSystem(systemManager);
            particleSystem = new ParticleSystem(systemManager);
            particleRenderer = new ParticleRenderingSystem(systemManager);
            animatedSystem = new AnimatedSystem(systemManager);
            audioSystem = new AudioSystem(systemManager);
            tiledRenderingSystem = new TiledRenderingSystem(systemManager);
            cooldownSystem = new CooldownSystem(systemManager);
            animationSystem = new SpritesheetAnimationSystem(systemManager);
            camera = CameraPrefab.Create();


            mainRenderTarget = new RenderTarget2D(graphicsDevice, 640, 360, false, SurfaceFormat.Color, DepthFormat.None, graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
            tileRenderTarget = new RenderTarget2D(graphicsDevice, 640, 360, false, SurfaceFormat.Color, DepthFormat.None, graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
            hudRenderTarget = new RenderTarget2D(graphicsDevice, 640, 360, false, SurfaceFormat.Color, DepthFormat.None, graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
            systemManager.Add(camera);
        }


        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.SetRenderTarget(tileRenderTarget);
            graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: GameManager.Instance.Camera.GetViewMatrix());


            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);



            graphics.GraphicsDevice.SetRenderTarget(mainRenderTarget);
            graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            graphics.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: GameManager.Instance.Camera.GetViewMatrix());


            tiledRenderingSystem.Draw(gameTime, spriteBatch);
            renderingSystem.Draw(gameTime, spriteBatch);
            particleRenderer.Draw(gameTime, spriteBatch);
            fontRenderingSystem.Draw(gameTime, spriteBatch);


            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);

            graphics.GraphicsDevice.SetRenderTarget(hudRenderTarget);
            graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            graphics.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

            renderingSystem.Draw(gameTime, spriteBatch, true);
            particleRenderer.Draw(gameTime, spriteBatch, true);
            fontRenderingSystem.Draw(gameTime, spriteBatch, true);


            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);



            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointWrap);

            spriteBatch.Draw(tileRenderTarget, GameManager.Instance.DestinationRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.Draw(mainRenderTarget, GameManager.Instance.DestinationRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(hudRenderTarget, GameManager.Instance.DestinationRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.End();
        }


        public override void LoadContent()
        {

            renderingSystem = new RenderingSystem(systemManager, window.ClientBounds.Height, camera, new Vector2(window.ClientBounds.Width, window.ClientBounds.Height));
            renderingSystem.debugMode = false;
            fontRenderingSystem = new FontRenderingSystem(systemManager, camera);


            ResourceManager.Load<Texture2D>("Sprites/MarioRight", "marioRight");



            //lightRenderer = new LightRenderingSystem(systemManager, camera, graphicsDevice);
            //lightRenderer.globalLightLevel = 0f;
        }

        public override void OnScreenDefocus()
        {
            Debug.WriteLine("Test Screen was unloaded");
        }

        public override void OnScreenFocus()
        {
            systemManager.StartSystems();
            inputSystem.Start();
            physicsEngine.Start();
            inputSystem.Start();
            scriptSystem.Start();
            particleSystem.Start();
            particleRenderer.Start();
            animationSystem.Start();
            animatedSystem.Start();
        }

        /// <summary>
        /// Note, while this one creates gameObjects manually inline, this should really be done in a separate file, in a static class.
        /// The reason this is done this way here, is so that any naming conventions you'd like to have don't conflict
        /// </summary>
        public override void SetupGameObjects()
        {
            SequoiaEngine.ScreenManager.Instance.SystemManager = systemManager;
            GameObject tiledMap = new();

            TiledMapComponent tiledMapComponent = new(Content.Load<TiledMap>("Maps/World1-1"));

            systemManager.Add(tiledMapComponent.GetCollisionLayer("Ground Collisions"));


            tiledMap.Add(tiledMapComponent);


            systemManager.Add(tiledMap);


            GameObject player = Player.Create(new Vector2(10, 17 * TILESIZE), Vector2.One);

            systemManager.Add(player);

            //GameObject cursor = CursorPrefab.Create(new Vector2(100, 100), Vector2.One, player);
            //systemManager.Add(cursor);

            camera.GetComponent<CameraScript>().SetFollow(player);

        }
    }
}