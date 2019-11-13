using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return x == coord.x && y == coord.y;

        return false;
    }

    public override string ToString()
    {
        return $"x: {x} y: {y}";
    }
    
    public static bool operator ==(Coord first, Coord second)
    {
        return first.x == second.x && first.x == second.y;
    }

    public static bool operator !=(Coord first, Coord second)
    {
        return !(first == second);
    }

    public static Coord operator +(Coord first, Coord second)
    {
        return new Coord(first.x + second.x, first.y + second.y);
    }

    public static Coord operator -(Coord first, Coord second)
    {
        return new Coord(first.x - second.x, first.y - second.y);
    }
}