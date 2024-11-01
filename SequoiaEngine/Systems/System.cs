﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    /// <summary>
    /// Abtract implementation of a System for ECS architecture. Taken from Dr. Mathias' ECS example.
    /// </summary>
    public abstract class System
    {
        private static uint nextId = 0;

        public uint Id { get; private set; }


        public System()
        {

        }


        protected Dictionary<uint, GameObject> gameObjects = new Dictionary<uint, GameObject>();
        private bool isEnabled;

        /// <summary>
        /// The types this system is interested in (for example, rendering system wants to see a sprite and transform)
        /// </summary>
        protected Type[] ComponentTypes { get; set; }
        protected SystemManager systemManager;

        public System(SystemManager systemManager, params Type[] componentTypes)
        {
            this.ComponentTypes = componentTypes;
            this.systemManager = systemManager;
            this.systemManager.AddGameObject += Add;
            this.systemManager.RemoveGameObject += Remove;
            this.systemManager.UpdateSystem += Update;
            this.systemManager.StartSystem += Start;
            Id = nextId++;
            isEnabled = true;
        }



        public virtual void Start()
        {

        }


        /// <summary>
        /// When enabled, a system is part of the global update queue
        /// </summary>
        public void Enable()
        {
            if (!isEnabled)
            {
                this.systemManager.UpdateSystem += Update;
            }
            isEnabled = true;
        }

        /// <summary>
        /// When disabled, a system is NOT part of the global update queue. Generally useful for turning off renderers
        /// </summary>
        /// 
        public void Disable()
        {
            if (!isEnabled)
            {
                this.systemManager.UpdateSystem -= Update;
            }
            isEnabled = false;
        }

        /// <summary>
        /// Checks a game object for all require types to subscribe to this system
        /// </summary>
        /// <param name="gameObject">The game object to check</param>
        /// <returns></returns>
        /// 
        protected virtual bool IsInterested(GameObject gameObject)
        {
            foreach (Type type in ComponentTypes)
            {
                if (!gameObject.ContainsComponentOfParentType(type))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Adds a game object to the system. This is private, as adding calls should be done through the system manager to ensure all systems recieve it
        /// </summary>
        /// <param name="gameObject">The game object to add</param>
        protected virtual void Add(GameObject gameObject)
        {
            if (IsInterested(gameObject))
            {
                gameObjects.Add(gameObject.Id, gameObject);
            }
        }

        /// <summary>
        /// Removes a gameobject from the system. Private, as removing calls should be done through the system manager to ensure all systems recieve it
        /// </summary>
        /// <param name="id">ID of the object to remove</param>
        protected virtual void Remove(uint id)
        {
            gameObjects.Remove(id);
        }

        /// <summary>
        /// The Update function for a system. This is protected, as the update will be called using the SystemManager.
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void Update(GameTime gameTime);


    }
}