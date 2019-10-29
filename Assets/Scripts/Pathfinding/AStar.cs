using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStar
{

    private int[,] map;

    public List<Coord> GetPath(int[,] map, Coord start, Coord end)
    {
        this.map = map;
        List<Coord> path = new List<Coord>();

        Node startNode = new Node(start);
        Node endNode = new Node(end);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.Count > 0) // Loop til end is found
        {
            Node currentNode = openList[0];

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

                if (openList.Contains(child) && child.G > openList[openList.IndexOf(child)].G)
                    continue;

                openList.Add(child);
            }
        }

        return new List<Coord>();
    }

    List<Node> GetValidAdjacentNodes(Node node)
    {
        List<Node> nodes = new List<Node>();
        int x = node.Position.x;
        int y = node.Position.y;

        nodes.Add(ConvertPositionToNode(new Coord(x, y - 1)));
        nodes.Add(ConvertPositionToNode(new Coord(x, y + 1)));
        nodes.Add(ConvertPositionToNode(new Coord(x - 1, y)));
        nodes.Add(ConvertPositionToNode(new Coord(x + 1, y)));

        for (int i = 0; i < nodes.Count; i++)
        {
            if (!IsValidPosition(nodes[i].Position))
            {
                nodes.RemoveAt(i);
                i--;
            }
        }

        return nodes;

        bool IsValidPosition(Coord pos)
        {
            if (pos.x > map.GetUpperBound(0) || pos.y > map.GetUpperBound(1) || pos.x < 0 || pos.y < 0)
                return false;

            if (map[pos.x, pos.y] != 2)
                return true;

            return false;
        }


        Node ConvertPositionToNode(Coord pos)
        {
            return new Node(pos);
        }
    }


    class Node
    {
        public int G;
        public int H;
        public int F;

        public Coord Position;
        public Node parent;

        public Node(Coord position, Node parent = null)
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
    }

}

public class Coord
{
    public int x;
    public int y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}