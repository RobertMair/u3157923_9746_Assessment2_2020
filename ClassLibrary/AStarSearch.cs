using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    // Source code from https://dotnetcoretutorials.com/2020/07/25/a-search-pathfinding-algorithm-in-c/ and
    // repo at https://gist.github.com/DotNetCoreTutorials/08b0210616769e81034f53a6a420a6d9
    public class AStarSearch
    {
        public static (int nextX, int nextY) AStar(int startX, int startY, int endX, int endY)
        {

            var startNode = new Node();

            startNode.X = startX;
            startNode.Y = startY;

            var endNode = new Node();

            endNode.X = endX;
            endNode.Y = endY;

            startNode.SetDistance(endNode.X, endNode.Y);

            var activeNodes = new List<Node>();
            activeNodes.Add(startNode);
            var visitedNodes = new List<Node>();

            while (activeNodes.Any())
            {
                var checkNode = activeNodes.OrderBy(x => x.CostDistance).First();

                // When destination/endnode is found with the lowest cost option (due to the OrderBy above)
                // the path is retraced back to it's origin and placed into a final 'path list' (which is
                // then reversed so it is then the least cost path option from the origin to the destination.
                if (checkNode.X == endNode.X && checkNode.Y == endNode.Y)
                {
                    var node = checkNode;
                    var pathNodes = new List<Node>();

                    while (true)
                    {
                        pathNodes.Add(node);
                        node = node.Parent;

                        if (node == null)
                        {
                            pathNodes.Reverse();
                            return (pathNodes[1].X, pathNodes[1].Y);
                        }
                    }
                }

                visitedNodes.Add(checkNode);
                activeNodes.Remove(checkNode);

                var walkableNodes = GetWalkableNodes(checkNode, endNode);

                foreach (var walkableNode in walkableNodes)
                {
                    // This node has already been visited so it doesn't need to be visited again.
                    if (visitedNodes.Any(x => x.X == walkableNode.X && x.Y == walkableNode.Y))
                        continue;

                    // This node is already in the active list, however it may now produce a lower cost option e.g.
                    // the path might have zigzagged but is now straighter). 
                    if (activeNodes.Any(x => x.X == walkableNode.X && x.Y == walkableNode.Y))
                    {
                        var existingNode = activeNodes.First(x => x.X == walkableNode.X && x.Y == walkableNode.Y);
                        if (existingNode.CostDistance > checkNode.CostDistance)
                        {
                            activeNodes.Remove(existingNode);
                            activeNodes.Add(walkableNode);
                        }
                    }
                    else
                    {
                        // This node has not been visited so it is added to the list. 
                        activeNodes.Add(walkableNode);
                    }
                }
            }

            return (startX, startY);
        }

        private static List<Node> GetWalkableNodes(Node currentNode, Node targetNode)
        {
            var possibleNodes = new List<Node>();

            // Checks node (if any) above current node to see if it can be entered/walkable.
            if (currentNode.Y - 1 >= 0)
            {
                if (NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y - 1][currentNode.X]].Up == -1 &&
                   NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y - 1][currentNode.X]].Walkable)
                {
                    var item = new Node
                    { X = currentNode.X, Y = currentNode.Y - 1, Parent = currentNode, Cost = currentNode.Cost + 1 };
                    possibleNodes.Add(item);
                }
            }

            // Checks node (if any) below current node to see if it can be entered/walkable.
            if (currentNode.Y + 1 <= Grid.GridYSize - 1)
            {
                if (NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y + 1][currentNode.X]].Down == 1 &&
                    NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y + 1][currentNode.X]].Walkable)
                {
                    var item = new Node { X = currentNode.X, Y = currentNode.Y + 1, Parent = currentNode, Cost = currentNode.Cost + 1 };
                    possibleNodes.Add(item);
                }
            }

            // Checks node (if any) left of current node to see if it can be entered/walkable.
            if (currentNode.X - 1 >= 0)
            {
                if (NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y][currentNode.X - 1]].Left == -1 &&
                    NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y][currentNode.X - 1]].Walkable)
                {
                    var item = new Node { X = currentNode.X - 1, Y = currentNode.Y, Parent = currentNode, Cost = currentNode.Cost + 1 };
                    possibleNodes.Add(item);
                }
            }

            // Checks node (if any) right of current node to see if it can be entered/walkable.
            if (currentNode.X + 1 <= Grid.GridXSize - 1)
            {
                if (NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y][currentNode.X + 1]].Right == 1 &&
                    NodeContentCollection.NodeContents[Grid.NodeGrid[currentNode.Y][currentNode.X + 1]].Walkable)
                {
                    var item = new Node { X = currentNode.X + 1, Y = currentNode.Y, Parent = currentNode, Cost = currentNode.Cost + 1 };
                    possibleNodes.Add(item);
                }
            }

            possibleNodes.ForEach(node => node.SetDistance(targetNode.X, targetNode.Y)); // Example use of Lambda expression to iterate through a list of node object instances.

            return possibleNodes.ToList();
        }
    }

    class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Node Parent { get; set; }

        //The distance is essentially the estimated distance, ignoring walls to our target. 
        //So how many tiles left and right, up and down, ignoring walls, to get there. 
        public void SetDistance(int targetX, int targetY)
        {
            Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }
    }
}