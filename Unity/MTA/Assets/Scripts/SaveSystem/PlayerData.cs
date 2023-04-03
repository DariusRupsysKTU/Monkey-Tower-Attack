using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
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

    public PlayerData(GameObject player)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //health = player;
        //money = GetComponent<Inventory>();

        //position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
