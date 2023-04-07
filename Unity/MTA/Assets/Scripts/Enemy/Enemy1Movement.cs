using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Movement : MonoBehaviour
{
    [SerializeField] float moveVelocity;
    [SerializeField] float moveUntilDistance;
    [SerializeField] float visionRange;
    [SerializeField] EnemyHealth enemyHealthScript;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisEnemyRB;

    private EnemySpawner thisEnemySpawnerScript;
    private EnemyManager enemyManagerScript;

    private Vector2 thisEnemyPosition;
    private Vector2 playerPosition;
    private Vector2 roomCenter;

    private float topWall;
    private float bottomWall;
    private float rightWall;
    private float leftWall;

    private Vector2 point1;
    private Vector2 point2 = Vector2.zero;
    private bool atPoint1 = true;
    private bool foundPoint2 = false;
    
    private void Awake() 
    {
        thisEnemyRB = this.GetComponent<Rigidbody2D>();
    }

    private void Start() 
    {
        thisEnemySpawnerScript = this.transform.parent.gameObject.GetComponent<EnemySpawner>();
        enemyManagerScript = thisEnemySpawnerScript.enemyManagerScript;
        topWall = enemyManagerScript.topWall;
        bottomWall = enemyManagerScript.bottomWall;
        rightWall = enemyManagerScript.rightWall;
        leftWall = enemyManagerScript.leftWall;
        roomCenter = thisEnemySpawnerScript.roomCenter;

        point1 = this.transform.position;
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
            Patrol();
        }
    }

    private void Patrol()
    {
        if (point2 == Vector2.zero || Vector2.Distance(point1, point2) < rightWall)
        {
            point2 = RandomPointInRoom2D();
        }
        else
        {
            foundPoint2 = true;
        }

        if (foundPoint2)
        {
            if (thisEnemyPosition == point1 || (atPoint1 && thisEnemyPosition != point2))
            {
                atPoint1 = true;
                thisEnemyRB.position = Vector2.MoveTowards(thisEnemyPosition, point2, moveVelocity * Time.deltaTime);
            }
            else if (thisEnemyPosition == point2 || (!atPoint1 && thisEnemyPosition != point1))
            {
                atPoint1 = false;
                thisEnemyRB.position = Vector2.MoveTowards(thisEnemyPosition, point1, moveVelocity * Time.deltaTime);
            }
        }
    }

    private Vector2 RandomPointInRoom2D()
    {
        return new Vector2(Random.Range(roomCenter.x + leftWall, roomCenter.x + rightWall), Random.Range(roomCenter.y + bottomWall, roomCenter.y + topWall));
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
