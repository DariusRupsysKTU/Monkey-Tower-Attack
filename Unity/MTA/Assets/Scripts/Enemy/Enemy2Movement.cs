using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Movement : MonoBehaviour
{
    //[SerializeField]
    //private Enemy2_beam linePrefab;
    //private List<Enemy2_beam> allLines;
    //private bool beamIsOn;

    [SerializeField] float moveVelocity;
    [SerializeField] float moveUntilDistance;
    [SerializeField] float visionRange;
    [SerializeField] float shootRangeMultiplier; //
    [SerializeField] float stopDistance; //
    [SerializeField] float retreatDistance; //
    [SerializeField] EnemyHealth enemyHealthScript;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private PlayerHealth playerHealth;
    private Rigidbody2D thisEnemyRB;

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

    private float timeBetweenShots; //
    public float startTimeBetweenShots; //
    public GameObject bulletPrefab; //

    private bool tooCloseToWall = false;

    private Vector2 point1;
    private Vector2 point2 = Vector2.zero;
    private bool atPoint1 = true;
    private bool foundPoint2 = false;
    private float spawnRange = 0.1f;

    private void Start()
    {
        timeBetweenShots = startTimeBetweenShots;
    }

    private void Awake()
    {
        thisEnemyRB = this.GetComponent<Rigidbody2D>();
        //allLines = new List<Enemy2_beam>();
        //Enemy2_beam newLine = Instantiate(linePrefab);
        //allLines.Add(newLine);
        //newLine.AssignTarget(transform.position, playerObject.transform);
        //newLine.gameObject.SetActive(false);
    }
    //void Shot()
    //{
    //    Draw2DRay(thisEnemyPosition, playerPosition);
    //}
    //void Draw2DRay(Vector2 startPos, Vector2 endPos)
    //{
    //lineRenderer.SetPosition(0, startPos);
    //lineRenderer.SetPosition(1, endPos);
    //}

    void Update()
    {
        FindPlayer();

        thisEnemyPosition = thisEnemyRB.position;
        playerPosition = playerRB.position;

        if (playerObject != null && enemyHealthScript.enemyHealth > 0)
        {
            MoveEnemy(playerPosition);
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

    /*private void Shot()
    {
        if(beamIsOn)
        {
            foreach (var line in allLines)
            {
                line.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var line in allLines)
            {
                line.gameObject.SetActive(true);
            }
        }
    }*/
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

        /*if (distance <= visionRange && distance > moveUntilDistance)
        {
            thisEnemyRB.position = Vector2.MoveTowards(thisEnemyPosition, targetPosition, moveVelocity * Time.deltaTime);


            //Shot();
            playerHealth.DamagePlayer(1, false);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            thisEnemyRB.velocity = Vector2.zero;



            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }*/

        Vector2 targetDirection = (targetPosition - thisEnemyPosition).normalized;

        if (!tooCloseToWall && IsInTheRoom(targetPosition))
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
    private Vector2 RandomPointInRoom2D()
    {
        return new Vector2(Random.Range(roomCenter.x + leftWall, roomCenter.x + rightWall), Random.Range(roomCenter.y + bottomWall, roomCenter.y + topWall));
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            playerHealth.DamagePlayer(1);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            playerHealth.DamagePlayer(1);
            thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        thisEnemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }*/
}
