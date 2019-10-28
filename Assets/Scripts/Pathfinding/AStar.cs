using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar 
{

    private int[,] map;

    public List<Vector2Int> GetPath(int[,] map, Vector2Int start, Vector2Int end)
    {
        this.map = map;
        List<Vector2Int> path = new List<Vector2Int>();

        Node startNode = new Node(start);
        Node endNode = new Node(end);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.Count > 0) // Loop til end is found
        {
            Node currentNode = openList[0];
            int currentIndex = 0;

            currentNode = openList.OrderBy(x => x.F).FirstOrDefault();

            openList.Remove(currentNode);
            closedList.Add(currentNode);


            if (currentNode == endNode) // Found goal
            {
                Node current = currentNode;

                while (current != null)
                {
                    path.Add(current.Position);
                    current = current.parent;
                }
                return path;
            }

            List<Node> children = GetValidAdjacentNodes(currentNode);


            foreach (Node child in children)
            {
                if (closedList.Contains(child))
                    continue;

                child.G = currentNode.G + 1;
                child.H = Convert.ToInt32(Math.Pow(child.Position.x - endNode.Position.x, 2) + Math.Pow(child.Position.y - endNode.Position.y, 2));
                child.F = child.G + child.H;

                foreach (Node openNode in openList)
                {
                    if (child.Equals(openNode) && child.G > openNode.G)
                        continue;
                }
                openList.Add(child);
            }
        }

        return new List<Vector2Int>();
    }

    List<Node> GetValidAdjacentNodes(Node node)
    {
        List<Node> nodes = new List<Node>();
        int x = node.Position.x;
        int y = node.Position.y;

        nodes.Add(ConvertPositionToNode(new Vector2Int(x, y - 1)));
        nodes.Add(ConvertPositionToNode(new Vector2Int(x, y + 1)));
        nodes.Add(ConvertPositionToNode(new Vector2Int(x - 1, y)));
        nodes.Add(ConvertPositionToNode(new Vector2Int(x + 1, y)));

        for (int i = 0; i < nodes.Count; i++)
        {
            if (!IsValidPosition(nodes[i].Position))
            {
                nodes.RemoveAt(i);
                i--;
            }
        }

        return nodes;

        bool IsValidPosition(Vector2Int pos)
        {
            if (pos.x > map.GetUpperBound(0) || pos.y > map.GetUpperBound(1) || pos.x < 0 || pos.y < 0)
                return false;

            if (map[pos.x, pos.y] != 2)
                return true;

            return false;
        }


        Node ConvertPositionToNode(Vector2Int pos)
        {
            return new Node(pos);
        }
    }


    class Node : IEquatable<Node>
    {
        public int G;
        public int H;
        public int F;

        public Vector2Int Position;
        public Node parent;

        public Node(Vector2Int position, Node parent = null)
        {
            Position = position;
            this.parent = parent;
        }


        public override bool Equals(object obj)
        {
            if (obj is Node node)
                return Position == node.Position;

            return false;
        }

        public bool Equals(Node other)
        {
            return other != null &&
                   Position.Equals(other.Position);
        }

        public override int GetHashCode()
        {
            var hashCode = -38550939;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Position);
            return hashCode;
        }
    }
}
