using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace SequoiaEngine
{
    /// <summary>
    /// Represents the possible buttons on a mouse. Left middle and right are self explanatory, while x1 and x2 are the potential side mouse buttons
    /// </summary>
    public enum MouseButton
    {
        LeftButton,
        MiddleButton,
        RightButton,
        X1Button,
        X2Button,
        ScrollWheelUp,
        ScrollWheelDown,
    }
    public class MouseInput : Input
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

        public int ScrollPosition = 0;

        public Dictionary<string, Action> OnPressActions = new();
        public Dictionary<string, Action> OnHeldActions = new();
        public Dictionary<string, Action> OnReleaseActions = new();
        public Dictionary<string, MouseButton> DefaultBindings = new();


        public Action OnMouseMove;


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

        public void RegisterOnPressAction(string actionName, Action action)
        {
            if (OnPressActions.ContainsKey(actionName))
            {
                OnPressActions[actionName] += action;
            }
            else
            {
                OnPressActions.Add(actionName, action);
            }

        }

        public void RegisterOnHeldAction(string actionName, Action action)
        {
            if (OnHeldActions.ContainsKey(actionName))
            {
                OnHeldActions[actionName] += action;
            }
            else
            {
                OnHeldActions.Add(actionName, action);
            }
        }

        public void RegisterOnReleaseAction(string actionName, Action action)
        {
            if (OnReleaseActions.ContainsKey(actionName))
            {
                OnReleaseActions[actionName] += action;
            }
            else
            {
                OnReleaseActions.Add(actionName, action);
            }
        }
    }
}