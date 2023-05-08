using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public int totalCurrency;
    public int score;

    public int level;

    public int damageBullet;
    public int damagePunch;
    public int damageFireball;

    public float movementSpeed;

    public int playerHealth;

    public GameData()
    {
        this.currency = 0;
        this.totalCurrency = 0;
        this.score = 0;
        this.level = 1;

        this.movementSpeed = 1f;

        this.playerHealth = 5;
    }
}
