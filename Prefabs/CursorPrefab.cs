using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SequoiaEngine;
using System.Diagnostics;

namespace MarioClone
{
    public static class CursorPrefab
    {

        public static GameObject Create(Vector2 position, Vector2 size)
        {
            GameObject gameObject = new(new Transform(position, 0, Vector2.One));

            gameObject.Add(new Rigidbody());
            gameObject.Add(new RectangleCollider(size * new Vector2(2, 2), false, CollisionLayer.UI, CollisionLayer.UI));

            gameObject.Add(new AudioSource());
            //gameObject.Add(new Sprite(ResourceManager.Get<Texture2D>("crosshair"), Color.White));


            MouseInput mouseInput = new MouseInput();

            /*mouseInput.RegisterOnPressAction("click", () =>
            {
            });


            mouseInput.DefaultBindings.Add("click", MouseButton.LeftButton);*/



            gameObject.Add(mouseInput);

            return gameObject;

        }
    }
}