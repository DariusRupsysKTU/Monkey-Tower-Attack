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

    public int bulletDamage;
    public int punchDamage;
    public int fireballDamage;

    public float movementSpeed;

    public float cooldown;

    public int playerHealth;

    public GameData()
    {
        this.currency = 0;
        this.totalCurrency = 0;
        this.score = 0;
        this.level = 1;
        
        this.bulletDamage = 1;
        this.punchDamage = 2;
        this.fireballDamage = 3;

        this.cooldown = 2;

        this.movementSpeed = 1f;

        this.playerHealth = 5;
    }
}
