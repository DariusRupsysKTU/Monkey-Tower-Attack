using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Movement : MonoBehaviour
{
    [SerializeField] float moveVelocity;
    [SerializeField] float visionRange;
    [SerializeField] float shootRangeMultiplier;
    [SerializeField] float stopDistance;
    [SerializeField] float retreatDistance;
    [SerializeField] int cloneCount;
    [SerializeField] GameObject clonePrefab;
    [SerializeField] EnemyHealth enemyHealthScript;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisEnemyRB;
    private Color thisColor;

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
    private float spawnRange = 0.1f;

    public int startHealth;
    public float startAlpha;
    private bool alphaChanged = false;
    private int spawnedCount = 0;

    private bool tooCloseToWall = false;

    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public GameObject bulletPrefab;

    private Vector2[] startDirections = new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private Vector2[] currentDirections = new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    
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
        thisEnemySpawnerScript = this.transform.parent.gameObject.GetComponent<EnemySpawner>();
        enemyManagerScript = thisEnemySpawnerScript.enemyManagerScript;
        topWall = enemyManagerScript.topWall;
        bottomWall = enemyManagerScript.bottomWall;
        rightWall = enemyManagerScript.rightWall;
        leftWall = enemyManagerScript.leftWall;
        roomCenter = thisEnemySpawnerScript.roomCenter;

        point1 = this.transform.position;

        timeBetweenShots = startTimeBetweenShots;
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

        if (Vector2.Distance(thisEnemyPosition, playerPosition) <= visionRange * shootRangeMultiplier)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (timeBetweenShots <= 0)
        {
            Instantiate(bulletPrefab, thisEnemyPosition, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    private void Divide()
    {
        if (startHealth - 1 > 0)
        {        
            GameObject clone = Instantiate(clonePrefab, GetClosePosition(), Quaternion.identity);
            clone.GetComponent<Enemy3Movement>().startHealth = this.startHealth - 1;
            clone.GetComponent<Enemy3Movement>().startAlpha = (this.startAlpha / this.startHealth) * (this.startHealth - 1);
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

        Vector2 targetDirection = (targetPosition - thisEnemyPosition).normalized;

        if (!tooCloseToWall)
        {
            if (distance <= visionRange && distance > stopDistance)
            {
                thisEnemyRB.position = Vector2.MoveTowards(thisEnemyPosition, targetPosition, moveVelocity * Time.deltaTime);
            }
            else if (distance < stopDistance && distance > retreatDistance)
            {
                thisEnemyRB.position = this.transform.position;
            }
            else if (distance < retreatDistance)
            {
                thisEnemyRB.position = Vector2.MoveTowards(thisEnemyPosition, thisEnemyPosition + -targetDirection, moveVelocity * Time.deltaTime);
            }
            else
            {
                Patrol();
            }
        }
        else if (distance < retreatDistance)
        {
            thisEnemyRB.position = this.transform.position;
        }
        else if (distance > visionRange)
        {
            Patrol();
        }

        if (distance <= visionRange)
        {
            // Debug.Log(thisEnemyPosition + " " + targetPosition + " " + targetDirection);
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

    private void GetPossibleDirections(Vector2 targetDirection)
    {
        for (int i = 0; i < startDirections.Length; i++)
        {
            if (startDirections[i] != targetDirection)
            {
                currentDirections[i] = startDirections[i];
            }
            else
            {
                currentDirections[i] = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.transform.tag == "WallCollider")
        {
            tooCloseToWall = true;
        }    
    }

    // private void OnTriggerStay2D(Collider2D other) 
    // {
    //     if (other.transform.tag == "WallCollider")
    //     {
    //         Debug.Log(other.transform.position + " " + GetTargetDirection(other.transform.position));
    //     }    
    // }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.transform.tag == "WallCollider")
        {
            tooCloseToWall = false;
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