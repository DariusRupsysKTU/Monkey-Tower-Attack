using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [SerializeField] float moveVelocity;
    [SerializeField] float moveUntilDistance;
    [SerializeField] float visionRange;

    [SerializeField] float playerTouchDelay;

    private GameObject playerObject;
    private Rigidbody2D playerRB;
    private Rigidbody2D thisBotRB;

    private Vector2 thisBotPosition;
    private Vector2 playerPosition;

    private bool touchedPlayer;
    private bool touchTimerOn;
    
    private void Awake() 
    {
        thisBotRB = this.GetComponent<Rigidbody2D>();

        touchedPlayer = false;
        touchTimerOn = false;
    }

    void Update()
    {
        FindPlayer();

        thisBotPosition = thisBotRB.position;
        playerPosition = playerRB.position;

        MoveBot(playerPosition);

        if (touchedPlayer)
        {
            Destroy(playerObject);
            Invoke(nameof(SetTouchedPlayerFalse), 0);
        }

        if (touchTimerOn)
        {
            Invoke(nameof(TouchTimerOff), playerTouchDelay);
        }
    }

    private void FindPlayer()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerObject.GetComponent<Rigidbody2D>();
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
            touchedPlayer = true;
            touchTimerOn = true;
        }
    }

    private void SetTouchedPlayerFalse()
    {
        touchedPlayer = false;
    }

    private void TouchTimerOff()
    {
        touchTimerOn = false;
    }

    // private void MoveBot(Vector2 targetPosition)
    // {
    //     Vector2 direction = GetDirection(thisBotPosition, targetPosition);
    //     float velocity;

    //     if (Mathf.Abs(Mathf.Abs(thisBotPosition.x) - Mathf.Abs(targetPosition.x)) > moveUntilDistance)
    //     {
    //         if (thisBotRB.velocity.y != 0)
    //             velocity = Mathf.Sqrt(moveVelocity * moveVelocity / 2);
    //         else
    //             velocity = moveVelocity;

    //         thisBotRB.velocity = new Vector2(direction.x * velocity, thisBotRB.velocity.y);
    //     }
    //     else 
    //     {
    //         thisBotRB.velocity = new Vector2(0, thisBotRB.velocity.y);
    //     }
    //     if (Mathf.Abs(Mathf.Abs(thisBotPosition.y) - Mathf.Abs(targetPosition.y)) > moveUntilDistance)
    //     {
    //         if (thisBotRB.velocity.x != 0)
    //             velocity = Mathf.Sqrt(moveVelocity * moveVelocity / 2);
    //         else
    //             velocity = moveVelocity;
    //         thisBotRB.velocity = new Vector2(thisBotRB.velocity.x, direction.y * velocity);
    //     }
    //     else
    //     {
    //         thisBotRB.velocity = new Vector2(thisBotRB.velocity.x, 0);
    //     }
    // }

    // private Vector2 GetDirection(Vector2 currentPosition, Vector2 targetPosition)
    // {
    //     float directionX;
    //     if (currentPosition.x - targetPosition.x < 0)
    //     {
    //         directionX = 1f;
    //     }
    //     else if (currentPosition.x - targetPosition.x > 0)
    //     {
    //         directionX = -1f;
    //     }
    //     else
    //     {
    //         directionX = 0f;
    //     }

    //     float directionY;
    //     if (currentPosition.y - targetPosition.y < 0)
    //     {
    //         directionY = 1f;
    //     }
    //     else if (currentPosition.y - targetPosition.y > 0)
    //     {
    //         directionY = -1f;
    //     }
    //     else
    //     {
    //         directionY = 0f;
    //     }

    //     return new Vector2(directionX, directionY);
    // }
}
