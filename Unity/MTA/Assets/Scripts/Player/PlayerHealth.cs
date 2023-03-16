using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int playerHealth;
    public int numOfHearts;
    private int prevHealth;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Image[] hearts;

    [SerializeField] private UnityEvent OnDie;

    [SerializeField] private float ImmuneTime;
    private bool IsImmune;

    private GameObject healthCanvas;

    private SpriteRenderer playerSpriteRenderer;
    private Color startColor;

    private void Start() 
    {
        playerSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        startColor = playerSpriteRenderer.color;
        prevHealth = playerHealth;
        IsImmune = false;
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
        }

        if (playerHealth < prevHealth && !IsImmune)
        {
            StartCoroutine(GetImmunity());
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

    private IEnumerator GetImmunity()
    {
        IsImmune = true;
        yield return new WaitForSeconds(ImmuneTime);
        IsImmune = false;
        prevHealth = playerHealth;
    }

    public void DamagePlayer(int amount) 
    {
        if (!IsImmune)
        {
            playerHealth -= amount;
        }
    }

    public void HealPlayer(int amount) => playerHealth += amount;

    public void PlayerDies() => Destroy(this.gameObject);
}
