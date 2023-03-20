using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public const float moveSpeed = 2f;
    public Vector2 PlayerInput;
    public Vector2 forceToApply;
    public const float forceDamping = 1.2f;
    public GameObject firePoint;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        firePoint = playerRB.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        if (firePoint != null)
        {
            ChangeFirePointRotation();
        }

        Vector2 moveForce = PlayerInput * moveSpeed;
        moveForce += forceToApply;
        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
        {
            forceToApply = Vector2.zero;
        }
        playerRB.velocity = moveForce;
    }

    private void ChangeFirePointRotation()
    {
        if (PlayerInput.x == 1)
        {
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 270f);
        }
        else if (PlayerInput.x == -1)
        {
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 90f);
        }
        else if (PlayerInput.y == 1)
        {
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (PlayerInput.y == -1)
        {
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            forceToApply += new Vector2(-20, 0);
            Destroy(collision.gameObject);
        }
    }
}
