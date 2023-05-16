using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, DataPersistence
{
    public Rigidbody2D playerRB;
    public float moveSpeed = 1f;
    public Vector2 PlayerInput;
    public Vector2 forceToApply;
    public const float forceDamping = 1.2f;
    public GameObject firePoint;
    private Animator anim;
    public static string lastDir = "down";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        firePoint = playerRB.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     Debug.Log(other.transform.name + "Parent: " + other.transform.parent.name);
    // }

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

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void EnableSpeedBoost(float time)
    {
        moveSpeed = moveSpeed * 2;
        Invoke(nameof(DisableSpeedBoost), time);
    }

    private void DisableSpeedBoost()
    {
        moveSpeed = moveSpeed / 2;
    }

    private void ChangeFirePointRotation()
    {
        if (PlayerInput.x > 0)
        {
            anim.SetTrigger("right");
            firePoint.transform.localPosition = new Vector3(0.05f, -0.03f, 0f);
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 270f);
            lastDir = "right";
        }
        else if (PlayerInput.x < 0)
        {
            anim.SetTrigger("left");
            firePoint.transform.localPosition = new Vector3(-0.05f, -0.03f, 0f);
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            lastDir = "left";
        }
        else if (PlayerInput.y == 1 )
        {
            anim.SetTrigger("up");
            firePoint.transform.localPosition = new Vector3(0f, 0.08f, 0f);
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            lastDir = "up";
        }
        else if (PlayerInput.y == -1)
        {
            anim.SetTrigger("down");
            firePoint.transform.localPosition = new Vector3(0f, -0.06f, 0f);
            firePoint.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            lastDir = "down";
        }
        else
        {
            anim.SetTrigger("idle_" + lastDir);
        }
    }

    public void LoadData(GameData data)
    {
        this.moveSpeed = data.movementSpeed;
    }

    public void SaveData(ref GameData data)
    {
        data.movementSpeed = this.moveSpeed;
    }
}
