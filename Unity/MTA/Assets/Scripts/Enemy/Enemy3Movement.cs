using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Movement : MonoBehaviour
{
    [SerializeField] int damageOnTouch;
    [SerializeField] float moveVelocity;
    [SerializeField] float visionRange;
    [SerializeField] int cloneCount;
    [SerializeField] GameObject clonePrefab;
    [SerializeField] EnemyHealth enemyHealthScript;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisEnemyRB;
    private Color thisColor;

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
    private float spawnRange = 0.1f;

    public int startHealth;
    public float startAlpha;
    private bool alphaChanged = false;
    private int spawnedCount = 0;

    private bool tooCloseToWall = false;
    
    private void Awake() 
    {
        thisEnemyRB = this.GetComponent<Rigidbody2D>();
        startHealth = enemyHealthScript.enemyHealth;
        thisColor = this.GetComponent<SpriteRenderer>().color;
        
        // this line is only need for the first balloon
        startAlpha = thisColor.a;
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
        ChangeColor();

        FindPlayer();

        thisEnemyPosition = thisEnemyRB.position;
        playerPosition = playerRB.position;

        if (playerObject != null && enemyHealthScript.enemyHealth > 0)
        {
            MoveEnemy(playerPosition);
        }

        if (enemyHealthScript.enemyHealth == 0 && cloneCount > spawnedCount)
        {
            Divide();
        }

        KillEveryEnemy3InRoomCheat();
    }

    private void Divide()
    {
        if (startHealth - 1 > 0)
        {        
            GameObject clone = Instantiate(clonePrefab, GetClosePosition(), Quaternion.identity);
            clone.transform.parent = this.transform.parent;
            Enemy3Movement cloneMovementScript = clone.GetComponent<Enemy3Movement>();
            cloneMovementScript.startHealth = this.startHealth - 1;
            cloneMovementScript.startAlpha = (this.startAlpha / this.startHealth) * (this.startHealth - 1);
            clone.GetComponent<EnemyHealth>().enemyHealth = this.startHealth - 1;
        }

        spawnedCount++;
    }

    private void ChangeColor()
    {
        if (!alphaChanged)
        {
            thisColor.a = startAlpha;
            this.GetComponent<SpriteRenderer>().color = thisColor;
            alphaChanged = true;
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

        if (distance <= visionRange && distance > 0.1 && IsInTheRoom(targetPosition))
        {
            thisEnemyRB.position = Vector2.MoveTowards(thisEnemyPosition, targetPosition, moveVelocity * Time.deltaTime);
        }
        else
        {
            Patrol();
        }

        //if (distance <= visionRange)
        //{
            // Debug.Log(thisEnemyPosition + " " + targetPosition + " " + targetDirection);
        //}
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

    private Vector2 GetClosePosition()
    {
        Vector2 currentPosition = this.transform.position;
        float xPosition = Random.Range(currentPosition.x - spawnRange, currentPosition.x + spawnRange);
        float yPosition = Random.Range(currentPosition.y - spawnRange, currentPosition.y + spawnRange);
        return new Vector2(xPosition, yPosition);
    }

    private Vector2 RandomPointInRoom2D()
    {
        return new Vector2(Random.Range(roomCenter.x + leftWall, roomCenter.x + rightWall), Random.Range(roomCenter.y + bottomWall, roomCenter.y + topWall));
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

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.transform.tag == "WallCollider")
        {
            tooCloseToWall = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.transform.tag == "WallCollider")
        {
            tooCloseToWall = false;
        }    
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.transform.tag == "Player" && enemyHealthScript.enemyHealth > 0 && playerHealth != null)
        {
            playerHealth.DamagePlayer(1, false);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
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
            playerHealth.DamagePlayer(damageOnTouch, false);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (other.transform.tag == "Box")
        {
            ItemHealth boxHealth = other.gameObject.GetComponent<ItemHealth>();
            boxHealth.DamageItem(damageOnTouch);
        }
    }

    private void KillEveryEnemy3InRoomCheat()
    {
        if (IsInTheRoom(playerPosition) && Input.GetKeyDown("k"))
        {
            enemyHealthScript.DamageEnemy(enemyHealthScript.enemyHealth);
        }
    }
}
