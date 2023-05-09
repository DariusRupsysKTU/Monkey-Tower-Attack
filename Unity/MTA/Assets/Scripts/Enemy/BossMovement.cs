using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] float moveVelocity;
    [SerializeField] float visionRange;
    [SerializeField] float shootRangeMultiplier;
    [SerializeField] int stage2BulletCount;
    [SerializeField] float stopDistance;
    [SerializeField] float retreatDistance;
    [SerializeField] EnemyHealth bossHealthScript;
    [SerializeField] GameObject[] pawnPrefabs;
    [SerializeField] GameObject bulletPrefab;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisBossRB;

    private EnemySpawner thisEnemySpawnerScript;
    private EnemyManager enemyManagerScript;

    private Vector2 thisBossPosition;
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
    private float spawnRange = 0.5f;

    private int startHealth;

    private bool tooCloseToWall = false;
    private bool lastAttackDone = false;

    // time settings
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    private float timeBetweenSpawns;
    public float startTimeBetweenSpawns; 
    
    private void Awake() 
    {
        thisBossRB = this.GetComponent<Rigidbody2D>();
        startHealth = bossHealthScript.enemyHealth;
    }

    private void Start() 
    {
        if (this.transform.parent.gameObject.TryGetComponent<EnemySpawner>(out thisEnemySpawnerScript))
        {
            enemyManagerScript = thisEnemySpawnerScript.enemyManagerScript;
            topWall = enemyManagerScript.topWall;
            bottomWall = enemyManagerScript.bottomWall;
            rightWall = enemyManagerScript.rightWall;
            leftWall = enemyManagerScript.leftWall;
            roomCenter = thisEnemySpawnerScript.roomCenter;
            GetWallPositions();
        }

        point1 = this.transform.position;

        timeBetweenShots = startTimeBetweenShots;
        timeBetweenSpawns = startTimeBetweenSpawns;
    }

    void Update()
    {
        FindPlayer();

        thisBossPosition = thisBossRB.position;
        playerPosition = playerRB.position;

        if (playerObject != null && bossHealthScript.enemyHealth > 0)
        {
            MoveBoss(playerPosition);
        }

        if (Vector2.Distance(thisBossPosition, playerPosition) <= visionRange * shootRangeMultiplier
        && IsInTheRoom(playerPosition))
        {
            if (!bossHealthScript.bossEnraged)
            {
                Shoot1();
                SpawnPawns(false);
            }
            else
            {
                Shoot2();
                SpawnPawns(true);

                if (bossHealthScript.lastAttack && !lastAttackDone)
                {
                    LastAttack();
                }
            }
        }
    }

    private void SpawnPawns(bool isEnraged)
    {
        if (timeBetweenSpawns <= 0)
        {
            GameObject pawnPrefab;

            if (!isEnraged)
            {
                pawnPrefab = pawnPrefabs[0];
            }
            else
            {
                pawnPrefab = pawnPrefabs[Random.Range(1,3)];
            }

            GameObject pawn = Instantiate(pawnPrefab, GetClosePosition(), Quaternion.identity);
            pawn.transform.parent = thisEnemySpawnerScript.gameObject.transform;
        
            // pawn.GetComponent<Enemy1Movement>().visionRange = visionRange;
            
            timeBetweenSpawns = startTimeBetweenSpawns;
        }
        else
        {
            timeBetweenSpawns -= Time.deltaTime;
        }
    }

    private void LastAttack()
    {
        GameObject pawn = Instantiate(pawnPrefabs[3], GetClosePosition(), Quaternion.identity);
        pawn.transform.parent = thisEnemySpawnerScript.gameObject.transform;
        lastAttackDone = true;
    }

    private void Shoot1()
    {
        if (timeBetweenShots <= 0)
        {
            Instantiate(bulletPrefab, thisBossPosition, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    private void Shoot2()
    {
        if (timeBetweenShots <= 0)
        {
            float bulletAngle = Mathf.Atan2((playerPosition - thisBossPosition).normalized.y, (playerPosition - thisBossPosition).normalized.x) * 180 / Mathf.PI;
            for (int i = 0; i < stage2BulletCount; i++)
            {
                int angleOffset = 360 / stage2BulletCount;
                GameObject currentBullet = Instantiate(bulletPrefab, thisBossPosition, Quaternion.identity);
                currentBullet.GetComponent<EnemyBullet>().shootAngle = DegreeToVector2(bulletAngle + (angleOffset * i)).normalized;
            }

            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
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

    private void MoveBoss(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(thisBossPosition, targetPosition);

        Vector2 targetDirection = (targetPosition - thisBossPosition).normalized;

        if (!tooCloseToWall && IsInTheRoom(targetPosition))
        {
            if (distance <= visionRange && distance > stopDistance)
            {
                thisBossRB.position = Vector2.MoveTowards(thisBossPosition, targetPosition, moveVelocity * Time.deltaTime);
            }
            else if (distance < stopDistance && distance > retreatDistance)
            {
                thisBossRB.position = this.transform.position;
            }
            else if (distance < retreatDistance)
            {
                thisBossRB.position = Vector2.MoveTowards(thisBossPosition, thisBossPosition + -targetDirection, moveVelocity * Time.deltaTime);
            }
            else
            {
                Patrol();
            }
        }
        else if (distance < retreatDistance)
        {
            thisBossRB.position = this.transform.position;
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
            if (thisBossPosition == point1 || (atPoint1 && thisBossPosition != point2))
            {
                atPoint1 = true;
                thisBossRB.position = Vector2.MoveTowards(thisBossPosition, point2, moveVelocity * Time.deltaTime);
            }
            else if (thisBossPosition == point2 || (!atPoint1 && thisBossPosition != point1))
            {
                atPoint1 = false;
                thisBossRB.position = Vector2.MoveTowards(thisBossPosition, point1, moveVelocity * Time.deltaTime);
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
        topWallPosition = roomCenter.y + topWall;
        bottomWallPosition = roomCenter.y + bottomWall;
        rightWallPosition = roomCenter.x + rightWall;
        leftWallPosition = roomCenter.x + leftWall;
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
        if (other.transform.tag == "Player" && bossHealthScript.enemyHealth > 0)
        {
            playerHealth.DamagePlayer(1, false);
            thisBossRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (other.transform.tag == "Player" && bossHealthScript.enemyHealth > 0)
        {
            playerHealth.DamagePlayer(1, false);
            thisBossRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        thisBossRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
