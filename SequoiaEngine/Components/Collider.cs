﻿using System;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;


namespace SequoiaEngine
{

    [Flags]
    public enum CollisionLayer
    {
        None = 0,
        Environment = 1,
        Ground = 1 << 1,
        Player = 1 << 2,
        UI = 1 << 3,
        Enemy = 1 << 4,
        Interactables = 1 << 5,
        EnemyBolt = 1 << 6,
        All = 1 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5 | 1 << 6 | 1 << 7,
    }


    /// <summary>
    /// Abstract class that colliders should extend in order to be recognized by the physics system
    /// </summary>
    public abstract class Collider : Component
    {
        public bool isStatic;
        public Vector2 offset;

        public BodyType bodyType;
        public float density;

        public CollisionLayer Layer;
        public CollisionLayer LayersToCollideWith;

        public Body Body;


        public abstract void CreateBody(World world, Transform position);
    }
}