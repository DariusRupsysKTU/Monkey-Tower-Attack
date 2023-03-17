using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject firePoint;
    [SerializeField] GameObject bulletPrefab;

    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
    }
}
