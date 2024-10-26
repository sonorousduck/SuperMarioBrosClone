using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class TiledRenderingSystem : System
    {
        private TiledMapRenderer renderer;
        private bool hasSetMap = false;
        private float renderDepth = 1.0f;

        public TiledRenderingSystem(SystemManager systemManager) : base(systemManager, typeof(TiledMapComponent)) 
        { }


        public void SetRenderedTiledMap(TiledMapComponent tiledMapComponent)
        {
            if (renderer == null)
            {
                Start();
            }

            this.renderer.LoadMap(tiledMapComponent.TiledMap);
            hasSetMap = true;
            this.renderDepth = tiledMapComponent.RenderDepth;
        }

        public void SetRenderedTiledMap(TiledMap tiledMap)
        {
            this.renderer.LoadMap(tiledMap);
            hasSetMap = true;
        }


        public override void Start()
        {
            if (renderer == null)
            {
                renderer = new TiledMapRenderer(GameManager.Instance.GraphicsDevice);
            }
        }


        protected override void Update(GameTime gameTime)
        {
            foreach ((_, GameObject gameObject) in gameObjects)
            {
                if (!hasSetMap)
                {
                    SetRenderedTiledMap(gameObject.GetComponent<TiledMapComponent>());
                }
            }

            renderer?.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool isDrawingHud = false)
        {
            renderer?.Draw(GameManager.Instance.Camera.GetViewMatrix(), depth: renderDepth);
        }

        
    }
}
