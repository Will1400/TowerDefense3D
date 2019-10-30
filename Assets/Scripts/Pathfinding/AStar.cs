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
        Node currentNode = openList[0];

        while (openList.Count > 0 && !closedList.Exists(x => x.Position.Equals(endNode.Position))) // Loop til end is found
        {

            currentNode = openList.OrderBy(x => x.F).FirstOrDefault();

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> children = GetValidAdjacentNodes(currentNode);

            foreach (Node child in children) // stuck
            {
                if (closedList.Contains(child) || openList.Contains(child))
                    continue;

                child.parent = currentNode;
                child.H = Math.Abs(child.Position.x - endNode.Position.x) + Math.Abs(child.Position.y - endNode.Position.y);
                child.G = currentNode.G + 1;
                child.F = child.G + child.H;

                openList.Add(child);
            }
        }

        if (currentNode.Equals(endNode)) // Found goal
        {
            Node current = currentNode;

            while (current != null)
            {
                path.Add(current.Position);
                current = current.parent;
            }
        }

        return path;
    }

    List<Node> GetValidAdjacentNodes(Node node)
    {
        List<Node> nodes = new List<Node>();
        int x = node.Position.x;
        int y = node.Position.y;

        if (IsValidPosition(new Coord(x, y - 1)))
            nodes.Add(ConvertPositionToNode(new Coord(x, y - 1)));
        if (IsValidPosition(new Coord(x, y + 1)))
            nodes.Add(ConvertPositionToNode(new Coord(x, y + 1)));
        if (IsValidPosition(new Coord(x - 1, y)))
            nodes.Add(ConvertPositionToNode(new Coord(x - 1, y)));
        if (IsValidPosition(new Coord(x + 1, y)))
            nodes.Add(ConvertPositionToNode(new Coord(x + 1, y)));


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
                return Position.Equals(node.Position);

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

    public override bool Equals(object obj)
    {
        if (obj is Coord coord)
            return coord.x == x && coord.y == y;

        return false;
    }

    public override string ToString()
    {
        return $"x: {x} y: {y}";
    }
}