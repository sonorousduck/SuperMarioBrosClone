using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class Link
    {
        public string DestinationNode;
        public bool ShouldFinishAnimationBeforeTransition;
        public Func<bool> ShouldTraverseLink;

        public Link(string destinationNode, Func<bool> shouldTraverse, bool shouldFinishAnimationBeforeTransition = false) 
        {
            this.DestinationNode = destinationNode;
            this.ShouldTraverseLink = shouldTraverse;
            this.ShouldFinishAnimationBeforeTransition = shouldFinishAnimationBeforeTransition;
        }
    }

    /// <summary>
    /// Subnodes make up nodes. This allows for easy transition between subnode transitions (i.e. walk_right, walk_left, walk_up, walk_down) instead
    /// of having to create transitions from every possible idle animation to every possible walk animation
    /// </summary>
    public class Subnode
    {
        public string Name;
        public List<AnimatedSprite> Animations;
        public Func<bool> ShouldTransitionToThisNode;

        public Subnode(string name, List<AnimatedSprite> animations, Func<bool> shouldTransitionToThisNode)
        {
            Name = name;
            Animations = animations;
            ShouldTransitionToThisNode = shouldTransitionToThisNode;
        }

        public Subnode(string name)
        {
            Name = name;
            Animations = new();
            ShouldTransitionToThisNode = null;
        }

        public void SetTransition(Func<bool> transition)
        {
            this.ShouldTransitionToThisNode = transition;
        }

        public void AddAnimation(AnimatedSprite anim)
        {
            this.Animations.Add(anim);
        }


    }

    public class Node
    {
        public List<Link> Links;
        public Dictionary<string, Subnode> Subnodes = new();
        public string CurrentSubnode;
        public string DefaultSubnode;
         
        public Node(List<Link> links, List<AnimatedSprite> animations)
        {
            this.Links = links;
        }

        public Node()
        {
            this.Links = new();
        }
        public void AddLink(Link link)
        {
            this.Links.Add(link);
        }

        public void AddSubnode(Subnode subnode)
        {
            if (this.Subnodes.Count == 0)
            {
                this.DefaultSubnode = subnode.Name;
                this.CurrentSubnode = subnode.Name;
            }

            this.Subnodes.Add(subnode.Name, subnode);
        }

        public void AddSubnode(string subnodeName, List<AnimatedSprite> animations, Func<bool> transition, bool isDefault = false)
        {
            if (this.Subnodes.Count == 0)
            {
                this.DefaultSubnode = subnodeName;
                this.CurrentSubnode = subnodeName;
            }

            if (isDefault)
            {
                this.DefaultSubnode = subnodeName;
            }

            this.Subnodes.Add(subnodeName, new Subnode(subnodeName, animations, transition));
        }

    }

    public class AnimationTree
    {
        public Dictionary<string, Node> Tree = new();
        
        public void AddNode(string animationNodeName, Node node) 
        {
            Tree.Add(animationNodeName, node);
        }

        public void RemoveNode(string animationNodeName)
        {
            Tree.Remove(animationNodeName);
        }
    }


    public class AnimationController : RenderedComponent
    {
        public string CurrentNode = null;
        public AnimationTree AnimationTree;
        public float RenderDepth;

        public AnimationController(float renderDepth = 1.0f)
        {
            this.RenderDepth = renderDepth;
            this.AnimationTree = new();
        }

        public AnimationController(AnimationTree animationTree, float renderDepth)
        {
            this.AnimationTree = animationTree;
            this.RenderDepth = renderDepth;
        }

        public void Play(string animationName)
        {
            this.CurrentNode = animationName;
        }

        public void AddAnimation(string animationName, Node node)
        {
            AnimationTree.AddNode(animationName, node);

            if (AnimationTree.Tree.Count == 1)
            {
                this.CurrentNode = animationName;
            }
        }

        public void AddAnimation(string animationName, List<AnimatedSprite> animatedSprites, List<Link> links)
        {
            AnimationTree.AddNode(animationName, new Node(links, animatedSprites));

            if (AnimationTree.Tree.Count == 1)
            {
                this.CurrentNode = animationName;
            }
        }

        public void RemoveAnimation(string animationName)
        {
            AnimationTree.RemoveNode(animationName);

            if (this.CurrentNode == animationName)
            {
                if (AnimationTree.Tree.Count == 0)
                {
                    this.CurrentNode = null;
                }
                else
                {
                    this.CurrentNode = AnimationTree.Tree.First().Key;
                }
            }
        }

        public List<AnimatedSprite> GetCurrentAnimation()
        {
            if (this.CurrentNode == null)
            {
                return null;
            }

            return this.AnimationTree.Tree[this.CurrentNode].Subnodes[this.AnimationTree.Tree[this.CurrentNode].CurrentSubnode].Animations;
        }

        public Node GetCurrentNode()
        {
            if (this.CurrentNode == null)
            {
                return null;
            }

            return this.AnimationTree.Tree[this.CurrentNode];
        }

        public List<Link> GetCurrentLinks()
        {
            if (this.CurrentNode == null)
            {
                return null;
            }

            return GetCurrentNode().Links;
        }

        public List<AnimatedSprite> GetSprites()
        {
            return this.AnimationTree.Tree[this.CurrentNode].Subnodes[this.AnimationTree.Tree[this.CurrentNode].CurrentSubnode].Animations;
        }
    }
}
