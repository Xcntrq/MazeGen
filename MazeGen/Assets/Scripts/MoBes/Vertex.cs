using System.Collections.Generic;

public class Vertex
{
    public int X;
    public int Y;

    public List<Vertex> Neighbours;

    public Vertex(int x, int y)
    {
        X = x;
        Y = y;
        Neighbours = new List<Vertex>();
    }
}