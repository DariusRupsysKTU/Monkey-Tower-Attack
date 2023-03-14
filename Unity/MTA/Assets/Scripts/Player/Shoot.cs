using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject bulletStart;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDestroyTime;

    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = bulletStart.transform.position;
        bullet.transform.eulerAngles = new Vector3(0f, 0f, 90f + player.transform.eulerAngles.z);
        bullet.GetComponent<Rigidbody2D>().velocity = FireDirection() * bulletSpeed;
        Destroy(bullet, bulletDestroyTime);
    }

    private Vector2 FireDirection()
    {
        float rotationZ = player.transform.eulerAngles.z;
        int x = 0;
        int y = 0;

        if (rotationZ == 270f)
        {
            x = 1;
        }
        else if (rotationZ == 90f)
        {
            x = -1;
        }
        else if (rotationZ == 0f)
        {
            y = 1;
        }
        else 
        {
            y = -1;
        }

        return new Vector2(x, y);
    }
}
