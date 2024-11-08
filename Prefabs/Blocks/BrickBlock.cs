using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using SequoiaEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioClone.Prefabs.Blocks
{ 
    public static class BrickBlock
    {
        public static GameObject Create(Vector2 position, Vector2 size, TiledMapProperties props)
        {
            GameObject gameObject = new(new Transform(position + new Vector2(size.X, -size.Y) / 2, 0, Vector2.One));
            RectangleCollider rectangleCollider = new(size, true, CollisionLayer.Ground, CollisionLayer.Player);





            gameObject.Add(new RenderedComponent());
            gameObject.Add(new Rigidbody());
            gameObject.Add(rectangleCollider);
            return gameObject;
        }


    }
}
