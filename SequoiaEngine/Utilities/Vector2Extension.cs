using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public static class Vector2Extension
    {
        public static Vector2 ToInt(this Vector2 vector2)
        {
            return (new Vector2((int)vector2.X, (int)vector2.Y));
        }

    }
}
