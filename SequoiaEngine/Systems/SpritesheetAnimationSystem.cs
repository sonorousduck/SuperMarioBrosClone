using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace SequoiaEngine
{
    public class SpritesheetAnimationSystem : System
    {
        public SpritesheetAnimationSystem(SystemManager systemManager) : base(systemManager, typeof(AnimationController))
        {
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (uint id in gameObjects.Keys)
            {
                if (!gameObjects[id].IsEnabled()) continue;

                AnimationController animationController = gameObjects[id].GetComponent<AnimationController>();
                Node currentNodeManager = animationController.GetCurrentNode();
                Subnode currentNode = currentNodeManager.Subnodes[currentNodeManager.CurrentSubnode];


                bool hasCompletedPlay = true;


                foreach (AnimatedSprite animation in currentNode.Animations)
                {
                    animation.UpdateElapsedTime(gameTime, gameObjects[id]);
                    hasCompletedPlay = hasCompletedPlay && animation.HasCompletedPlay;
                }

                foreach (Link link in currentNodeManager.Links)
                {
                    if (link.ShouldFinishAnimationBeforeTransition && !hasCompletedPlay)
                    {
                        continue;
                    }
                    else if (link.ShouldTraverseLink.Invoke())
                    {
                        foreach (AnimatedSprite animation in currentNode.Animations)
                        {
                            animation.ResetAnimation();
                        }

                        animationController.CurrentNode = link.DestinationNode;
                    }
                }

                foreach (Subnode subnode in animationController.GetCurrentNode().Subnodes.Values) 
                {
                    if (animationController.GetCurrentNode().CurrentSubnode != subnode.Name && (subnode.ShouldTransitionToThisNode?.Invoke() ?? false))
                    {
                        animationController.GetCurrentNode().CurrentSubnode = subnode.Name;
                        break;
                    }
                }


            }
        }
    }
}