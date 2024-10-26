using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SequoiaEngine;


namespace SequoiaEngine
{
    public class AnimationFrame
    {
        public int FrameIndex { get; private set; }
        public string Name { get; private set; }
        public float Duration { get; private set; }


        public AnimationFrame(int frameIndex, float duration, string name)
        {
            this.FrameIndex = frameIndex;
            this.Duration = duration;
            this.Name = name;
        }
    }

    public class AnimatedSprite
    {
        // The texture from which to take the frame data

        public TextureAtlas TextureAtlas { get; private set; }
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

        public Vector2 SpriteSize;

        public AnimatedSprite(TextureAtlas atlas, Dictionary<int, Action<GameObject>> callbacks = null, bool playForever = true, bool playOnce = false, float renderDepth = 0f)
        {
            this.TextureAtlas = atlas;
            this.Callbacks = callbacks ?? new();
            this.PlayOnce = playOnce;
            this.RenderDepth = renderDepth;
            CurrentFrame = 0;
            CurrentElapsedTime = 0;
            SpriteSize = atlas.SpriteSize;
            this.PlayForever = playForever;
        }

        public AnimatedSprite(TextureAtlas atlas, List<float> timings, Dictionary<int, Action<GameObject>> callbacks = null, bool playForever = true, bool playOnce = false, float renderDepth = 0f)
        {
            this.TextureAtlas = atlas;
            this.Callbacks = callbacks ?? new();
            this.PlayOnce = playOnce;
            this.RenderDepth = renderDepth;
            CurrentFrame = 0;
            CurrentElapsedTime = 0;
            SpriteSize = atlas.SpriteSize;
            this.PlayForever = playForever;
            this.AutocalculateFrames(timings);
        }

        public AnimatedSprite(TextureAtlas atlas, float timing, Dictionary<int, Action<GameObject>> callbacks = null, bool playForever = true, bool playOnce = false, float renderDepth = 0f)
        {
            this.TextureAtlas = atlas;
            this.Callbacks = callbacks ?? new();
            this.PlayOnce = playOnce;
            this.RenderDepth = renderDepth;
            CurrentFrame = 0;
            CurrentElapsedTime = 0;
            SpriteSize = atlas.SpriteSize;
            this.PlayForever = playForever;
            this.AutocalculateFrames(timing);
        }

        public void ResetAnimation()
        {
            CurrentFrame = 0;
            CurrentElapsedTime = 0;
            HasCompletedPlay = false;
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
            else
            {
                HasCompletedPlay = true;
            }
        }

        public void AddFrame(int frameIndex, float duration, string name = "")
        {
            Frames.Add(new AnimationFrame(frameIndex, duration, name));
        }

        public void AutocalculateFrames(float duration)
        {
            Vector2 spriteSize = TextureAtlas.SpriteSize;

            int numCols = (int)(TextureAtlas.Texture.Width / spriteSize.X);
            int numRows = (int)(TextureAtlas.Texture.Height / spriteSize.Y);

            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    Frames.Add(new AnimationFrame(r * numCols + c, duration, ""));
                }
            }
        }

        public void AutocalculateFrames(List<float> durations)
        {
            Vector2 spriteSize = TextureAtlas.SpriteSize;

            int numCols = (int)(TextureAtlas.Texture.Width / spriteSize.X);
            int numRows = (int)(TextureAtlas.Texture.Height / spriteSize.Y);

            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    Frames.Add(new AnimationFrame(r * numCols + c, durations[r * numCols + c], ""));
                }
            }
        }

        public AnimationFrame GetCurrentFrame()
        {
            return Frames[CurrentFrame];
        }       
    }
}