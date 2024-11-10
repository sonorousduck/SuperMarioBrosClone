using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class Hit
    {
        public Collider Collider { get; set; }
        public Vector2 Position;
        public Vector2 Delta;
        public Vector2 Normal;
        public float Time;


        public Hit(Collider collider) 
        {
            this.Collider = collider;
            this.Position = new();
            this.Delta = new();
            this.Normal = new();
            this.Time = 0;
        }
    }
}
