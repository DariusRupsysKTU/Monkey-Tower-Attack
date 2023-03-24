using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2 gridPos;
    public int type;
    public bool doorTop, doorBot, doorRight, doorLeft;

    public Room(Vector2 newGridPos, int newType)
    {
        gridPos = newGridPos;
        type = newType;
    }
}
