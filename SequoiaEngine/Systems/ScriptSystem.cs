using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SequoiaEngine
{
    public class ScriptSystem : System
    {
        public ScriptSystem(SystemManager systemManager) : base(systemManager, typeof(Script))
        {
            foreach (uint id in gameObjects.Keys)
            {
                gameObjects[id].GetComponent<Script>().Start();
            }
        }

        protected override void Add(GameObject gameObject)
        {
            if (IsInterested(gameObject))
            {
                gameObject.GetComponent<Script>().Start();
            }
            base.Add(gameObject);
        }

        protected override void Remove(uint id)
        {
            if (gameObjects.ContainsKey(id))
            {
                gameObjects[id].GetComponent<Script>().Destroyed();
            }
            base.Remove(id);
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (uint id in gameObjects.Keys)
            {
                CallInputScripts(id);
                gameObjects[id].GetComponent<Script>().Update(gameTime);
            }
        }

        private void CallInputScripts(uint id)
        {
        }


    }
}