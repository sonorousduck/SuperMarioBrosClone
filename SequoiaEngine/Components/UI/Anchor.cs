using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace SequoiaEngine
{

    public enum AnchorLocation
    {
        None,
        TopLeft,
        TopMiddle,
        TopRight,
        MiddleLeft,
        MiddleMiddle,
        MiddleRight,
        BottomLeft,
        BottomMiddle,
        BottomRight
    }

    public class Anchor : Component
    {
        public AnchorLocation Location;
         
        public Anchor(AnchorLocation location)
        {
            Location = location;
        }


        public Vector2 GetAnchorPoint(GameObject go)
        {
            GameObject parent = go.GetParent();

            // If there is no parent, you can't anchor to anything
            // Thus, just return a modification of zero.
            if (parent == null)
            {
                return Vector2.Zero;
            }

            Transform parentTransform = parent.GetComponent<Transform>();
            Sprite parentSprite = parent.GetComponent<Sprite>();
            Vector2 parentScale = parentTransform.scale;

            if (parentSprite?.size != null)
            {
                parentScale *= parentSprite.size;
            }

            Vector2 parentPosition = parentTransform.position - parentScale / 2;

            switch (this.Location)
            {
                case AnchorLocation.TopLeft:
                    return parentPosition;
                
                case AnchorLocation.TopMiddle:
                    return parentPosition + new Vector2(parentScale.X / 2, 0f);

                case AnchorLocation.TopRight:
                    return parentPosition + new Vector2(parentScale.X, 0f);

                case AnchorLocation.MiddleLeft:
                    return parentPosition + new Vector2(0f, parentScale.Y / 2f);

                case AnchorLocation.MiddleMiddle:
                    return parentPosition + parentScale / 2f;
                
                case AnchorLocation.MiddleRight:
                    return parentPosition + new Vector2(parentScale.X, parentScale.Y / 2);

                case AnchorLocation.BottomLeft:
                    return parentPosition + new Vector2(0f, parentScale.Y);

                case AnchorLocation.BottomMiddle:
                    return parentPosition + new Vector2(parentScale.X / 2, parentScale.Y);
                
                case AnchorLocation.BottomRight:
                    return parentPosition + new Vector2(parentScale.X, parentScale.Y);
                
                default:
                    break;
            }

            return Vector2.Zero;
        }
    }
}
