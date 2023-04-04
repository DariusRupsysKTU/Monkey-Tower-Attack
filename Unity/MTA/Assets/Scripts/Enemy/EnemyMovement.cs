using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveVelocity;
    [SerializeField] float moveUntilDistance;
    [SerializeField] float visionRange;
    [SerializeField] EnemyHealth enemyHealthScript;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisEnemyRB;

    private Vector2 thisEnemyPosition;
    private Vector2 playerPosition;
    
    private void Awake() 
    {
        thisEnemyRB = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindPlayer();

        thisEnemyPosition = thisEnemyRB.position;
        playerPosition = playerRB.position;

        if (playerObject != null && enemyHealthScript.enemyHealth > 0)
        {
            MoveEnemy(playerPosition);
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

    private void MoveEnemy(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(thisEnemyPosition, targetPosition);

        if (distance <= visionRange && distance > moveUntilDistance)
        {
            thisEnemyRB.position = Vector2.MoveTowards(thisEnemyPosition, targetPosition, moveVelocity * Time.deltaTime);
        }
        else
        {
            thisEnemyRB.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.transform.tag == "Player" && enemyHealthScript.enemyHealth > 0)
        {
            playerHealth.DamagePlayer(1);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (other.transform.tag == "Player" && enemyHealthScript.enemyHealth > 0)
        {
            playerHealth.DamagePlayer(1);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
