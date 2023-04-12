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

        if (distance <= visionRange && distance > moveUntilDistance)
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
        }
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
