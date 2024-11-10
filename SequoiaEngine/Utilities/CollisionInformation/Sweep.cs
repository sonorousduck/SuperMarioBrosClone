using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class Sweep
    {
        public List<Hit> HitInformation;
        public Vector2 Position;
        public float Time;

        public Sweep() 
        {
            this.HitInformation = new();
            this.Position = new Vector2();
            this.Time = 1;
        }

    }
}
