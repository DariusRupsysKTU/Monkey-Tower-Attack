using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public double currency;
    public double score;

    public GameData()
    {
        this.currency = 0;
        this.score = 0;
    }
}
