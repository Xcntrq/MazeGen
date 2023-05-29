using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    [SerializeField] private LineRenderer _prefabGrey;
    [SerializeField] private LineRenderer _prefabRed;
    [SerializeField] private LineRenderer _prefabWhite;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Vector2Int _start;
    [SerializeField] private Vector2Int _finish;

    public Vertex[,] Vertices;
    public Square[,] WallSquares;

    public int MazeLenght;
    public int AnswerLenght;

    private HashSet<Vertex> _visited;
    private List<Vertex> _answer;

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

        MazeLenght = 0;
        _visited = new HashSet<Vertex>();
        _answer = new List<Vertex>();
        ResetVertices();
        ResetWallSquares();
        _visited.Remove(Vertices[_start.x, _start.y]);
        _visited.Remove(Vertices[_finish.x, _finish.y]);
        Visit(_start.x, _start.y);

        HashSet<Wall> allWalls = new HashSet<Wall>();
        foreach (var square in WallSquares)
        {
            if (square != null)
            {
                allWalls.UnionWith(square.AllWalls);
            }
        }
        foreach (var wall in allWalls)
        {
            // LineRenderer lr = PrefabUtility.InstantiatePrefab(_prefabWhite, transform) as LineRenderer;
            LineRenderer lr = Instantiate(_prefabWhite, transform);
            lr.positionCount = 2;
            lr.SetPosition(0, wall.StartPoint);
            lr.SetPosition(1, wall.EndPoint);
        }


        Vector3[] nodes = new Vector3[_answer.Count];
        for (int i = 0; i < _answer.Count; i++)
        {
            nodes[i] = new Vector3(_answer[i].X, _answer[i].Y, 0);
        }
        // LineRenderer lrAns = PrefabUtility.InstantiatePrefab(_prefabRed, transform) as LineRenderer;
        LineRenderer lrAns = Instantiate(_prefabRed, transform);
        lrAns.positionCount = _answer.Count;
        lrAns.SetPositions(nodes);

        AnswerLenght = _answer.Count;
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

                if ((x == 0) || (x == _width - 1) || (y == 0) || (y == _height - 1))
                    _visited.Add(Vertices[x, y]);
            }
        }
    }

    private void ResetWallSquares()
    {
        WallSquares = new Square[_width, _height];

        for (int y = 1; y < _height - 1; y++)
        {
            for (int x = 1; x < _width - 1; x++)
            {
                WallSquares[x, y] = new Square(x, y, 1);
            }
        }
    }

    public bool Visit(int x, int y)
    {
        bool result = false;
        _visited.Add(Vertices[x, y]);

        for (int i = Vertices[x, y].Neighbours.Count - 1; i >= 0; i--)
        {
            int r = Random.Range(0, i + 1);

            (Vertices[x, y].Neighbours[r], Vertices[x, y].Neighbours[i]) = (Vertices[x, y].Neighbours[i], Vertices[x, y].Neighbours[r]);

            if (!_visited.Contains(Vertices[x, y].Neighbours[i]))
            {
                if (Vertices[x, y].Neighbours[i].X > x)
                {
                    if (WallSquares[x, y] != null)
                    {
                        WallSquares[x, y].AllWalls.Remove(WallSquares[x, y].RightWall);
                    }
                    if (WallSquares[x + 1, y] != null)
                    {
                        WallSquares[x + 1, y].AllWalls.Remove(WallSquares[x + 1, y].LeftWall);
                    }
                }

                if (Vertices[x, y].Neighbours[i].X < x)
                {
                    if (WallSquares[x, y] != null)
                    {
                        WallSquares[x, y].AllWalls.Remove(WallSquares[x, y].LeftWall);
                    }
                    if (WallSquares[x - 1, y] != null)
                    {
                        WallSquares[x - 1, y].AllWalls.Remove(WallSquares[x - 1, y].RightWall);
                    }
                }

                if (Vertices[x, y].Neighbours[i].Y > y)
                {
                    if (WallSquares[x, y] != null)
                    {
                        WallSquares[x, y].AllWalls.Remove(WallSquares[x, y].TopWall);
                    }
                    if (WallSquares[x, y + 1] != null)
                    {
                        WallSquares[x, y + 1].AllWalls.Remove(WallSquares[x, y + 1].BottomWall);
                    }
                }

                if (Vertices[x, y].Neighbours[i].Y < y)
                {
                    if (WallSquares[x, y] != null)
                    {
                        WallSquares[x, y].AllWalls.Remove(WallSquares[x, y].BottomWall);
                    }
                    if (WallSquares[x, y - 1] != null)
                    {
                        WallSquares[x, y - 1].AllWalls.Remove(WallSquares[x, y - 1].TopWall);
                    }
                }

                // LineRenderer lr = PrefabUtility.InstantiatePrefab(_prefabGrey, transform) as LineRenderer;
                LineRenderer lr = Instantiate(_prefabGrey, transform);
                lr.positionCount = 2;
                lr.SetPosition(0, new Vector2(Vertices[x, y].X, Vertices[x, y].Y));
                lr.SetPosition(1, new Vector2(Vertices[x, y].Neighbours[i].X, Vertices[x, y].Neighbours[i].Y));
                result |= Visit(Vertices[x, y].Neighbours[i].X, Vertices[x, y].Neighbours[i].Y);
                MazeLenght++;
            }
        }

        result |= ((x == _finish.x) && (y == _finish.y));

        if (result)
            _answer.Add(Vertices[x, y]);

        return result;
    }
}
