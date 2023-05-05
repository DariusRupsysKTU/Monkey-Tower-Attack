using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, DataPersistence
{
    public int playerHealth;
    public int numOfHearts;
    private int prevHealth;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Image[] hearts;

    [SerializeField] private UnityEvent OnDie;

    [SerializeField] private float immuneTime;

    [Header("Sounds")]
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource hitSound;

    private bool IsImmune;
    private bool damagedByBlast;

    private GameObject healthCanvas;

    private SpriteRenderer playerSpriteRenderer;
    private Color startColor;

    private void Start() 
    {
        playerSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        startColor = playerSpriteRenderer.color;
        prevHealth = playerHealth;
        IsImmune = false;
        damagedByBlast = false;
        FindHealthCanvas();    
    }

    private void Update() 
    {
        if (playerHealth > numOfHearts)
        {
            playerHealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else 
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        } 

        if (playerHealth <= 0)
        {
            OnDie.Invoke();
            deathSound.Play();
        }

        if (playerHealth < prevHealth && (!IsImmune || damagedByBlast))
        {
            StartCoroutine(GetImmunity(immuneTime));
        }

        if (IsImmune)
        {
            playerSpriteRenderer.color = Color.red;
        }
        else
        {
            playerSpriteRenderer.color = startColor;
        }
    }

    private void FindHealthCanvas()
    {
        if (healthCanvas == null)
        {
            healthCanvas = GameObject.Find("HealthCanvas");
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i] = healthCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>();
            }
        }
    }

    private IEnumerator GetImmunity(float time)
    {
        IsImmune = false;
        IsImmune = true;
        yield return new WaitForSeconds(time);
        IsImmune = false;
        damagedByBlast = false;
        prevHealth = playerHealth;
    }

    public void GetImmunityPower(float time)
    {
        StartCoroutine(GetImmunity(time));
    }

    public void DamagePlayer(int amount, bool isBlastDamage) 
    {
        if (!IsImmune || isBlastDamage && !damagedByBlast)
        {
            playerHealth -= amount;
            
            if (isBlastDamage)
            {
                damagedByBlast = true;
            }

            hitSound.Play();
        }
    }

    public void HealPlayer(int amount) => playerHealth += amount;

    public void PlayerDies() => Destroy(this.gameObject);

    public void LoadData(GameData data)
    {
        this.playerHealth = data.playerHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.playerHealth = this.playerHealth;
    }
}
