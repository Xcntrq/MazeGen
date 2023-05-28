using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    [SerializeField] private LineRenderer _prefab;
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    public Vertex[,] Vertices;

    private HashSet<Vertex> _visited;

    public void Generate()
    {
        List<Transform> children = new List<Transform>(transform.GetComponentsInChildren<Transform>());

        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (children[i] != transform)
            {
                DestroyImmediate(children[i].gameObject);
            }
        }

        _visited = new HashSet<Vertex>();
        ResetVertices();
        Visit(0, 0);
    }

    private void ResetVertices()
    {
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

    public void Visit(int x, int y)
    {
        _visited.Add(Vertices[x, y]);

        for (int i = Vertices[x, y].Neighbours.Count - 1; i >= 0; i--)
        {
            int r = Random.Range(0, i + 1);

            (Vertices[x, y].Neighbours[r], Vertices[x, y].Neighbours[i]) = (Vertices[x, y].Neighbours[i], Vertices[x, y].Neighbours[r]);

            if (!_visited.Contains(Vertices[x, y].Neighbours[i]))
            {
                LineRenderer lr = Instantiate(_prefab, transform);
                lr.positionCount = 2;
                lr.SetPosition(0, new Vector2(Vertices[x, y].X, Vertices[x, y].Y));
                lr.SetPosition(1, new Vector2(Vertices[x, y].Neighbours[i].X, Vertices[x, y].Neighbours[i].Y));
                Visit(Vertices[x, y].Neighbours[i].X, Vertices[x, y].Neighbours[i].Y);
            }
        }
    }
}
