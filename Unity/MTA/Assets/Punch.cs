using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private AudioSource punchSound;
    public float attackRange = 0.1f;
    public LayerMask enemyLayers;
    public int damage = 2;

    public Transform LeftPuchPoint;
    public Transform RightPuchPoint;
    public Transform UpPuchPoint;
    public Transform DownPuchPoint;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            Punching();
        }
    }

    void Punching()
    {
        animator.SetTrigger("sp_" + PlayerMovement.lastDir);
        animator.SetTrigger("attack");
        punchSound.Play();
    }
    void PunchDown()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(DownPuchPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void PunchUp()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(UpPuchPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void PunchLeft()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(LeftPuchPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void PunchRight()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(RightPuchPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void DealDamage(Collider2D other)
    {
        bool enemyIsDead = false;

        EnemyHealth enemyHealthScript = other.GetComponent<EnemyHealth>();
        if (enemyHealthScript != null)
        {
            if (enemyHealthScript.enemyHealth > 0)
            {
                enemyHealthScript.DamageEnemy(damage);
            }
            else
            {
                enemyIsDead = true;
            }
        }

        ItemHealth itemHealthScript = other.GetComponent<ItemHealth>();
        if (itemHealthScript != null)
        {
            itemHealthScript.DamageItem(damage);
        }
    }
    void OnDrawGizmosSelected()
    {
        if(LeftPuchPoint == null || RightPuchPoint == null || UpPuchPoint == null || DownPuchPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(LeftPuchPoint.position, attackRange);
        Gizmos.DrawWireSphere(RightPuchPoint.position, attackRange);
        Gizmos.DrawWireSphere(UpPuchPoint.position, attackRange);
        Gizmos.DrawWireSphere(DownPuchPoint.position, attackRange);
    }
}
