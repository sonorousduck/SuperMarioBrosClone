using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public enum ScaleSize
    {
        None,
        Entire_Width,
        Entire_Height,
        Entire_Parent
    }

    public class Scale : Component
    {
        public Vector2 ScaleOfParent = Vector2.One;
        public ScaleSize VariableScaleSize = ScaleSize.None;

        public Scale(ScaleSize scaleSize)
        {
            this.VariableScaleSize = scaleSize;
        }

        public Scale(Vector2 scaleSize)
        {
            this.ScaleOfParent = scaleSize;
        }

        public Vector2 GetScaleModifier(GameObject go)
        {
            GameObject parent = go.GetParent();

            // If there is no parent, you can't size to anything
            // Thus, just return a modification of zero (since we add the scale).
            if (parent == null) return Vector2.One;

            Transform parentTransform = parent.GetComponent<Transform>();
            Sprite parentSprite = parent.GetComponent<Sprite>();
            Vector2 parentScale = parentTransform.scale * parentSprite?.size ?? Vector2.One;

            if (VariableScaleSize != ScaleSize.None)
            {

                switch (VariableScaleSize)
                {
                    case ScaleSize.Entire_Width:
                        return new Vector2(parentScale.X, 1f) / go.GetComponent<Sprite>()?.size ?? Vector2.One;
                    case ScaleSize.Entire_Height:
                        return new Vector2(1f, parentScale.Y) / go.GetComponent<Sprite>()?.size ?? Vector2.One;
                    case ScaleSize.Entire_Parent:
                        return new Vector2(parentScale.X, parentScale.Y) / go.GetComponent<Sprite>()?.size ?? Vector2.One;
                    default:
                        return Vector2.One;
                }
            }
            else
            {   
                return this.ScaleOfParent * parentScale;
            }

        }
    }
}
