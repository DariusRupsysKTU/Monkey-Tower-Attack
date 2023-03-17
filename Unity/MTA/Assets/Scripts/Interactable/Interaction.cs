using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [SerializeField] private bool isPickUp;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private GameObject aboveTextPrefab;

    private GameObject player;
    private GameObject aboveText;
    private AboveTextAnimations aboveTextAnimations;

    public UnityEvent interactAction;

    private bool inRange;

    void Update()
    {
        if (player != null)
        {
            aboveText.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z);
        }

        if(inRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPickUp)
        {
            player = collision.gameObject;
            AddAboveText();
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPickUp)
        {
            player = null;
            RemoveAboveText();
            inRange = false;
        }
    }

    private void AddAboveText()
    {
        aboveText = Instantiate(aboveTextPrefab, this.transform.position, this.transform.rotation);
        aboveText.transform.localScale = new Vector3(0f, 0f, 0f);
        aboveTextAnimations = aboveText.GetComponent<AboveTextAnimations>();
        aboveTextAnimations.PlayFadeInAnimation();
    }

    private void RemoveAboveText()
    {
        aboveTextAnimations.PlayFadeOutAnimation();
        Destroy(aboveText.gameObject, 1f);
    }

    public void PickUp()
    {
        if (isPickUp)
        {
            Destroy(this.gameObject);
        }
    }
}
