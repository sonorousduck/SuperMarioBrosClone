using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace SequoiaEngine
{
    /// <summary>
    /// Updates Input components in the game (does not do anything with it, just updates state)
    /// </summary>
    public class InputSystem : System
    {
        public InputSystem(SystemManager systemManager) : base(systemManager, typeof(Input))
        {
        }

        /// <summary>
        /// Register the 
        /// </summary>
        public override void Start()
        {
            base.Start();

            foreach (uint id in gameObjects.Keys)
            {
                if (gameObjects[id].TryGetComponent(out KeyboardInput keyboardInput))
                {
                    InputConfig.Instance.RegisterKeyboardDefaultConfig(keyboardInput.DefaultBindings);
                }
                if (gameObjects[id].TryGetComponent(out ControllerInput controllerInput))
                {
                    InputConfig.Instance.RegisterControllerDefaultConfig(controllerInput.DefaultBindings);
                }
                if (gameObjects[id].TryGetComponent(out MouseInput mouseInput))
                {
                    InputConfig.Instance.RegisterMouseDefaultConfig(mouseInput.DefaultBindings);
                }
            }
        }
            

        protected override void Update(GameTime gameTime)
        {
            InputConfig inputConfig = InputConfig.Instance;
            InputManager inputState = InputManager.Instance;

            foreach (uint id in gameObjects.Keys)
            {

                // =============================================================================================================
                //
                // Keyboard Input handling
                //
                // =============================================================================================================
                if (gameObjects[id].TryGetComponent(out KeyboardInput keyboardInput))
                {
                    Dictionary<string, Keys> actions = inputConfig.ActionsToKeyboardKeys;

                    foreach (KeyValuePair<string, Action> keyValuePair in keyboardInput.OnPressActions)
                    {
                        if (inputState.KeyboardButtonState[actions[keyValuePair.Key]] == ButtonStateExtended.Pressed)
                        {
                            keyboardInput.OnPressActions[keyValuePair.Key]?.Invoke();
                        }
                    }

                    foreach (KeyValuePair<string, Action> keyValuePair in keyboardInput.OnHeldActions)
                    {
                        if (inputState.KeyboardButtonState[actions[keyValuePair.Key]] == ButtonStateExtended.Held)
                        {
                            keyboardInput.OnHeldActions[keyValuePair.Key]?.Invoke();
                        }
                    }

                    foreach (KeyValuePair<string, Action> keyValuePair in keyboardInput.OnReleaseActions)
                    {
                        if (inputState.KeyboardButtonState[actions[keyValuePair.Key]] == ButtonStateExtended.Released)
                        {
                            keyboardInput.OnReleaseActions[keyValuePair.Key]?.Invoke();
                        }
                    }
                }


                // =============================================================================================================
                //
                // Controller Input handling
                //
                // =============================================================================================================

                if (gameObjects[id].TryGetComponent(out ControllerInput controllerInput))
                {
                    Dictionary<string, Buttons> actions = inputConfig.ActionsToControllerButtons;

                    foreach (KeyValuePair<string, Action> keyValuePair in controllerInput.OnPressActions)
                    {
                        if (inputState.ControllerButtonState[actions[keyValuePair.Key]] == ButtonStateExtended.Pressed)
                        {
                            controllerInput.OnPressActions[keyValuePair.Key]?.Invoke();
                        }
                    }

                    foreach (KeyValuePair<string, Action> keyValuePair in keyboardInput.OnHeldActions)
                    {
                        if (inputState.ControllerButtonState[actions[keyValuePair.Key]] == ButtonStateExtended.Held)
                        {
                        controllerInput.OnHeldActions[keyValuePair.Key]?.Invoke();
                        }
                    }

                    foreach (KeyValuePair<string, Action> keyValuePair in keyboardInput.OnReleaseActions)
                    {
                        if (inputState.ControllerButtonState[actions[keyValuePair.Key]] == ButtonStateExtended.Released)
                        {
                        controllerInput.OnReleaseActions[keyValuePair.Key]?.Invoke();
                        }
                    }
                }

                
                // =============================================================================================================
                //
                // Mouse Input handling
                //
                // =============================================================================================================

                if (gameObjects[id].TryGetComponent(out MouseInput mouseInput))
                {
                    Dictionary<string, MouseButton> actions = inputConfig.ActionsToMouseButtons;

                    foreach (KeyValuePair<string, Action> keyValuePair in mouseInput.OnPressActions)
                    {
                        if (inputState.MouseButtonsState[actions[keyValuePair.Key]] == ButtonStateExtended.Pressed)
                        {
                            mouseInput.OnPressActions[keyValuePair.Key]?.Invoke();
                        }
                    }

                    foreach (KeyValuePair<string, Action> keyValuePair in mouseInput.OnHeldActions)
                    {
                        if (inputState.MouseButtonsState[actions[keyValuePair.Key]] == ButtonStateExtended.Held)
                        {
                            mouseInput.OnHeldActions[keyValuePair.Key]?.Invoke();
                        }
                    }

                    foreach (KeyValuePair<string, Action> keyValuePair in mouseInput.OnReleaseActions)
                    {
                        if (inputState.MouseButtonsState[actions[keyValuePair.Key]] == ButtonStateExtended.Released)
                        {
                            mouseInput.OnReleaseActions[keyValuePair.Key]?.Invoke();
                        }
                    }
                }
            }
        }
    }
}