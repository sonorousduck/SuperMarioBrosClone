/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using SequoiaEngine.Utilities;

namespace SequoiaEngine
{
    class DefaultScreen : Screen
    {

        private RenderingSystem renderingSystem;
        private PhysicsSystem physicsEngine;
        private ParticleRenderingSystem particleRenderer;
        private ParticleSystem particleSystem;
        private InputSystem inputSystem;
        private ScriptSystem scriptSystem;
        private LightRenderingSystem lightRenderer;
        RenderTarget2D mainRenderTarget;
        private GameObject camera;
        private FontRenderingSystem fontRenderingSystem;
        private TileRenderingSystem tileRenderingSystem;
        private Transform sharedTransform;

        private TiledMap map;
        private Dictionary<int, TiledTileset> tilesets;
        private Texture2D tilesetTexture;

        private RenderTarget2D renderTarget;
        private RenderTarget2D lightRenderTarget;
        private RenderTarget2D tileRenderTarget;

        private GameObject player;
        public DefaultScreen(ScreenEnum screenEnum) : base(screenEnum) { }

        public override void Initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, GameWindow window)
        {
            base.Initialize(graphicsDevice, graphics, window);

            physicsEngine = new PhysicsSystem(systemManager);
            inputSystem = new InputSystem(systemManager);
            scriptSystem = new ScriptSystem(systemManager);
            camera = new GameObject();
            camera.Add(new Transform(Vector2.Zero, 0, Vector2.One));
            camera.Add(new Rigidbody());
            camera.Add(new CircleCollider(1, false));
            camera.Add(new MouseInput());
            camera.Add(new CameraScript(camera));
            particleSystem = new ParticleSystem(systemManager);
            mainRenderTarget = new RenderTarget2D(graphicsDevice, 640, 360, false, SurfaceFormat.Color, DepthFormat.None, graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
            tileRenderTarget = new RenderTarget2D(graphicsDevice, 640, 360, false, SurfaceFormat.Color, DepthFormat.None, graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);

            systemManager.Add(camera);
        }

        private void LoadTileMapColliders()
        {
            TiledLayer collisionLayers = map.Layers.First(layer => layer.name == "Ground");

            foreach (TiledObject collider in collisionLayers.objects)
            {
                GameObject tileCollider = GenericTileCollider.Create(new Vector2(collider.x + collider.width / 2, collider.y + collider.height / 2), new Vector2(collider.width, collider.height));
                systemManager.Add(tileCollider);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.SetRenderTarget(tileRenderTarget);
            graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

            tileRenderingSystem.Draw(gameTime, spriteBatch, map, tilesets, tilesetTexture);

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);



            graphics.GraphicsDevice.SetRenderTarget(mainRenderTarget);
            graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            graphics.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);
            renderingSystem.Draw(gameTime, spriteBatch);
            particleRenderer.Draw(gameTime, spriteBatch);
            fontRenderingSystem.Draw(gameTime, spriteBatch);


            spriteBatch.End();


            graphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                        SamplerState.LinearClamp, DepthStencilState.Default,
                        RasterizerState.CullNone);

