using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens.Transitions;


namespace SequoiaEngine
{


    /// <summary>
    /// A representative of a screen. Add the names of your screens to this, in the enum. When you want to add a gameObject to this screen, you should call systemManager.Add(gameObject).
    /// </summary>
    public abstract partial class Screen
    {
        public SystemManager systemManager;


        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphicsDevice;
        protected GameWindow window;
        protected ContentManager content;

        /// <summary>
        /// This MUST be invoked with a base() call.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="graphics"></param>
        /// <param name="window"></param>
        /// <param name="screen"></param>
        public virtual void Initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, GameWindow window)
        {
            this.systemManager = new SystemManager();
            this.graphics = graphics;
            this.window = window;
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override abstract void LoadContent();

        /// <summary>
        /// Updates all registered systems. You shouldn't need to modify this
        /// </summary>
        /// <param name="gameTime"></param>
        


        //public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Function that is called the first frame this screen is switched to. Useful for loading and such
        /// </summary>
        public abstract void OnScreenFocus();

        /// <summary>
        /// Function that is called on the last frame this screen is switched to.
        /// </summary>
        public abstract void OnScreenDefocus();

        /// <summary>
        /// Sets up all entities for the given screen. This is called AFTER the onLoad
        /// </summary>
        /// 

        public abstract void SetupGameObjects();


    }
}