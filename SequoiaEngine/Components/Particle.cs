using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;


namespace SequoiaEngine
{
    public class Particle : Component
    {
        public ParticleEffect Effect;
        public Texture2D Texture;
        public bool IsHud = false;
        public Particle(Texture2D texture, ParticleEffect effect)
        {
            this.Texture = texture;
            this.Effect = effect;
        }

    }
}
