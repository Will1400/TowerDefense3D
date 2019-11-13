using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStar
{
    private int[,] map;
    private Coord start;
    private Coord end;

    public List<Coord> GetPath(int[,] map, Coord start, Coord end)
    {
        this.map = map;
        this.start = start;
        this.end = end;
        List<Coord> path = new List<Coord>();

        Node startNode = new Node(start);
        Node endNode = new Node(end);

        Heap<Node> openList = new Heap<Node>(map.GetUpperBound(0) * map.GetUpperBound(1));
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);
        Node currentNode = null;

        while (openList.Count > 0 && !closedList.Select(x => x.Position).Contains(end)) // Loop til end is found
        {
            currentNode = openList.RemoveFirst();

            closedList.Add(currentNode);

            List<Node> children = GetValidAdjacentNodes(currentNode);

            foreach (Node child in children)
            {
                if (closedList.Contains(child) || openList.Contains(child))
                    continue;

                child.parent = currentNode;
                child.H = Math.Abs(child.Position.x - endNode.Position.x) + Math.Abs(child.Position.y - endNode.Position.y);
                child.G = currentNode.G + 1;

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
            if (pos.x > map.GetUpperBound(0) || pos.y > map.GetUpperBound(1) || pos.x < 0 || pos.y < 0) // outside of map
                return false;

            if (map[pos.x, pos.y] != 2)
            {
                if ((map[pos.x, pos.y] == 9 || map[pos.x, pos.y] == 10) && !pos.Equals(end) && !pos.Equals(start))
                    return false;

                return true;
            }

            return false;
        }

        Node ConvertPositionToNode(Coord pos)
        {
            return new Node(pos);
        }
    }

    private class Node : IHeapItem<Node>
    {
        public int G;
        public int H;
        public int F
        {
            get
            {
                return H + G;
            }
        }

        public Coord Position;
        public Node parent;

        public int HeapIndex { get; set; }

        public Node(Coord position, Node parent = null)
        {
            Position = position;
            this.parent = parent;
        }

        public int CompareTo(Node other)
        {
            int compare = F.CompareTo(other.F);
            if (compare == 0)
                compare = H.CompareTo(other.H);

            return -compare;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node node)
                return Position.Equals(node.Position);

            return false;
        }
    }
}
