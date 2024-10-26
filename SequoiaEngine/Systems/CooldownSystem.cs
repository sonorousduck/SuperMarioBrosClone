using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class CooldownSystem : System
    {
        public CooldownSystem(SystemManager systemManager) : base(systemManager, typeof(CooldownCollection)) { }

        protected override void Update(GameTime gameTime)
        {
            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                CooldownCollection cooldownCollection = gameObject.GetComponent<CooldownCollection>();

                foreach (Cooldown i in cooldownCollection.Cooldowns.Values)
                {
                    if (i.ShouldStart)
                    {
                        i.IsRunning = true;
                        i.ShouldStart = false;
                        i.CurrentTime = 0f;

                        i.OnCooldownStart?.Invoke();
                    }

                    if (i.IsRunning)
                    {
                        i.CurrentTime += GameManager.Instance.ElapsedSeconds;

                        i.OnCooldownUpdate?.Invoke(i.CooldownTime);
                    }

                    if (i.CurrentTime >= i.CooldownTime)
                    {
                        i.IsRunning = false;
                        i.OnCooldownEnd?.Invoke();
                    }
                }
            }
        }
    }
}
