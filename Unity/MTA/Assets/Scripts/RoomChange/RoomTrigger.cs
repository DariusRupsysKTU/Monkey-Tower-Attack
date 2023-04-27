using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Vector3 cameraChange;
    public Vector3 playerChange;

    private Camera cam;
    private CameraPosition cameraPositionScript;
    private Vector3 curCamPos;
    private Vector3 newCamPos;
    // private float cameraMoveSpeed;
    // private float timeBetweenCamPosUpdates;

    void Start()
    {
        cam = Camera.main;
        cameraPositionScript = cam.GetComponent<CameraPosition>();
        newCamPos = cameraPositionScript.calculatedCameraPosition;
        // cameraMoveSpeed = cameraPositionScript.cameraMoveSpeed;
        // timeBetweenCamPosUpdates = cameraPositionScript.timeBetweenCamPosUpdates;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            TransitionRoom(other);
        }    
    }

    void Update() 
    {
        curCamPos = cameraPositionScript.calculatedCameraPosition;

        // if (newCamPos != Vector3.zero)
        // {
        //     // Debug.Log(newCamPos);
        //     // cam.transform.position = Vector3.Lerp(cam.transform.position, newCamPos, cameraMoveSpeed);
        //     Invoke(nameof(NewCamPosZero), timeBetweenCamPosUpdates);
        // }
    }

    private void TransitionRoom(Collider2D other)
    {    
        newCamPos = curCamPos + cameraChange;
        cameraPositionScript.calculatedCameraPosition = newCamPos;
        other.transform.position += playerChange;
    }

    // private void NewCamPosZero()
    // {
    //     newCamPos = Vector3.zero;
    // }
}
