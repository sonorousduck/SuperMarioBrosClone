using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    public class Rigidbody : Component
    {
        public float mass;
        public float gravityScale;
        public Vector2 velocity;
        public Vector2 acceleration;
        public bool usesGravity;

        /// <summary>
        /// List of ids which this rigidbody is colliding with
        /// </summary>
        public List<uint> currentlyCollidingWith;

        public bool hasMoved = false;

        List<Vector2> scriptedMovements = new();

        // Convenience calls. Calls to the last function in this file
        public Rigidbody(float mass = 0, float gravityScale = 0) : this(new Vector2(0, 0), new Vector2(0, 0), mass, gravityScale, usesGravity: gravityScale > 0f)
        {
        }

        // Convenience calls. Calls to the last function in this file
        public Rigidbody(Vector2 startVelocity, float mass = 0, float gravityScale = 0) : this(new Vector2(0, 0), startVelocity, mass, gravityScale)
        {
        }

        public Rigidbody(Vector2 acceleration, Vector2 startVelocity, float mass = 0, float gravityScale = 0, bool usesGravity = false)
        {
            this.mass = mass;
            this.gravityScale = gravityScale;
            this.velocity = startVelocity;
            this.currentlyCollidingWith = new List<uint>();
            this.acceleration = acceleration;
            this.usesGravity = usesGravity;
        }

        public void AddScriptedMovement(Vector2 scriptedMovement)
        {
            scriptedMovements.Add(scriptedMovement);
        }

        public Vector2 GetNextScriptedMovement()
        {
            Vector2 movement = scriptedMovements[scriptedMovements.Count - 1];
            scriptedMovements.RemoveAt(scriptedMovements.Count - 1);
            
            return movement;
        }

        public int ScriptedMovementLength()
        {
            return scriptedMovements.Count; 
        }

        public void SetMoved()
        {
            this.hasMoved = true;
        }

        public bool HasMoved()
        {
            return hasMoved; 
        }

    }
}