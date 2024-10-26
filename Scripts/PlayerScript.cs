using SequoiaEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MarioClone
{
    public class PlayerScript : Script
    {
        public PlayerScript(GameObject gameObject) : base(gameObject) { }



        public override void OnCollisionStart(GameObject other)
        {
            base.OnCollisionStart(other);

            
        }

    }
}
