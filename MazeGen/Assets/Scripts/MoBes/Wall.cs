using UnityEngine;

public class Wall
{
    public Vector2 StartPoint;
    public Vector2 EndPoint;

    public Wall(Vector2 startPoint, Vector2 endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }
}