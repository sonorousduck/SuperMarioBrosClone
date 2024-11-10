using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.Utilities;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using SequoiaEngine;
using System;
using System.Diagnostics;


namespace MarioClone
{
    public static class Player
    {



        public static GameObject Create(Vector2 position, Vector2 size)
        {
            int movementSpeed = 100;


            GameObject gameObject = new(new Transform(position, 0, size));

            gameObject.Add(new Rigidbody(gravityScale: 7));
            gameObject.Add(new Sprite(ResourceManager.Get<Texture2D>("marioRight"), Color.White));


            OnCollisionEventHandler onCollision = (Fixture fixture, Fixture other, Contact contact) =>
            {
                return true;
                //if (other != null)
                //{
                //    RectangleCollider collider = other.GetComponent<RectangleCollider>();
                //    Transform colliderTransform = other.GetComponent<Transform>();

                //    collider.IsColliding = true;

                //    Transform transform = gameObject.GetComponent<Transform>();
                //    Rigidbody rb = gameObject.GetComponent<Rigidbody>();


                    //Vector2 overlap = collider.CalculateOverlap(transform, colliderTransform, collider);
                    //if (Math.Abs(overlap.Y) < Math.Abs(overlap.X))
                    //{
                    //    // Vertical collision (e.g., landing on ground or hitting a ceiling)
                    //    /*transform.position.Y = transform.previousPosition.Y;
                    //    rb.velocity.Y = 0; // Stop vertical movement

                    //    if (overlap.Y > 0) // Landing on the ground
                    //    {
                    //        // Trigger landing events, etc., if needed
                    //    }*/
                    //}
                    //else
                    //{
                    //    // Horizontal collision (e.g., hitting a wall)
                    //    transform.position.X = transform.previousPosition.X;
                    //}


                    //if (collider.Layer == CollisionLayer.Ground)
                    //{
                    //    Transform playerTransform = gameObject.GetComponent<Transform>();
                    //    Sprite playerSprite = gameObject.GetComponent<Sprite>();

                    //    // First, check to make sure the player isn't the the left or right of the platform. If this is the case
                    //    // they should just not be able to go left/right


                    //    // Check if player's y was less than the ground, this means they landed on it
                    //    if (playerTransform.position.Y - playerSprite.size.Y / 2 < colliderTransform.position.Y)
                    //    {
                    //        rb.velocity.Y = 0;
                    //        rb.acceleration = Vector2.Zero;
                    //        playerTransform.position.Y = colliderTransform.position.Y - (collider.size.Y / 2) - playerSprite.size.Y / 2;
                    //        gameObject.GetComponent<Gravity>().LandedOnGround();


                    //    }
                    //    // If player's y was greater than the ground, they bonked their head
                    //    else if (playerTransform.position.Y - playerSprite.size.Y / 2 > colliderTransform.position.Y)
                    //    {
                    //        playerTransform.position.Y = colliderTransform.position.Y + (collider.size.Y / 2) + playerSprite.size.Y / 2;

                    //        rb.velocity = new Vector2(rb.velocity.X, -rb.velocity.Y);
                    //        rb.acceleration = new Vector2(rb.acceleration.X, -rb.velocity.X);
                    //    }


                    //    // TODO: Handle if they player walked into the side of the wall
                    //}
                //}
            };

            OnSeparationEventHandler onCollisionEnd = (Fixture sender, Fixture other, Contact contact) =>
            {
                //if (other != null)
                //{
                //    RectangleCollider collider = other.GetComponent<RectangleCollider>();
                //    Transform colliderTransform = other.GetComponent<Transform>();
                //    collider.IsColliding = false;
                //    if (collider.Layer == CollisionLayer.Ground)
                //    {
                //        gameObject.GetComponent<Gravity>().Jumped();
                //    }
                //}
            };


            gameObject.Add(new RectangleCollider(
                size * Vector2.One * gameObject.GetComponent<Sprite>().size, 
                false, 
                CollisionLayer.Player, 
                CollisionLayer.Environment | CollisionLayer.Ground, 
                onCollision: onCollision, 
                onCollisionEnd: onCollisionEnd, 
                bodyType: BodyType.Dynamic,
                density: 10f));


            SequoiaEngine.KeyboardInput keyboardInput = new SequoiaEngine.KeyboardInput();


            keyboardInput.RegisterOnHeldAction("moveLeft", () =>
            {
                RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();
                Vector2 movement = new Vector2(-movementSpeed, rectangleCollider.Body.LinearVelocity.Y);
                //movement = movement.ToInt();     

                rectangleCollider.Body.LinearVelocity = movement;
            });

            keyboardInput.RegisterOnReleaseAction("moveLeft", () =>
            {
                RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();
                Vector2 movement = new Vector2(0f, rectangleCollider.Body.LinearVelocity.Y);
                rectangleCollider.Body.LinearVelocity = movement;
            });


            keyboardInput.RegisterOnHeldAction("moveRight", () =>
            {
                RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();
                Vector2 currentLinearVelocity = rectangleCollider.Body.LinearVelocity;
                Vector2 movement = new Vector2(movementSpeed, currentLinearVelocity.Y);

                rectangleCollider.Body.LinearVelocity = movement;
            });

            keyboardInput.RegisterOnReleaseAction("moveRight", () =>
            {
                RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();
                Vector2 movement = new Vector2(0f, rectangleCollider.Body.LinearVelocity.Y);
                rectangleCollider.Body.LinearVelocity = movement;
            });

            keyboardInput.RegisterOnPressAction("jump", () =>
            {
                RectangleCollider rectangleCollider = gameObject.GetComponent<RectangleCollider>();
                Gravity gravity = gameObject.GetComponent<Gravity>();

                if (rectangleCollider.IsColliding)
                {
                    rectangleCollider.Body.ApplyLinearImpulse(new Vector2(0f, gravity.JumpVelocity * 100f));
                }
                if (gravity.OnGround)
                {
                    gameObject.GetComponent<Rigidbody>().velocity.Y = gravity.JumpVelocity * gravity.PercentageToApplyPerFrame;
                    gravity.RemainingVelocity -= gravity.JumpVelocity * gravity.PercentageToApplyPerFrame;
                    gravity.Jumped();
                }
            });

            keyboardInput.RegisterOnHeldAction("jump", () =>
            {
                Gravity gravity = gameObject.GetComponent<Gravity>();

                if (!gravity.OnGround && gravity.RemainingVelocity < 0f)
                {
                    gameObject.GetComponent<Rigidbody>().velocity.Y += gravity.JumpVelocity * gravity.PercentageToApplyPerFrame * gravity.PercentageOnceInAir;
                    gravity.RemainingVelocity -= gravity.JumpVelocity * gravity.PercentageToApplyPerFrame;
                }
            });




            keyboardInput.DefaultBindings.Add("moveLeft", Keys.A);
            keyboardInput.DefaultBindings.Add("moveRight", Keys.D);
            keyboardInput.DefaultBindings.Add("jump", Keys.Space);

            gameObject.Add(keyboardInput);
            gameObject.Add(new Gravity(3300.0f, 320.0f, 0.15f, 0.75f));

            MouseInput mouseInput = new MouseInput();




       

          

          

            gameObject.Add(mouseInput);
            


            return gameObject;

        }
    }
}
