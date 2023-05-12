using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cast : MonoBehaviour
{
    [SerializeField] GameObject firePoint;
    public GameObject bulletPrefab;
    public float shootCooldown;
    private float nextShot = 0f;

    [SerializeField] private AudioSource shootSound;

    void Update()
    {
        // Debug.Log(nextShot + " " + Time.time);
        if (Input.GetKeyDown("h") && Time.time >= nextShot)
        {
            nextShot = Time.time + shootCooldown;
            FireBullet();
        }
    }

    private void FireBullet()
    {
        //Quaternion angle = new Quaternion();
        //angle.Euler(1f, 2f, 2f);
        //for (int j = 0; j < 20; j++)
        //{
        //Quaternion angle = firePoint.transform.rotation * (new Quaternion().SetEulerRotation(1f, 2f, 0f));
        //Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        //}
        Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        shootSound.Play();
    }

    public void DecreaseCooldown()
    {
        shootCooldown = shootCooldown - 0.2f;
    }
}
