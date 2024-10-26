using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace SequoiaEngine
{
    public enum ScreenEnum
    {
        None,
        Default,
        Game,
        MainMenu,
        Controls,
        Credits,
        PauseScreen,
        Quit,
        CameraTest,
        Test
    }

    public partial class Screen : GameScreen
    {
        protected ScreenEnum screenName;
        protected ScreenEnum currentScreen;



        protected Screen(Game game, ScreenEnum screenEnum) : base(game)
        {

            this.screenName = screenEnum;
            this.currentScreen = screenEnum;
        }

        public void Start()
        {
            systemManager.Start();
        }
        
        public override void Update(GameTime gameTime)
        {
            systemManager.Update(gameTime);
        }

        protected void SetCurrentScreen(ScreenEnum screenEnum)
        {
            currentScreen = screenEnum;
        }

        public delegate void SetCurrentScreenDelegate(ScreenEnum screenEnum);
    }
}
