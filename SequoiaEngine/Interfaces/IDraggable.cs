
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    
    public interface IDraggable
    {
        Rectangle Rectangle { get; }
        Transform Transform { get; set;  }

    }
}
