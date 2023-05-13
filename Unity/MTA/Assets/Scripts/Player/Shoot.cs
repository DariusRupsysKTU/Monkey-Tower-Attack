using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] KeyCode shootKey;
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

    public void DecreaseCooldown()
    {
        shootCooldown = shootCooldown - 0.2f;
    }
}
