using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Animator anim;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().position;
        float distance = Vector2.Distance(transform.position, pos);
        if(distance < 0.65)
        {
            //Invoke(nameof(OpenAnimation), 0.05f);
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("doors_idle"))
                anim.SetTrigger("open_door");
        }
        if (distance >= 0.65)
        {
            //Invoke(nameof(CloseAnimation), 0.05f);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("opened_doors_idle"))
                anim.SetTrigger("close_door");
        }
    }
    private void OpenAnimation()
    {
        anim.SetTrigger("open_door");
    }
    private void CloseAnimation()
    {
        anim.SetTrigger("close_door");
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("open_door");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetTrigger("close_door");
    }*/
}
