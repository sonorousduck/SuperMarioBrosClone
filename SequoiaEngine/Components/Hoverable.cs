using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class Hoverable : Component
    {
        public Hoverable(Action<GameObject> onHoverStart, Action<GameObject> onHoverEnd) 
        {
            this.OnHoverStart = onHoverStart; 
            this.OnHoverEnd = onHoverEnd;
        }

        public Action<GameObject> OnHoverStart;
        public Action<GameObject> OnHoverEnd;
    }
}
