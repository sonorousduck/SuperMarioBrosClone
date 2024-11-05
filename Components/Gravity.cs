using SequoiaEngine;
using System;

namespace MarioClone
{
    public class Gravity : Component
    {
        public bool OnGround = false;

        public float JumpHeight;
        public float HorizontalJumpHeight;

        public float GravitySpeed;
        public float JumpVelocity;
        public float RemainingVelocity;
        public float PercentageToApplyPerFrame;
        public float PercentageOnceInAir;
        

        public Gravity(float jumpHeight, float horizontalJumpHeight, float percentageToApplyPerFrame = 1.0f, float percentageOnceInAir = 0.1f) 
        {
            this.JumpHeight = jumpHeight;
            this.HorizontalJumpHeight = horizontalJumpHeight;
            this.PercentageToApplyPerFrame = percentageToApplyPerFrame;
            this.PercentageOnceInAir = percentageOnceInAir;
            // https://youtu.be/hG9SzQxaCm8?si=Yu4LjtI0WLkoip7K&t=545
            this.GravitySpeed = (2 * this.JumpHeight * this.HorizontalJumpHeight) / (this.HorizontalJumpHeight * this.HorizontalJumpHeight);
            this.JumpVelocity = (-2 * this.JumpHeight * this.HorizontalJumpHeight) / (this.HorizontalJumpHeight);
            this.RemainingVelocity = this.JumpVelocity;
        }

        public void LandedOnGround()
        {
            this.OnGround = true;
            this.RemainingVelocity = this.JumpVelocity;
        }

        public void Jumped()
        {
            this.OnGround = false;
        }
    }
}
