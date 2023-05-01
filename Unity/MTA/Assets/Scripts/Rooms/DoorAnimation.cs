using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    private DoorCollider doorColliderScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        doorColliderScript = GetComponent<DoorCollider>();
    }

    void Update()
    {
        // Vector2 pos = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().position;
        // float distance = Vector2.Distance(transform.position, pos);
        
        // if(distance < 0.65)
        // {
        //     OpedDoorsAnimation();
        // }
        // if (distance >= 0.65)
        // {
        //     CloseDoorsAnimation();
        // }

        if (doorColliderScript.doorsOpen)
        {
            OpedDoorsAnimation();
        }
        else
        {
            CloseDoorsAnimation();
        }
    }

    private void OpedDoorsAnimation()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("doors_idle"))
            anim.SetTrigger("open_door");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("side_doors_idle"))
            anim.SetTrigger("open_door");
    }

    private void CloseDoorsAnimation()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("opened_doors_idle"))
            anim.SetTrigger("close_door");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("opened_side_doors_idle"))
            anim.SetTrigger("close_door");
    }
}
