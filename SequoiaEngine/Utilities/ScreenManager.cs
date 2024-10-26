using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens.Transitions;
using System.Collections.Generic;

namespace SequoiaEngine
{
    public class ScreenManager
    {
        public static ScreenManager Instance { get; private set; }

        public Screen CurrentScreen { get; private set; }
        public ScreenEnum NextScreen { get; private set; }
        public ScreenEnum CurrentScreenEnum { get; private set; }
        public bool NewScreenFocused { get; private set; }

        public bool IsLeavingScreen { get; private set; }
        public bool IsStartingScreen { get; private set; }

        public Dictionary<ScreenEnum, Screen> Screens { get; private set; } = new Dictionary<ScreenEnum, Screen>();
        public SpriteBatch SpriteBatch { get; private set; }

        private Transition activeTransition;

        public SequoiaEngine.SystemManager SystemManager { get; set; }    

        public ScreenManager()
        {
            Instance = this;
        }

        public void SetNextScreen(ScreenEnum screen, Transition transition = null)
        {
            NextScreen = screen;

            if (activeTransition == null && transition != null)
            {
                CurrentScreen.OnScreenDefocus();
                
                activeTransition = transition;
                activeTransition.StateChanged += delegate
                {
                    SetCurrentScreen(screen);
                    IsLeavingScreen = false;
                };
                activeTransition.Completed += delegate
                {
                    activeTransition.Dispose();
                    activeTransition = null;

                    CurrentScreen.Start();
                    CurrentScreen.OnScreenFocus();
                };
            }
            else if (transition == null)
            {
                SetCurrentScreen(screen);
                CurrentScreen.Start();
                CurrentScreen.OnScreenFocus();
            }
        }

        private void SetCurrentScreen(ScreenEnum newScreen = ScreenEnum.None)
        {
            if (newScreen.Equals(ScreenEnum.Quit))
            {
                NextScreen = newScreen;
                return;
            }

            if (!newScreen.Equals(ScreenEnum.None))
            {
                NextScreen = newScreen;
                CurrentScreen = Screens[newScreen];
                CurrentScreenEnum = newScreen;
                SystemManager = CurrentScreen.systemManager;
            }

            else
            {
                CurrentScreen = Screens[NextScreen];
                CurrentScreenEnum = NextScreen;
            }

            NewScreenFocused = true;
        }

        public void Update(GameTime gameTime)
        {

            if (activeTransition != null)
            {
                activeTransition.Update(gameTime);
            }

            CurrentScreen.Update(gameTime);
            
        }

        public void Draw(GameTime gameTime)
        {



            CurrentScreen.Draw(gameTime);

            if (activeTransition != null)
            {
                activeTransition.Draw(gameTime);
            }
        }


        public void PreUpdate()
        {
/*            if (CurrentScreenEnum != NextScreen)
            {
                SetCurrentScreen();
            }*/
        }

    }
}
