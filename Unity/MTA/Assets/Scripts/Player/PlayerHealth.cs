using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, DataPersistence
{
    public static PlayerHealth instance { get; private set; }

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
    private bool IsRoomImmune;

    private GameObject healthCanvas;

    private SpriteRenderer playerSpriteRenderer;
    private Color startColor;

    private bool immortalCheatOn = false;
    private GameObject pauseMenu;

    private void Start() 
    {
        playerSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        startColor = playerSpriteRenderer.color;
        prevHealth = playerHealth;
        IsImmune = false;
        IsRoomImmune = false;
        FindHealthCanvas();    
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }
    private void Update() 
    {
        // Debug.Log("Current: " + playerHealth + " Prev:" + prevHealth);

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

        if (playerHealth < prevHealth && !IsImmune)
        {
            StartCoroutine(GetImmunity(immuneTime, false));
        }

        if (IsImmune)
        {
            playerSpriteRenderer.color = Color.red;
        }
        else
        {
            playerSpriteRenderer.color = startColor;
        }

        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseCanvas");
        }

        if (pauseMenu != null && pauseMenu.GetComponent<PauseMenu>().cheatsOn)
        {
            ImmunityCheat();
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

    private IEnumerator GetImmunity(float time, bool roomImmune)
    {
        if (roomImmune)
        {
            IsRoomImmune = false;
            IsRoomImmune = true;
        }
        IsImmune = false;
        IsImmune = true;
        yield return new WaitForSeconds(time);
        IsImmune = false;
        if (roomImmune)
        {
            IsRoomImmune = false;
        }
        prevHealth = playerHealth;
    }

    public void GetImmunityPower(float time)
    {
        StartCoroutine(GetImmunity(time, true));
    }

    public void DamagePlayer(int amount, bool pierceImmunity) 
    {
        if (!IsImmune || (pierceImmunity && !IsRoomImmune))
        {
            playerHealth -= amount;

            hitSound.Play();
        }
    }

    public void HealPlayer(int amount) 
    {
        playerHealth += amount;
        prevHealth = playerHealth;
    } 
        

    public void PlayerDies() => Destroy(this.gameObject);

    public void LoadData(GameData data)
    {
        this.playerHealth = data.playerHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.playerHealth = this.playerHealth;
    }

    private void ImmunityCheat()
    {
        if (Input.GetKeyDown("i"))
        {
            if (!immortalCheatOn)
            {
                immortalCheatOn = true;
            }
            else
            {
                immortalCheatOn = false;
            }
        }

        if (immortalCheatOn)
        {
            playerHealth = 5;
            prevHealth = 5;
        }

        // Debug.Log(prevHealth + " " + playerHealth);
    }
}
