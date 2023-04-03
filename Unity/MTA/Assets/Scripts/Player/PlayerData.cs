using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int health;
    public float[] position;
    public double money;
    public double score;

    public GameObject player;
    
    void Start()
    {
        
    }

    private void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(player.name);
        }


    }

    public PlayerData()
    {

    }
}
