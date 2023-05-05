using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public bool isPickUp;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private GameObject aboveTextPrefab;
    [SerializeField] ParticleSystem healVFX;
    [SerializeField] ParticleSystem fullHealVFX;
    [SerializeField] ParticleSystem speedBoostVFX;
    [SerializeField] ParticleSystem doubleDamageVFX;
    [SerializeField] ParticleSystem coinsVFX;

    private GameObject player;
    private GameObject aboveText;
    private AboveTextAnimations aboveTextAnimations;

    [Header("Ability")]
    public bool heal;
    public bool fullHeal;
    public bool doubleDamage;
    public bool speedBoost;
    public bool immunity;
    public bool granade;
    public bool coin;

    private bool inRange;

    void Start() 
    {
        if (!isPickUp)
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }    
        else
        {
            this.transform.tag = "Currency";
        }
    }

    void Update()
    {
        if (player != null)
        {
            aboveText.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z);
        }

        if(inRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                PickUp();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player" && isPickUp)
        {
            Physics2D.IgnoreCollision(other.collider, other.otherCollider, true);
        }
        if (((other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Box") && !isPickUp)
        || ((other.gameObject.tag == "Currency" || other.gameObject.tag == "WallCollider" || other.gameObject.name.Contains("Doors")) && isPickUp))
        {
            Physics2D.IgnoreCollision(other.collider, other.otherCollider, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPickUp)
        {
            player = collision.gameObject;
            AddAboveText();
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPickUp)
        {
            player = null;
            RemoveAboveText();
            inRange = false;
        }
    }

    private void AddAboveText()
    {
        aboveText = Instantiate(aboveTextPrefab, this.transform.position, this.transform.rotation);
        aboveText.transform.localScale = new Vector3(0f, 0f, 0f);
        aboveTextAnimations = aboveText.GetComponent<AboveTextAnimations>();
        aboveTextAnimations.PlayFadeInAnimation();
    }

    private void RemoveAboveText()
    {
        aboveTextAnimations.PlayFadeOutAnimation();
        Destroy(aboveText.gameObject, 1f);
    }

    public void PickUp()
    {
        if (isPickUp)
        {
            ActivatePower();
            Destroy(this.gameObject);
        }
    }

    private void ActivatePower()
    {
        if (heal)
        {
            player.GetComponent<PlayerHealth>().HealPlayer(1);
            PlayHealVFX();
        }
        else if (fullHeal)
        {
            player.GetComponent<PlayerHealth>().HealPlayer(5);
            PlayFullHealVFX();
        }
        else if (doubleDamage)
        {
            if (player.GetComponent<Shoot>().enabled)
            {
                PlayerBullet bulletScript = player.GetComponent<Shoot>().bulletPrefab.GetComponent<PlayerBullet>();
                bulletScript.EnableDoubleDamage(5f);
                StartCoroutine(DisableDamage(bulletScript, 5f));
                PlayDoubleDamageVFX();
            }
        }
        else if (speedBoost)
        {
            float speedBoostDuration = 5f;
            player.GetComponent<PlayerMovement>().EnableSpeedBoost(speedBoostDuration);
            PlaySpeedBoostVFX(speedBoostDuration);
        }
        else if (immunity)
        {
            player.GetComponent<PlayerHealth>().GetImmunityPower(3f);
        }
        else if (coin)
        {
            GetComponent<CoinManager>().UpdateCoins();
            PlayCoinVFX();
        }
    }

    IEnumerator DisableDamage(PlayerBullet bulletScript, float time)
    {
        yield return new WaitForSeconds(time);
        bulletScript.DisableExtraDamage();
    }

    private void PlayHealVFX()
    {
        ParticleSystem vfx = Instantiate(healVFX, player.transform.position, Quaternion.identity);
        vfx.transform.parent = player.transform;
        vfx.Play();
        Destroy(vfx.gameObject, healVFX.main.duration);
    }

    private void PlayFullHealVFX()
    {
        ParticleSystem vfx = Instantiate(fullHealVFX, player.transform.position, Quaternion.identity);
        vfx.transform.parent = player.transform;
        vfx.Play();
        Destroy(vfx.gameObject, fullHealVFX.main.duration);
    }

    private void PlaySpeedBoostVFX(float duration)
    {
        float heightCorrection = 0.05f;
        ParticleSystem vfx = Instantiate(speedBoostVFX, new Vector2(player.transform.position.x, player.transform.position.y - heightCorrection), Quaternion.identity);
        vfx.transform.parent = player.transform;
        var main = vfx.main;
        main.duration = duration;
        vfx.Play();
        Destroy(vfx.gameObject, speedBoostVFX.main.duration + 1);
    }

    private void PlayDoubleDamageVFX()
    {
        ParticleSystem vfx = Instantiate(doubleDamageVFX, player.transform.position, Quaternion.identity);
        vfx.transform.parent = player.transform;
        vfx.Play();
        Destroy(vfx.gameObject, doubleDamageVFX.main.duration + 1);
    }

    private void PlayCoinVFX()
    {
        ParticleSystem vfx = Instantiate(coinsVFX, player.transform.position, Quaternion.Euler(-90f,0f,0f));
        vfx.transform.parent = player.transform;
        vfx.Play();
        Destroy(vfx.gameObject, coinsVFX.main.duration);
    }
}
