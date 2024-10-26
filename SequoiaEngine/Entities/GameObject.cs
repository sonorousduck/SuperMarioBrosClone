using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SequoiaEngine
{
    /// <summary>
    /// Heavily referenced from Dr. Dean Mathias's ECS example program
    /// </summary>
    public sealed class GameObject
    {
        private readonly Dictionary<Type, Component> components = new();

        private static uint nextId = 0;

        public string Tag { get; private set; } = "";

        public uint Id { get; private set; }

        private bool enabled = true;

        private List<GameObject> children;
        private GameObject parent;

        public GameObject(string tag = "")
        {
            Id = nextId++;
            children = new List<GameObject>();
            this.Tag = tag;

            Transform defaultTransform = new();
            this.Add(defaultTransform);
        }

        public GameObject(GameObject parent, string tag = "") : this()
        {
            this.parent = parent;
            parent.children.Add(this);
            this.Tag = tag;
        }

        public GameObject(Transform transform, GameObject parent = null, string tag = "")
        {
            Id = nextId++;
            children = new List<GameObject>();

            this.Add(transform);
            this.parent = parent;
            if (parent != null)
            {
                parent.RegisterChildRefless(this);
            }
            this.Tag = tag;
        }

        // Return the number of children present on the game object
        public int GetChildrenCount()
        { 
            return children.Count; 
        }


        public void Disable()
        {
            this.enabled = false;
        }

        public void Enable()
        {
            this.enabled = true; 
        }

        public bool IsEnabled()
        {
            return this.enabled;
        }

        /// <summary>
        /// Used to get all registered children of the Game Object. This is (most likely) a precursor to just using a Scene Graph
        /// </summary>
        public List<GameObject> GetChildren() 
        { 
            return children; 
        }

        /// <summary>
        /// Used to get (if exists) the parent GameObject
        /// </summary>
        /// <returns></returns>
        public GameObject GetParent()
        {
            if (parent == null)
            {
                return null;
            }

            return parent;
        }


        /// <summary>
        /// Register a new child game object
        /// </summary>
        /// <param name="child">Reference to the Child Game Object</param>
        public void RegisterChild(ref GameObject child)
        {
            child.GetComponent<Transform>().Offset = child.GetComponent<Transform>().position;

            children.Add(child);
        }

        /// <summary>
        /// Register a new child game object
        /// </summary>
        /// <param name="child">Child Game Object. Will not exist outside of this call</param>
        public void RegisterChildRefless(GameObject child)
        {
            child.GetComponent<Transform>().Offset = child.GetComponent<Transform>().position;


            children.Add(child);
        }

        /// <summary>
        /// Remove tracking of the Child Game Object
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(ref GameObject child)
        {
            children.Remove(child);
        }


        /// <summary>
        /// Used to get reference to a component on a game object, which will be useful for scripting
        /// </summary>
        /// <typeparam name="TComponent">The type of component to get</typeparam>
        /// <returns>The component if found, or null otherwise</returns>
        public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : Component
        {
            bool value = (components.TryGetValue(typeof(TComponent), out Component tempComponent));
            
            if (value)
            {
                component = tempComponent as TComponent;
                return true;
            }

            component = GetInheritedComponent<TComponent>();
            return false;
            
        }


        /// <summary>
        /// Used to get reference to a component on a game object, which will be useful for scripting
        /// </summary>
        /// <typeparam name="TComponent">The type of component to get</typeparam>
        /// <returns>The component if found, or null otherwise</returns>
        public TComponent GetComponent<TComponent>()
            where TComponent : Component
        {
            Component component;
            components.TryGetValue(typeof(TComponent), out component);

            if (component == null)
            {
                component = GetInheritedComponent<TComponent>();
            }

            return (TComponent)component;
        }



        /// <summary>
        /// Gets the first component which fulfills this condition, which is useful for scripting
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        private TComponent GetInheritedComponent<TComponent>()
            where TComponent : Component
        {
            // 1. Find the matching type
            Type type = null;
            foreach (Type componentType in components.Keys)
            {
                if (typeof(TComponent).IsAssignableFrom(componentType))
                {
                    type = componentType;
                    break;
                }
            }
            if (type == null)
            {
                return null;
            }
            else
            {
                Component component;
                components.TryGetValue(type, out component);
                return (TComponent)component;
            }
        }

        /// <summary>
        /// Checks if a game object contains a component of the given type. Used by systems to subscribe when they need to
        /// </summary>
        /// <typeparam name="TComponent">The type of component to check for</typeparam>
        /// <returns></returns>
        public bool ContainsComponent<TComponent>()
            where TComponent : Component
        {
            return ContainsComponent(typeof(TComponent));
        }

        public bool ContainsComponent(Type type)
        {
            return components.ContainsKey(type) && components[type] != null;
        }


        public bool ContainsComponentOfParentType<TComponent>()
            where TComponent : Component
        {
            return ContainsComponentOfParentType(typeof(TComponent));
        }

        /// <summary>
        /// This is slower, but checks if the gameobject has a component which could satisfy the condition.
        /// This is useful for things like colliders having a 'parent' component
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ContainsComponentOfParentType(Type type)
        {
            foreach (Type component in components.Keys)
            {
                if (type.IsAssignableFrom(component))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a list of components to a game object
        /// </summary>
        /// <param name="newComponents">The list of components to add</param>
        public void Add(params Component[] newComponents)
        {
            Debug.Assert(components != null, "You cannot add an empty list of components");

            foreach (Component component in newComponents)
            {
                Type type = component.GetType();

                Debug.Assert(typeof(Component).IsAssignableFrom(type), $"The given type should be assignable to {typeof(Component)}");
                Debug.Assert(!this.components.ContainsKey(type), $"A component of type {type} is already attached to this game object");

                component.Parent = this;
                this.components.Add(type, component);
            }
        }

        /// <summary>
        /// Add a single component
        /// </summary>
        /// <param name="component">The component to add</param>
        public void Add(Component component)
        {
            Debug.Assert(component != null, "Cannot add a null component");
            Debug.Assert(!this.components.ContainsKey(component.GetType()), $"A component of type {component.GetType()} has already been attached to this game object");

            component.Parent = this;
            components.Add(component.GetType(), component);
        }

        /// <summary>
        /// Remove all components
        /// </summary>
        public void Clear()
        {
            components.Clear();
        }

        /// <summary>
        /// Remove a list of components from a game object
        /// </summary>
        /// <param name="removedComponents">The list of components to remove</param>
        public void Remove(params Component[] removedComponents)
        {
            foreach (Component component in removedComponents)
            {
                components.Remove(component.GetType());
            }
        }

        public void Remove<TComponent>()
            where TComponent : Component
        {
            this.components.Remove(typeof(TComponent));
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Id, string.Join(", ", from c in components.Values select c.GetType().Name));
        }
    }
}