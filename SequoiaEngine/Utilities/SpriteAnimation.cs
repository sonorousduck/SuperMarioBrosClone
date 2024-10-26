using MarioClone;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
/*    public class SpriteAnimation
    {
        public List<AnimationFrame> Frames { get; private set; } = new();

        /// <summary>
        /// Controls whether this animation should only play a single time
        /// </summary>
        public bool PlayOnce = true;

        /// <summary>
        /// Loops forever unless something is triggered to leave this animation
        /// </summary>
        public bool PlayForever = false;

        /// <summary>
        /// If Play Once or Play forever is true, this is ignored
        /// </summary>
        public int NumberTimesToRepeat = 1;


        /// <summary>
        /// Specifies if the animation has completed
        /// </summary>
        public bool HasCompletedPlay = false;

        // Maps from frame index to a function
        // Gameobject is passed in to be able to access itself
        public Dictionary<int, Action<GameObject>> Callbacks;

        /// <summary>
        /// Creates a queue of the callbacks that should run during the update phase
        /// </summary>
        public Queue<Action<GameObject>> CallbacksToRun { get; private set; }

        /// <summary>
        /// Specifies the render depth during drawing
        /// </summary>
        public float RenderDepth;

        public int CurrentFrame;
        public float CurrentElapsedTime;


        public SpriteAnimation(Dictionary<int, Action<GameObject>> callbacks = null, bool playOnce = false, float renderDepth = 0f)
        {
            this.Callbacks = callbacks ?? new();
            this.PlayOnce = playOnce;
            this.RenderDepth = renderDepth;
            CurrentFrame = 0;
            CurrentElapsedTime = 0;
        }

        public void ResetAnimation()
        {
            CurrentFrame = 0;
            CurrentElapsedTime = 0;
        }

        public void UpdateElapsedTime(GameTime gameTime, GameObject go)
        {
            CurrentElapsedTime += GameManager.Instance.ElapsedSeconds;

            while (CurrentElapsedTime >= Frames[CurrentFrame].Duration)
            {
                CurrentElapsedTime -= Frames[CurrentFrame].Duration;
                IncrementSprite(go);
            }

        }

        private void IncrementSprite(GameObject go)
        {
            // Make sure the callbacks are called for every frame, even it happens to skip over it
            if (Callbacks.TryGetValue(CurrentFrame, out Action<GameObject> callback))
            {
                callback?.Invoke(go);
            }

            if (CurrentFrame + 1 < Frames.Count)
            {
                CurrentFrame++;
            }
            else if ((!PlayOnce && NumberTimesToRepeat > 0) || PlayForever)
            {
                CurrentFrame = 0;
                NumberTimesToRepeat--;
            }
        }

        public void AddFrame(int frameIndex, float duration, string name = "")
        {
            Frames.Add(new AnimationFrame(frameIndex, duration, name));
        }*/
    //}
}
