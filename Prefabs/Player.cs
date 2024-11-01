﻿using Microsoft.Xna.Framework;
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

            gameObject.Add(new Rigidbody(gravityScale: 4));
            gameObject.Add(new Sprite(ResourceManager.Get<Texture2D>("marioRight"), Color.White));


            Action<GameObject> onCollision = (GameObject other) =>
            {
                if (other != null)
                {
                    RectangleCollider collider = other.GetComponent<RectangleCollider>();
                    Transform colliderTransform = other.GetComponent<Transform>();

                    collider.IsColliding = true;

                    Transform transform = gameObject.GetComponent<Transform>();
                    transform.position.Y = transform.previousPosition.Y;
                    if (collider.Layer == CollisionLayer.Ground)
                    {
                        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                        Transform playerTransform = gameObject.GetComponent<Transform>();
                        Sprite playerSprite = gameObject.GetComponent<Sprite>();


                        // First, check to make sure the player isn't the the left or right of the platform. If this is the case
                        // they should just not be able to go left/right


                        // Check if player's y was less than the ground, this means they landed on it
                        if (playerTransform.position.Y - playerSprite.size.Y / 2 < colliderTransform.position.Y)
                        {
                            rb.velocity.Y = 0;
                            rb.acceleration = Vector2.Zero;
                            playerTransform.position.Y = colliderTransform.position.Y - (collider.size.Y / 2) - playerSprite.size.Y / 2;
                            gameObject.GetComponent<Gravity>().LandedOnGround();

                        }
                        // If player's y was greater than the ground, they bonked their head
                        else if (playerTransform.position.Y - playerSprite.size.Y / 2 > colliderTransform.position.Y)
                        {
                            playerTransform.position.Y = colliderTransform.position.Y + (collider.size.Y / 2) + playerSprite.size.Y / 2;

                            rb.velocity = new Vector2(rb.velocity.X, -rb.velocity.Y);
                            rb.acceleration = new Vector2(rb.acceleration.X, -rb.velocity.X);
                        }



                        // TODO: Handle if they player walked into the side of the wall
                    }
                }
            };

            Action<GameObject> onCollisionEnd = (GameObject other) =>
            {
                if (other != null)
                {
                    RectangleCollider collider = other.GetComponent<RectangleCollider>();
                    Transform colliderTransform = other.GetComponent<Transform>();
                    collider.IsColliding = false;
                    if (collider.Layer == CollisionLayer.Ground)
                    {
                        gameObject.GetComponent<Gravity>().Jumped();
                    }
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

            keyboardInput.RegisterOnPressAction("jump", () =>
            {
                Gravity gravity = gameObject.GetComponent<Gravity>();

                if (gravity.OnGround)
                {
                    gameObject.GetComponent<Rigidbody>().velocity.Y = gravity.JumpVelocity;
                    gravity.Jumped();
                }
            });




            keyboardInput.DefaultBindings.Add("moveLeft", Keys.A);
            keyboardInput.DefaultBindings.Add("moveRight", Keys.D);
            keyboardInput.DefaultBindings.Add("jump", Keys.Space);

            gameObject.Add(keyboardInput);
            gameObject.Add(new Gravity(114.0f, 32.0f));

            MouseInput mouseInput = new MouseInput();




       

          

          

            gameObject.Add(mouseInput);
            


            return gameObject;

        }
    }
}
