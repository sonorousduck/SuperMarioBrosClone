using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;


namespace SequoiaEngine
{
    public class ParticleRenderingSystem : System
    {

        public ParticleRenderingSystem(SystemManager systemManager) : base(systemManager, typeof(Particle))
        {
            systemManager.UpdateSystem -= Update; // remove the automatically added update
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool isDrawingHud = false)
        {
            foreach ((uint id, GameObject gameObject) in gameObjects)
            {
                Particle particle = gameObject.GetComponent<Particle>();
                Transform particleTransform = gameObject.GetComponent<Transform>();

                if (isDrawingHud != particleTransform.IsHUD) return;

                spriteBatch.Draw(particle.Effect);
            }
        }

        protected override void Update(GameTime gameTime)
        {
        }
    }
}
