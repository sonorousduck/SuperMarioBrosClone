using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    public class ControllerInput : Input
    {
        /// <summary>
        /// Each of these map a function name to an action.
        /// Each frame, we will loop through the strings on each action, ask the InputConfig for
        /// the key that is associated with it, and check its state. If its state is the same as the action type
        /// (i.e. PRESSED for OnPressActions) it will then call the Action
        /// </summary>
        public Dictionary<string, Action> OnPressActions = new();
        public Dictionary<string, Action> OnHeldActions = new();
        public Dictionary<string, Action> OnReleaseActions = new();
        public Dictionary<string, Buttons> DefaultBindings = new();

        public PlayerIndex PlayerIndex;


        public ControllerInput(PlayerIndex controllerOwner = PlayerIndex.One)
        {
           PlayerIndex = controllerOwner;
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