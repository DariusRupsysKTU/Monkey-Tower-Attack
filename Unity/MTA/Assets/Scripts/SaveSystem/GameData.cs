using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public int score;

    public Vector2 playerPosition;

    public GameData()
    {
        this.currency = 0;
        this.score = 0;
        playerPosition = Vector2.zero;
    }
}
