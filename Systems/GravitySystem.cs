using Microsoft.Xna.Framework;
using SequoiaEngine;


namespace MarioClone
{
    public class GravitySystem : SequoiaEngine.System
    {
        public GravitySystem(SystemManager systemManager) : base(systemManager, typeof(Gravity), typeof(Rigidbody), typeof(RectangleCollider)) 
        {}

        protected override void Update(GameTime gameTime)
        {

/*            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                gameObject.TryGetComponent(out Gravity gravityComponent);
                gameObject.TryGetComponent(out RectangleCollider rectangleCollider);
                gameObject.TryGetComponent(out Rigidbody rigidbodyComponent);



            }*/
        }
    }
}
