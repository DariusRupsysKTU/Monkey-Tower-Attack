using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject firePoint;
    public GameObject bulletPrefab;
    [SerializeField] public float cooldownTime;

    private float nextFireTime = 0;

    [SerializeField] private AudioSource shootSound;


    void Update()
    {
        if (Time.time > nextFireTime)
        {
            if (Input.GetKeyDown("g"))
            {
                this.nextFireTime = Time.time + cooldownTime;
                FireBullet();
            }
        }
    }

    private void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        shootSound.Play();
    }
}
