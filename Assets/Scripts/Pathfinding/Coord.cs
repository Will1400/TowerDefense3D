using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Coord : IEquatable<Coord>
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
            return Equals(coord);

        return false;
    }

    public bool Equals(Coord other)
    {
        return other != null &&
               x == other.x &&
               y == other.y;
    }

    public override int GetHashCode()
    {
        var hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return $"x: {x} y: {y}";
    }
    
    public static bool operator ==(Coord first, Coord second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(Coord first, Coord second)
    {
        return !first.Equals(second);
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