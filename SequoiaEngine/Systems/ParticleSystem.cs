using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;

namespace SequoiaEngine
{
    public class ParticleSystem : System
    {
        public ParticleSystem(SystemManager manager) : base(manager, typeof(Particle)) 
        {
        }


        protected override void Update(GameTime gameTime)
        {
            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                Particle particle = gameObject.GetComponent<Particle>();
                particle.Effect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}
