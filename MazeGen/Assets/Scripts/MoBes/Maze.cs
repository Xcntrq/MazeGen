using System.Collections.Generic;

public class Maze
{
    public int _width;
    public int _height;

    public Vertex[,] Vertices;

    private HashSet<Vertex> _visited;

    public Maze(int width, int height)
    {
        _width = width;
        _height = height;
        Vertices = new Vertex[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Vertices[x, y] = new Vertex(x, y);
            }
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (x > 0)
                    Vertices[x, y].Neighbours.Add(Vertices[x - 1, y]);

                if (x < _width - 1)
                    Vertices[x, y].Neighbours.Add(Vertices[x + 1, y]);

                if (y > 0)
                    Vertices[x, y].Neighbours.Add(Vertices[x, y - 1]);

                if (y < _height - 1)
                    Vertices[x, y].Neighbours.Add(Vertices[x, y + 1]);
            }
        }
    }
}