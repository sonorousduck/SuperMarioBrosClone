using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SequoiaEngine;
using System;


namespace MarioClone
{
    public static class Player
    {



        public static GameObject Create(Vector2 position, Vector2 size)
        {
            int movementSpeed = 50;


            GameObject gameObject = new(new Transform(position, 0, size));

            gameObject.Add(new Rigidbody());
            gameObject.Add(new Sprite(ResourceManager.Get<Texture2D>("marioRight"), Color.White));


            Action<GameObject> onCollision = (GameObject other) =>
            {
                if (other != null)
                {
                    RectangleCollider collider = other.GetComponent<RectangleCollider>();
                    Transform colliderTransform = other.GetComponent<Transform>();

                    collider.IsColliding = true;

                    Transform transform = gameObject.GetComponent<Transform>();
                    transform.position = transform.previousPosition;

                }
            };

            Action<GameObject> onCollisionEnd = (GameObject other) =>
            {
                if (other != null)
                {
                    RectangleCollider collider = other.GetComponent<RectangleCollider>();
                    Transform colliderTransform = other.GetComponent<Transform>();
                    collider.IsColliding = false;
                }
            };


            gameObject.Add(new RectangleCollider(size * Vector2.One * gameObject.GetComponent<Sprite>().size, false, CollisionLayer.Player, CollisionLayer.Environment | CollisionLayer.Ground, onCollisionStart: onCollision, onCollision: onCollision, onCollisionEnd: onCollisionEnd));


            SequoiaEngine.KeyboardInput keyboardInput = new SequoiaEngine.KeyboardInput();


            keyboardInput.RegisterOnHeldAction("moveLeft", () =>
            {
                
                Vector2 movement = new Vector2(-movementSpeed, 0f) / GameManager.Instance.ElapsedMilliseconds;
                movement = movement.ToInt();
                
                gameObject.GetComponent<Rigidbody>().AddScriptedMovement(movement);

            });


            keyboardInput.RegisterOnHeldAction("moveRight", () =>
            {
                Vector2 movement = new Vector2(movementSpeed, 0f) / GameManager.Instance.ElapsedMilliseconds;
                movement = new Vector2((int)movement.X, (int)movement.Y);
                gameObject.GetComponent<Rigidbody>().AddScriptedMovement(movement);
            });




            keyboardInput.DefaultBindings.Add("moveLeft", Keys.A);
            keyboardInput.DefaultBindings.Add("moveRight", Keys.D);

            gameObject.Add(keyboardInput);


            MouseInput mouseInput = new MouseInput();




       

          

          

            gameObject.Add(mouseInput);
            


            return gameObject;

        }
    }
}
