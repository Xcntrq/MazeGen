using System.Collections.Generic;
using UnityEngine;

public class Square
{
    public int X;
    public int Y;

    public Wall TopWall;
    public Wall RightWall;
    public Wall BottomWall;
    public Wall LeftWall;

    public HashSet<Wall> AllWalls;

    public Square(int x, int y, float width)
    {
        X = x;
        Y = y;

        Vector2 topLeftCorner = new Vector2(x - width / 2, y + width / 2);
        Vector2 topRightCorner = new Vector2(x + width / 2, y + width / 2);
        Vector2 bottomLeftCorner = new Vector2(x - width / 2, y - width / 2);
        Vector2 bottomRightCorner = new Vector2(x + width / 2, y - width / 2);

        TopWall = new Wall(topLeftCorner, topRightCorner);
        RightWall = new Wall(topRightCorner, bottomRightCorner);
        BottomWall = new Wall(bottomLeftCorner, bottomRightCorner);
        LeftWall = new Wall(topLeftCorner, bottomLeftCorner);

        AllWalls = new HashSet<Wall>();
        AllWalls.Add(TopWall);
        AllWalls.Add(RightWall);
        AllWalls.Add(BottomWall);
        AllWalls.Add(LeftWall);
    }
}