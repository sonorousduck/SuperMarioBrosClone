using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class Animated : Component
    {

        public Action OnStart;
        public Action OnEnd;
        public Action<float> OnUpdate;

        public bool IsStarted;
        public bool IsFinished;

        public float AnimationTime;
        public float MaxAnimationTime;

        public Dictionary<string, Component> ExtraData;



        public Animated(float animationTime, Action onStart = null, Action<float> onUpdate = null, Action onEnd = null, Dictionary<string, Component> extraData = null) 
        {
            this.AnimationTime = 0f;
            this.MaxAnimationTime = animationTime;
            this.OnStart = onStart;
            this.OnStart += OnBegin;
            this.ExtraData = extraData;


            if (onEnd == null)
            {
                this.OnEnd = OnFinish;
            }
            else
            {
                this.OnEnd = onEnd;
                this.OnEnd += OnFinish;
            }

            this.OnUpdate = onUpdate;
            this.OnUpdate += Update;
            this.IsStarted = false;
            this.IsFinished = false;
        }

        public void OnBegin()
        {
            this.IsStarted = true;
            this.IsFinished = false;
            this.AnimationTime = 0f;
        }

        public void OnFinish()
        {
            this.IsFinished = true;
            this.IsStarted = false;
            AnimationTime = 0f;
        }

        public void Restart()
        {
            this.IsFinished = false;
            this.IsStarted = false;

            this.OnStart?.Invoke();
        }


        public void Update(float deltaTime)
        {
            if (this.IsFinished) return;
            this.AnimationTime += deltaTime;

            if (this.AnimationTime >= MaxAnimationTime)
            {
                this.OnEnd?.Invoke();
            }


        }

    }
}
