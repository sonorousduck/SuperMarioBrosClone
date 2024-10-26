using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class AnimatedSystem : System
    {
        public AnimatedSystem(SystemManager systemManager) : base(systemManager, typeof(Animated))
        { }

        protected override void Update(GameTime gameTime)
        {
            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                if (!gameObject.IsEnabled()) return;

                Animated animated = gameObject.GetComponent<Animated>();

                if (animated.IsStarted && !animated.IsFinished)
                {
                    animated.OnUpdate?.Invoke(GameManager.Instance.ElapsedSeconds);
                }
            }
        }
    }
}
