using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SequoiaEngine
{
    public enum ButtonStateExtended
    {
        None,
        Pressed,
        Held,
        Released
    }

    public class MousePositionState
    {
        public float RelativeX = 0;
        public float RelativeY = 0;

        public float PositionX = 0;
        public float PositionY = 0;
        public float PreviousX = 0;
        public float PreviousY = 0;

        public Vector2 Position = new();
        public Vector2 PreviousPosition = new();
        public Vector2 RelativePosition = new();


        public void SetMousePosition(Vector2 absolutePosition, Vector2 relativePosition)
        {
            PreviousX = PositionX;
            PreviousY = PositionY;

            PreviousPosition = Position;
            Position = absolutePosition;
            RelativePosition = relativePosition;



            PositionX = absolutePosition.X;
            PositionY = absolutePosition.Y;

            RelativeX = relativePosition.X;
            RelativeY = relativePosition.Y;
        }
    }

   


    public class InputManager
    {
        public static InputManager Instance { get; private set; }
        public Dictionary<MouseButton, ButtonStateExtended> MouseButtonsState = new();
        public Dictionary<Keys, ButtonStateExtended> KeyboardButtonState = new();
        public Dictionary<Buttons, ButtonStateExtended> ControllerButtonState = new();

        public MousePositionState MousePositionState = new();

        public int ScrollPosition = 0;

        public InputManager()
        {
            Instance = this;

            // Iterate through all mouse buttons and register them
            var mouseButtons = Enum.GetValues(typeof(MouseButton));
            foreach (MouseButton key in mouseButtons)
            {
                MouseButtonsState.Add(key, ButtonStateExtended.None);
            }


            // Iterate through all keyboard buttons and register them
            var values = Enum.GetValues(typeof(Keys));
            foreach (Keys key in values)
            {
                KeyboardButtonState.Add(key, ButtonStateExtended.None);
            }

            // Iterate through all controller buttons and register them
            var controllerButtons = Enum.GetValues(typeof(Buttons));
            foreach (Buttons key in controllerButtons)
            {
                ControllerButtonState.Add(key, ButtonStateExtended.None);
            }
        }

        public void Update()
        {
            // Grab state from keyboard and update everything
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            

            foreach (KeyValuePair<Keys, ButtonStateExtended> keyValuePair in KeyboardButtonState)
            {
                bool isDown = keyboardState.IsKeyDown(keyValuePair.Key);

                if (isDown)
                {
                    if (keyValuePair.Value == ButtonStateExtended.Pressed)
                    {
                        KeyboardButtonState[keyValuePair.Key] = ButtonStateExtended.Held;
                    }
                    else if (keyValuePair.Value == ButtonStateExtended.None)
                    {
                        KeyboardButtonState[keyValuePair.Key] = ButtonStateExtended.Pressed;
                    }
                }
                else
                {
                    if (keyValuePair.Value == ButtonStateExtended.Pressed || keyValuePair.Value == ButtonStateExtended.Held)
                    {
                        KeyboardButtonState[keyValuePair.Key] = ButtonStateExtended.Released;
                    }
                    else
                    {
                        KeyboardButtonState[keyValuePair.Key] = ButtonStateExtended.None;
                    }
                }
            }

            foreach (KeyValuePair<MouseButton, ButtonStateExtended> keyValuePair in MouseButtonsState)
            {
                bool isDown = GetMouseState(mouseState, keyValuePair.Key);

                if (isDown)
                {
                    if (keyValuePair.Value == ButtonStateExtended.Pressed)
                    {
                        MouseButtonsState[keyValuePair.Key] = ButtonStateExtended.Held;
                    }
                    else if (keyValuePair.Value == ButtonStateExtended.None)
                    {
                        MouseButtonsState[keyValuePair.Key] = ButtonStateExtended.Pressed;
                    }
                }
                else
                {
                    if (keyValuePair.Value == ButtonStateExtended.Pressed || keyValuePair.Value == ButtonStateExtended.Held)
                    {
                        MouseButtonsState[keyValuePair.Key] = ButtonStateExtended.Released;
                    }
                    else
                    {
                        MouseButtonsState[keyValuePair.Key] = ButtonStateExtended.None;
                    }
                }
            }


            Vector2 newMouseState = new(mouseState.X, mouseState.Y);
            MousePositionState.SetMousePosition(newMouseState, newMouseState - MousePositionState.PreviousPosition);


            foreach (KeyValuePair<Buttons, ButtonStateExtended> keyValuePair in ControllerButtonState)
            {
                bool isDown = GetControllerState(keyValuePair.Key, gamePadState) == 1;

                if (isDown)
                {
                    if (keyValuePair.Value == ButtonStateExtended.Pressed)
                    {
                        ControllerButtonState[keyValuePair.Key] = ButtonStateExtended.Held;
                    }
                    else if (keyValuePair.Value == ButtonStateExtended.None)
                    {
                        ControllerButtonState[keyValuePair.Key] = ButtonStateExtended.Pressed;
                    }
                }
                else
                {
                    if (keyValuePair.Value == ButtonStateExtended.Pressed || keyValuePair.Value == ButtonStateExtended.Held)
                    {
                        ControllerButtonState[keyValuePair.Key] = ButtonStateExtended.Released;
                    }
                    else
                    {
                        ControllerButtonState[keyValuePair.Key] = ButtonStateExtended.None;
                    }
                }
            }
        }

        private bool GetMouseState(MouseState mouseState, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return mouseState.LeftButton == ButtonState.Pressed;

                case MouseButton.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Pressed;

                case MouseButton.RightButton:
                    return mouseState.RightButton == ButtonState.Pressed;

                case MouseButton.X1Button:
                    return mouseState.XButton1 == ButtonState.Pressed;

                case MouseButton.X2Button:
                    return mouseState.XButton2 == ButtonState.Pressed;


                case MouseButton.ScrollWheelUp:
                    bool changedScrollUp = mouseState.ScrollWheelValue > ScrollPosition;

                    if (changedScrollUp)
                    {
                        ScrollPosition = mouseState.ScrollWheelValue;
                    }

                    return changedScrollUp;

                case MouseButton.ScrollWheelDown:
                    bool changedScrollDown = mouseState.ScrollWheelValue < ScrollPosition;

                    if (changedScrollDown)
                    {
                        ScrollPosition = mouseState.ScrollWheelValue;
                    }

                    return changedScrollDown;
                default:
                    break;
            }


            return false;
        }

        /// <summary>
        /// Very ugly way of reading controller input. Should be improved upon, but allows all input types to be treated as floats
        /// </summary>
        /// <param name="type"></param>
        /// <param name="gamePadState"></param>
        /// <returns></returns>
        private float GetControllerState(Buttons type, GamePadState gamePadState)
        {
            if (gamePadState.IsConnected)
            {
                switch (type)
                {
                    case (Buttons.A):
                        return gamePadState.IsButtonDown(Buttons.A) ? 1 : 0;
                    case (Buttons.B):
                        return gamePadState.IsButtonDown(Buttons.B) ? 1 : 0;
                    case (Buttons.Back):
                        return gamePadState.IsButtonDown(Buttons.Back) ? 1 : 0;
                    case (Buttons.DPadDown):
                        return gamePadState.DPad.Down == ButtonState.Pressed ? 1 : 0;
                    case (Buttons.DPadLeft):
                        return gamePadState.DPad.Left == ButtonState.Pressed ? 1 : 0;
                    case (Buttons.DPadRight):
                        return gamePadState.DPad.Right == ButtonState.Pressed ? 1 : 0;
                    case (Buttons.DPadUp):
                        return gamePadState.DPad.Up == ButtonState.Pressed ? 1 : 0;
                    case (Buttons.LeftShoulder):
                        return gamePadState.IsButtonDown(Buttons.LeftShoulder) ? 1 : 0;
                    case (Buttons.LeftThumbstickLeft):
                        return -gamePadState.ThumbSticks.Left.X;
                    case (Buttons.LeftThumbstickRight):
                        return gamePadState.ThumbSticks.Left.X;
                    case (Buttons.LeftStick):
                        return gamePadState.IsButtonDown(Buttons.LeftStick) ? 1 : 0;
                    case (Buttons.LeftThumbstickUp):
                        return gamePadState.ThumbSticks.Left.Y;
                    case (Buttons.LeftThumbstickDown):
                        return -gamePadState.ThumbSticks.Left.Y;
                    case (Buttons.LeftTrigger):
                        return gamePadState.Triggers.Left;
                    case (Buttons.RightShoulder):
                        return gamePadState.IsButtonDown(Buttons.RightShoulder) ? 1 : 0;
                    case (Buttons.RightThumbstickRight):
                        return gamePadState.ThumbSticks.Right.X;
                    case (Buttons.RightThumbstickDown):
                        return gamePadState.IsButtonDown(Buttons.RightStick) ? 1 : 0;
                    case (Buttons.RightThumbstickUp):
                        return gamePadState.ThumbSticks.Right.Y;
                    case (Buttons.RightTrigger):
                        return gamePadState.Triggers.Right;
                    case (Buttons.Start):
                        return gamePadState.IsButtonDown(Buttons.Start) ? 1 : 0;
                    case (Buttons.X):
                        return gamePadState.IsButtonDown(Buttons.X) ? 1 : 0;
                    case (Buttons.Y):
                        return gamePadState.IsButtonDown(Buttons.Y) ? 1 : 0;
                    default:
                        Debug.WriteLine("Found a controller input type that was non-existant, returning 0");
                        return 0;
                }

            }
            return 0;
        }
    }
}
