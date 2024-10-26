using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SequoiaEngine;


namespace SequoiaEngine
{
    public class SystemManager
    {
        public event Action<GameObject> AddGameObject;
        public event Action StartSystem;
        public event Action<uint> RemoveGameObject;
        public event Action<GameTime> UpdateSystem;
        private Queue<GameObject> toAddObjects = new Queue<GameObject>();
        private Queue<uint> toRemoveObjects = new Queue<uint>();

        private Queue<System> toAddSystems = new();
        private Queue<uint> toRemoveSystems = new Queue<uint>();

        public Dictionary<uint, GameObject> gameObjectsDictionary = new Dictionary<uint, GameObject>();

        public Dictionary<uint, System> systemObjectsDictionary = new();

        /// <summary>
        /// Adds a new gameobject to all systems.
        /// </summary>
        /// <param name="gameObject"></param>
        public void Add(GameObject gameObject)
        {
            toAddObjects.Enqueue(gameObject);
        }


        /// <summary>
        /// Convenience method to automatically add a list of game objects
        /// </summary>
        /// <param name="gameObjects"></param>
        public void Add(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Add(gameObject);
            }
        }

        /// <summary>
        /// Adds a system to be tracked by the System manager. Useful for calling system.Start()
        /// </summary>
        /// <param name="system"></param>
        public void AddSystem(System system)
        {
            toAddSystems.Enqueue(system);
        }


        /// <summary>
        /// Removes a system from being tracked.
        /// </summary>
        /// <param name="system"></param>
        public void RemoveSystem(System system)
        {
            toRemoveSystems.Enqueue(system.Id);
        }

        /// <summary>
        /// Adds to a deletion queue to be removed at the end of a Update Loop
        /// </summary>
        public void Remove(GameObject gameObject)
        {
            toRemoveObjects.Enqueue(gameObject.Id);
        }

        public void Remove(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                toRemoveObjects.Enqueue(gameObject.Id);
            }
        }

        public void StartSystems()
        {
            StartSystem?.Invoke();
        }

        public void Start()
        {
            StartSystem?.Invoke();
            while (toAddSystems.Count > 0)
            {
                AddSystemSafe(toAddSystems.Dequeue());
            }

            while (toRemoveSystems.Count > 0)
            {
                RemoveSystemSafe(toRemoveSystems.Dequeue());
            }

            while (toAddObjects.Count > 0)
            {
                AddObject(toAddObjects.Dequeue());
            }
            while (toRemoveObjects.Count > 0)
            {
                uint idToRemove = toRemoveObjects.Dequeue();
                gameObjectsDictionary.Remove(idToRemove);
                RemoveObject(idToRemove);
            }

            
        }

        public void Update(GameTime gameTime)
        {
            UpdateSystem?.Invoke(gameTime);

            while (toAddSystems.Count > 0)
            {
                AddSystemSafe(toAddSystems.Dequeue());
            }

            while (toRemoveSystems.Count > 0)
            {
                RemoveSystemSafe(toRemoveSystems.Dequeue());
            }

            while (toAddObjects.Count > 0)
            {
                AddObject(toAddObjects.Dequeue());
            }
            while (toRemoveObjects.Count > 0)
            {
                uint idToRemove = toRemoveObjects.Dequeue();
                gameObjectsDictionary.Remove(idToRemove);
                RemoveObject(idToRemove);
            }
        }

        private void AddSystemSafe(System system)
        {
            this.systemObjectsDictionary.Add(system.Id, system);
        }

        private void RemoveSystemSafe(uint id)
        {
            this.systemObjectsDictionary.Remove(id);
        }


        private void AddObject(GameObject gameObject)
        {
            gameObjectsDictionary.Add(gameObject.Id, gameObject);
            AddGameObject?.Invoke(gameObject);
        }

        private void RemoveObject(uint id)
        {
            RemoveGameObject?.Invoke(id);
            gameObjectsDictionary.Remove(id);
        }

    }
}