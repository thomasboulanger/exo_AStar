using System;
using System.Collections.Generic;

public class Dijkstra
{
    public static List<(int, int)> Apply(int[,] maze, (int, int) startPos, (int, int) finalPos, bool allNodesValue1 = true)
    {
        // 0 - Build dictionnary with all the nodes (x,y) => Node
        Dictionary<(int, int), Node> allNodes = BuildNodes(maze, startPos);
        while (true)
        {
            // 1 - Choose the cheapest node not treated
            Node currentNode = GetCheapestNodeNotTreaded(allNodes);
            if (currentNode == null || allNodesValue1 && currentNode.isSamePos(finalPos))
            {
                // 3 - We processes all the nodes
                break;
            }
            // Mark as treated
            currentNode.done = true;
            // 2 - Update the neighbours of the current node
            UpdateNeighbours(currentNode, allNodes);
        }
        // 4 - Build shortest path
        List<(int, int)> path = BuildShortestPath(allNodes, startPos, finalPos);

        return path;
    }

    private static Dictionary<(int, int), Node> BuildNodes(int[,] maze, (int, int) startPos)
    {
        var nodesTab = new Dictionary<(int, int), Node>();

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            for (int x = 0; x < maze.GetLength(0); x++)
            {
                if (MazeGenerator.IsFree(maze,(x,y)))
                {
                    Node freeNode = new Node((x, y),startPos == (x,y));
                    nodesTab.Add((x, y), freeNode);
                }
            }
        }
        return nodesTab;
    }

    private static Node GetCheapestNodeNotTreaded(Dictionary<(int, int), Node> nodesTab)
    {
        Node toReturn = null;
        var nb = int.MaxValue;
        foreach (Node oneNode in nodesTab.Values)
        {
            if (!oneNode.done)
            {
                if (oneNode.distanceFromStart < nb)
                {
                    nb = oneNode.distanceFromStart;
                    toReturn = oneNode;
                }
                
            }   
        }
        return toReturn;
    }

    private static void UpdateNeighbours(Node currentNode, Dictionary<(int, int), Node> allNodes)
    {
        List<Node> neighbours = GetNeighbours(currentNode, allNodes);
        foreach (Node oneNeighbour in neighbours)
        {
            oneNeighbour.UpdateNeighbourIfNeeds(currentNode);
        }
    }

    private static List<Node> GetNeighbours(Node currentNode, Dictionary<(int, int), Node> allNodes)
    {
        List<Node> neighbours = new List<Node>();
        int x = currentNode.x;
        int y = currentNode.y;
        if (allNodes.ContainsKey((x + 1,y)))
        {
            Node voisinsNode = allNodes[(x+1,y)];
            if (!voisinsNode.done)
            {
                neighbours.Add(voisinsNode);
            }
        }
        if (allNodes.ContainsKey((x - 1,y)))
        {
            Node voisinsNode = allNodes[(x-1,y)];
            if (!voisinsNode.done)
            {
                neighbours.Add(voisinsNode);
            }
        }
        if (allNodes.ContainsKey((x,y+1)))
        {
            Node voisinsNode = allNodes[(x,y+1)];
            if (!voisinsNode.done)
            {
                neighbours.Add(voisinsNode);
            }
        }
        if (allNodes.ContainsKey((x,y-1)))
        {
            Node voisinsNode = allNodes[(x,y-1)];
            if (!voisinsNode.done)
            {
                neighbours.Add(voisinsNode);
            }
        }
         
        return neighbours;
    }

    private static List<(int, int)> BuildShortestPath(Dictionary<(int, int), Node> nodesTab, (int, int) startPos, (int, int) finalPos)
    {
        List<(int, int)> path = new List<(int, int)>();
        // if not path between start & final
        if (!nodesTab[finalPos].hasPrevious())
        {
            return path;
        }
        Node currentNode = nodesTab[finalPos];
        path.Add((currentNode.x, currentNode.y));
        while (currentNode.hasPrevious())
        {
            path.Insert(0, currentNode.previousPos);
            currentNode = nodesTab[currentNode.previousPos];
        }

        return path;
    }

    private class Node
    {
        private (int, int) coodinates;
        private Node previousNode;

        public Node((int, int) coodinates, bool isStart = false)
        {
            done = false;
            this.coodinates = coodinates;
            this.distanceFromStart = isStart ? 0 : int.MaxValue;
            this.previousNode = null;
        }

        public int x
        {
            get
            {
                return coodinates.Item1;
            }
        }

        public int y
        {
            get
            {
                return coodinates.Item2;
            }
        }

        public bool done { get; set; }

        public int distanceFromStart { get; private set; }

        public bool hasPrevious()
        {
            return null != previousNode;
        }

        public (int, int) previousPos
        {
            get
            {
                return previousNode.coodinates;
            }
        }

        public bool isSamePos((int, int) pos)
        {
            return coodinates == pos;
        }


        public void UpdateNeighbourIfNeeds(Node fromNode)
        {
            // distance manhattan
            int distanceBetweenNodes = Math.Abs(x - fromNode.x) + Math.Abs(y - fromNode.y);
            int newDistanceFromStart = fromNode.distanceFromStart + distanceBetweenNodes;
            if (newDistanceFromStart < distanceFromStart)
            {
                distanceFromStart = newDistanceFromStart;
                previousNode = fromNode;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                return isSamePos(((Node)obj).coodinates);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return coodinates.GetHashCode();
        }
    }
}