using SequoiaEngine;
using System;

namespace MarioClone
{
    public class Gravity : Component
    {
        public bool IsOnGround = false;

        public float JumpHeight;
        public float HorizontalJumpHeight;

        public float GravitySpeed;
        public float JumpVelocity;
        

        public Gravity(float jumpHeight, float horizontalJumpHeight) 
        {
            this.JumpHeight = jumpHeight;
            this.HorizontalJumpHeight = horizontalJumpHeight;

            // https://youtu.be/hG9SzQxaCm8?si=Yu4LjtI0WLkoip7K&t=545
            this.GravitySpeed = (2 * this.JumpHeight * this.HorizontalJumpHeight) / (this.HorizontalJumpHeight * this.HorizontalJumpHeight);
            this.JumpVelocity = (-2 * this.JumpHeight * this.HorizontalJumpHeight) / (this.HorizontalJumpHeight);
        }

        public void LandedOnGround()
        {
            this.IsOnGround = true;
        }
    }
}