            spriteBatch.Draw(tileRenderTarget, new Rectangle(0, 0, window.ClientBounds.Width, window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(mainRenderTarget, new Rectangle(0, 0, window.ClientBounds.Width, window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.End();


            *//*            lightRenderer.Draw(gameTime, lightRenderTarget);
            */

            /*            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            */

            /*graphicsDevice.SetRenderTarget(null);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(lightRenderTarget);

            spriteBatch.Begin(SpriteSortMode.Immediate, LightRenderingSystem.multiplyBlend);

            spriteBatch.Draw(lightRenderTarget, new Rectangle(0, 0, window.ClientBounds.Width, window.ClientBounds.Height), Color.White);

            graphicsDevice.SetRenderTarget(null);

            spriteBatch.End();*//*
        }




        public override void LoadContent()
        {
            ResourceManager.RegisterTexture("Images/crow", "crow");
            ResourceManager.RegisterTexture("Images/fire", "fire");
            ResourceManager.RegisterTexture("TileMaps/TileMap-Sheet", "DefaultMap");
            ResourceManager.RegisterTexture("Images/Smelter", "Smelter");


            map = new TiledMap("Content/TileMaps/untitled1.tmx");
            // Loads in the TSX
            tilesets = map.GetTiledTilesets("Content/TileMaps/");

            tilesetTexture = ResourceManager.GetTexture("DefaultMap");
            LoadTileMapColliders();

            renderingSystem = new RenderingSystem(systemManager, window.ClientBounds.Height, camera, new Vector2(window.ClientBounds.Width, window.ClientBounds.Height));
            tileRenderingSystem = new TileRenderingSystem(systemManager, camera, new Vector2(window.ClientBounds.Width, window.ClientBounds.Height));
            particleRenderer = new ParticleRenderingSystem(systemManager, camera);
            fontRenderingSystem = new FontRenderingSystem(systemManager, camera);
            lightRenderer = new LightRenderingSystem(systemManager, camera, graphicsDevice);
            lightRenderer.globalLightLevel = 0f;

        }

        public override void OnScreenDefocus()
        {
            Debug.WriteLine("Default Screen was unloaded");
        }

        public override void OnScreenFocus()
        {
            Debug.WriteLine("Default Screen was loaded");
        }

        /// <summary>
        /// Note, while this one creates gameObjects manually inline, this should really be done in a separate file, in a static class.
        /// The reason this is done this way here, is so that any naming conventions you'd like to have don't conflict
        /// </summary>
        public override void SetupGameObjects()
        {
            GameObject player = new();
            KeyboardInput keyboardInput = new();
            keyboardInput.actionKeyPairs.Add("MoveUp", Keys.W);
            keyboardInput.actionKeyPairs.Add("MoveDown", Keys.S);
            keyboardInput.actionKeyPairs.Add("MoveLeft", Keys.A);
            keyboardInput.actionKeyPairs.Add("MoveRight", Keys.D);

            *//*            ParticleGroup particleGroup = new ParticleGroup(ResourceManager.GetTexture("fire"));
                        particleGroup.rate = TimeSpan.FromMilliseconds(50);
                        particleGroup.maxSpeed = 100;
                        particleGroup.minSpeed = 100;
                        particleGroup.minScale = 1;
                        particleGroup.maxScale = 4;
                        particleGroup.rotationSpeed = 1;
                        particleGroup.renderDepth = 1;
                        particleGroup.maxLifeSpan = TimeSpan.FromSeconds(1);
                        particleGroup.emissionArea = Vector2.One * 5;

                        player.Add(particleGroup);*//*
            player.Add(keyboardInput);
            player.Add(new BasicTestScript(player));
            player.Add(new Transform(Vector2.One * 240, 0, Vector2.One));
            player.Add(new Sprite(ResourceManager.GetTexture("crow"), Color.White, 0));
            player.Add(new Rigidbody());
            player.Add(new CircleCollider(10, false));
            player.Add(new Light(Color.White, 1f, 100));

            systemManager.Add(player);

            camera.GetComponent<CameraScript>().SetFollow(player);

            GameObject testParticles = new GameObject();

            ParticleGroup particle = new(ResourceManager.GetTexture("fire"));

            particle.maxSystemLifeSpan = TimeSpan.FromMilliseconds(2000);
            particle.maxSpeed = 100;
            particle.emissionArc = new Vector2(-45, 45);
            particle.rate = TimeSpan.FromMilliseconds(200);
            particle.minScale = 0.5f;
            particle.maxScale = 1;
            particle.maxSystemLifeSpan = TimeSpan.MaxValue;
            particle.emissionArea = Vector2.One * 10;
            particle.maxLifeSpan = TimeSpan.FromMilliseconds(1000);

            testParticles.Add(particle);
            testParticles.Add(new Transform(Vector2.Zero, 0, Vector2.One));

            systemManager.Add(testParticles);


            GameObject testText = new GameObject();

            testText.Add(new Text("Test Text", ResourceManager.GetFont("default"), Color.White, Color.Black, true));
            testText.Add(new Transform(new Vector2(0, 50), 0, Vector2.One));

            systemManager.Add(testText);
        }
    }
}*/