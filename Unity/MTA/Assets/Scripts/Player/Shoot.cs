using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject firePoint;
    public GameObject bulletPrefab;
    public float shootCooldown;
    private float nextShot = 0f;

    [SerializeField] private AudioSource shootSound;


    void Update()
    {
        // Debug.Log(nextShot + " " + Time.time);
        if (Input.GetKeyDown("g") && Time.time >= nextShot)
        {
            nextShot = Time.time + shootCooldown;
            FireBullet();
        }
    }

    private void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        shootSound.Play();            
    }

    public void DecreaseCooldown()
    {
        shootCooldown = shootCooldown - 0.2f;
    }
}
