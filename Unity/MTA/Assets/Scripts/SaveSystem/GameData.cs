using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public int totalCurrency;
    public int score;

    public int playerHealth;
    GameObject grid;

    public Vector2 playerPosition;

    public GameData()
    {
        this.currency = 0;
        this.totalCurrency = 0;
        this.score = 0;
        this.playerHealth = 5;
        this.grid = null;
        playerPosition = Vector2.zero;
    }
}
