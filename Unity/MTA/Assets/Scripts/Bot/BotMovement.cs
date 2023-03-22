using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [SerializeField] float moveVelocity;
    [SerializeField] float moveUntilDistance;
    [SerializeField] float visionRange;
    [SerializeField] BotHealth botHealthScript;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisBotRB;

    private Vector2 thisBotPosition;
    private Vector2 playerPosition;
    
    private void Awake() 
    {
        thisBotRB = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindPlayer();

        thisBotPosition = thisBotRB.position;
        playerPosition = playerRB.position;

        if (playerObject != null && botHealthScript.botHealth > 0)
        {
            MoveBot(playerPosition);
        }
    }

    private void FindPlayer()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerObject.GetComponent<Rigidbody2D>();
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }

    private void MoveBot(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(thisBotPosition, targetPosition);

        if (distance <= visionRange && distance > moveUntilDistance)
        {
            thisBotRB.position = Vector2.MoveTowards(thisBotPosition, targetPosition, moveVelocity * Time.deltaTime);
        }
        else
        {
            thisBotRB.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.transform.tag == "Player")
        {
            playerHealth.DamagePlayer(1);
        }
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (other.transform.tag == "Player")
        {
            playerHealth.DamagePlayer(1);
        }
    }
}
