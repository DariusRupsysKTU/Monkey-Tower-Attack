using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour, DataPersistence
{
    public KeyCode punchKey;
    public Animator animator;
    [SerializeField] private AudioSource whiffSound;
    [SerializeField] private AudioSource punchSound;
    [SerializeField] private AudioSource parrySound;
    public float attackRange = 0.11f;
    public int damage = 2;

    public Transform LeftPuchPoint;
    public Transform RightPuchPoint;
    public Transform UpPuchPoint;
    public Transform DownPuchPoint;
    private Transform LastPP;
    public GameObject bulletPrefab;
    public ParticleSystem lightningVFX;
    public ParticleSystem torpedoVFX;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(punchKey))
        {
            Punching();
        }
    }

    public void IncreaseDamage()
    {
        damage++;
    }

    public void EnableDoubleDamage(float time)
    {
        damage = damage * 2;
        Invoke(nameof(DisableDoubleDamage), time);
    }

    private void DisableDoubleDamage()
    {
        damage = damage / 2;
    }

    public void DisableExtraDamage()
    {
        damage = 1;
    }

    void Punching()
    {
        animator.SetTrigger("sp_" + PlayerMovement.lastDir);
        animator.SetTrigger("attack");
    }
    void PunchDown()
    {
        LastPP = DownPuchPoint;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(DownPuchPoint.position, attackRange);
        foreach(Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void PunchUp()
    {
        LastPP = UpPuchPoint;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(UpPuchPoint.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void PunchLeft()
    {
        LastPP = LeftPuchPoint;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(LeftPuchPoint.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void PunchRight()
    {
        LastPP = RightPuchPoint;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(RightPuchPoint.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    void DealDamage(Collider2D other)
    {
        if (other.transform.tag == "EnemyBullet")
        {
            Parry(other);
            return;
        }
        else
        {

            EnemyHealth enemyHealthScript = other.GetComponent<EnemyHealth>();
            if (enemyHealthScript != null)
            {
                enemyHealthScript.DamageEnemy(damage);
                punchSound.Play();
                return;
            }

            ItemHealth itemHealthScript = other.GetComponent<ItemHealth>();
            if (itemHealthScript != null)
            {
                itemHealthScript.DamageItem(damage);
                punchSound.Play();
                return;
            }
            whiffSound.Play();
        }
    }
    void Parry(Collider2D other)
    {
        parrySound.Play();
        EnemyBullet Enemy3BulletScript = other.GetComponent<EnemyBullet>();
        // Quaternion rotation = Quaternion.Euler(LastPP.transform.rotation.x, LastPP.transform.rotation.y, LastPP.transform.rotation.z - 90f);
        GameObject parriedBullet = Instantiate(bulletPrefab, LastPP.transform.position, LastPP.transform.rotation);
        parriedBullet.GetComponent<SpriteRenderer>().sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;
        parriedBullet.GetComponent<PlayerBullet>().parried = true;

        if (Enemy3BulletScript.rockSplashVFX.name.Contains("Lightning"))
        {
            parriedBullet.GetComponent<PlayerBullet>().splashVFX = lightningVFX;
        }
        else
        {
            parriedBullet.GetComponent<PlayerBullet>().splashVFX = torpedoVFX;
        }

        Enemy3BulletScript.DestroyBullet(0f);
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

    public void LoadData(GameData data)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player.transform.name.Contains("2"))
        {
            this.damage = data.punchDamage;
        }
    }

    public void SaveData(ref GameData data)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.transform.name.Contains("2"))
        {
            data.punchDamage = this.damage;
        }
    }
}
