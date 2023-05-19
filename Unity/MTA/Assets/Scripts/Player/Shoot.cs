using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shoot : MonoBehaviour, DataPersistence
{
    public KeyCode shootKey;
    [SerializeField] GameObject firePoint;
    public GameObject bulletPrefab;
    [SerializeField] int bulletCount;
    public float shootCooldown;
    private float nextShot = 0f;

    [SerializeField] private AudioSource shootSound;


    void Update()
    {
        // Debug.Log(nextShot + " " + Time.time);
        if (Input.GetKeyDown(shootKey) && Time.time >= nextShot)
        {
            nextShot = Time.time + shootCooldown;
            FireBullet();
        }

        if (shootCooldown <= 0)
        {
            shootCooldown = 0;
        }
        else
        {
            shootCooldown = Mathf.Round(shootCooldown*10) * 0.1f;
        }
    }

    private void FireBullet()
    {
        for (int j = 0; j < bulletCount; j++)
        {
            Quaternion angle = firePoint.transform.rotation * Quaternion.Euler(0f, 0f, 360 / bulletCount * j);
            Instantiate(bulletPrefab, firePoint.transform.position, angle);
        }        
        
        shootSound.Play();            
    }

    public void DecreaseCooldown(float amount)
    {
        shootCooldown -= amount;
    }

    public void LoadData(GameData data)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<Shoot>().bulletPrefab.name.Contains("Fireball"))
        {
            this.shootCooldown = data.cooldown;
        }
    }

    public void SaveData(ref GameData data)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<Shoot>().bulletPrefab.name.Contains("Fireball"))
        {
            data.cooldown = this.shootCooldown;
        }
    }
}
