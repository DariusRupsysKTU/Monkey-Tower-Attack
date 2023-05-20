using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Movement : MonoBehaviour
{
    [SerializeField] float moveVelocity;
    [SerializeField] float moveUntilDistance;
    public float visionRange;
    [SerializeField] int damageOnTouch;
    [SerializeField] bool explodes;

    private EnemyHealth enemyHealthScript;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisEnemyRB;

    private EnemySpawner thisEnemySpawnerScript;

    private Vector2 thisEnemyPosition;
    private Vector2 playerPosition;
    private Vector2 roomCenter;

    // wall distances from center
    private float topWall;
    private float bottomWall;
    private float rightWall;
    private float leftWall;
    // wall positions
    private float topWallPosition;
    private float bottomWallPosition;
    private float rightWallPosition;
    private float leftWallPosition;

    private Vector2 point1;
    private Vector2 point2 = Vector2.zero;
    private bool atPoint1 = true;
    private bool foundPoint2 = false;

    private GameObject pauseMenu;
    
    private void Awake() 
    {
        thisEnemyRB = this.GetComponent<Rigidbody2D>();
        enemyHealthScript = this.GetComponent<EnemyHealth>();
    }

    private void Start() 
    {
        if (this.transform.parent.gameObject.TryGetComponent<EnemySpawner>(out thisEnemySpawnerScript))
        {
            topWall = thisEnemySpawnerScript.topWall;
            bottomWall = thisEnemySpawnerScript.bottomWall;
            rightWall = thisEnemySpawnerScript.rightWall;
            leftWall = thisEnemySpawnerScript.leftWall;
            roomCenter = thisEnemySpawnerScript.roomCenter;
            GetWallPositions();
        }

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

        if (enemyHealthScript.enemyHealth == 0 && explodes)
        {
            Explode();
        }

        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseCanvas");
        }

        if (pauseMenu != null && pauseMenu.GetComponent<PauseMenu>().cheatsOn)
        {
            KillEveryEnemy1InRoomCheat();
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

        if (distance <= visionRange && distance > moveUntilDistance && IsInTheRoom(targetPosition))
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
        CollisionHandler(other);
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        CollisionHandler(other);
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void CollisionHandler(Collision2D other)
    {
        if (other.transform.tag == "Player" && enemyHealthScript.enemyHealth > 0)
        {
            playerHealth.DamagePlayer(damageOnTouch);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;

            if (explodes)
            {
                enemyHealthScript.DamageEnemy(enemyHealthScript.enemyHealth);
                Explode();
            }
        }
        else if (other.transform.tag == "Box")
        {
            ItemHealth boxHealth = other.gameObject.GetComponent<ItemHealth>();
            boxHealth.DamageItem(damageOnTouch);
        }
    }

    private void Explode()
    {
        StartCoroutine(GetComponentInChildren<BlastWave>().Blast());
    }

    private void GetWallPositions()
    {
        topWallPosition = roomCenter.y + thisEnemySpawnerScript.enemyManagerScript.topWall;
        bottomWallPosition = roomCenter.y + thisEnemySpawnerScript.enemyManagerScript.bottomWall;
        rightWallPosition = roomCenter.x + thisEnemySpawnerScript.enemyManagerScript.rightWall;
        leftWallPosition = roomCenter.x + thisEnemySpawnerScript.enemyManagerScript.leftWall;
    }

    private bool IsInTheRoom(Vector2 targetPosition)
    {
        if (topWallPosition != 0)
        {
            if (targetPosition.x >= leftWallPosition && targetPosition.x <= rightWallPosition &&
                targetPosition.y >= bottomWallPosition && targetPosition.y <= topWallPosition)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, visionRange);    
    }

    private void KillEveryEnemy1InRoomCheat()
    {
        if (IsInTheRoom(playerPosition) && Input.GetKeyDown("k"))
        {
            enemyHealthScript.DamageEnemy(enemyHealthScript.enemyHealth);
        }
    }
}
