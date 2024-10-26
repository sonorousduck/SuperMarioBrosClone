using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class ANode
    {
        public ANode Parent;
        
        public Vector2 Position;

        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                {
                    return DistanceToTarget + Cost;
                }
                else
                {
                    return -1;
                }
            }
        }


        public bool Walkable;

        public ANode(Vector2 position, bool isWalkable, float weight = 1)
        {
            Parent = null;
            Position = position;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            Walkable = isWalkable;
        }
    }



    public class Astar
    {
        public static Astar Instance { get; private set; }
        public Grid Grid { get; private set; }
        public Grid StaticGrid { get; private set; }

        public Astar()
        {
            Instance = this;
        }

        public void UpdateDynamicGrid(Grid newGrid)
        {
            this.Grid = newGrid;
        }

        public void UpdateStaticGrid(Grid newGrid)
        {
            this.StaticGrid = newGrid;
        }

        public void UpdateBothGrids(Grid newGrid, Grid newStaticGrid)
        {
            this.Grid = Grid;
            this.StaticGrid = newStaticGrid;
        }

        public Stack<ANode> FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            ANode start = new ANode(startPosition, true);
            ANode end = new ANode(endPosition, true);


            Stack<ANode> Path = new();
            PriorityQueue<ANode, float> OpenList = new();
            List<ANode> ClosedList = new();
            List<ANode> adjacencies = new();

            ANode current = start;

            OpenList.Enqueue(start, start.F);


            while (OpenList.Count > 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);

                adjacencies = GetAdjacentNodes(current);

                foreach (ANode node in adjacencies)
                {
                    if (!ClosedList.Contains(node) && node.Walkable)
                    {
                        bool isFound = false;

                        foreach (var openNode in OpenList.UnorderedItems)
                        {
                            if (openNode.Element == node)
                            {
                                isFound = true;
                            }
                        }

                        if (!isFound)
                        {
                            node.Parent = current;
                            node.DistanceToTarget = Math.Abs(node.Position.X - end.Position.X) + Math.Abs(node.Position.Y - end.Position.Y);
                            node.Cost = node.Weight + node.Parent.Cost;
                            OpenList.Enqueue(node, node.F);
                        }
                    }
                }
            }

            if (!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            ANode temp = ClosedList[ClosedList.IndexOf(current)];
                
            if (temp == null) return null;

            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null);

            return Path;
        }

        private List<ANode> GetAdjacentNodes(ANode n)
        {
            List<ANode> adjacentNodes = new();

            int row = (int)n.Position.Y;
            int col = (int)n.Position.X;

            int indexPosition = Grid.AmountPerRow * row + col;

            if (indexPosition - 1 > 0 && indexPosition - 1 <= Grid.grid.Count)
            {
            }

            return null;



        }

    }
}
