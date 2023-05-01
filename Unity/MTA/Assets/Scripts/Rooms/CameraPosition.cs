using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    private Camera cam;

    public Vector3 calculatedCameraPosition;
    [Range(0.01f, 1)]
    public float cameraMoveSpeed;

    private float timeBetweenCamPosCheck;
    public float startTimeBetweenCamPosCheck; 

    private void Awake() 
    {
        cam = Camera.main;

        calculatedCameraPosition = this.transform.position;  

        timeBetweenCamPosCheck = startTimeBetweenCamPosCheck;  
    }

    private void Update() 
    {
        AdjustCameraPosition();
    }

    private void AdjustCameraPosition()
    {
        float xDiff = Mathf.Abs(cam.transform.position.x - calculatedCameraPosition.x);
        float yDiff = Mathf.Abs(cam.transform.position.y - calculatedCameraPosition.y);
        float zDiff = Mathf.Abs(cam.transform.position.z - calculatedCameraPosition.z);
        
        float errorValue = 0.01f;
        // Debug.Log(this.transform.position + " " + cam.transform.localPosition + "   " + calculatedCameraPosition);
        // Debug.Log(xDiff + " " + yDiff + " " + zDiff);

        if (xDiff >= errorValue || yDiff >= errorValue || zDiff >= errorValue)
        {
            if (timeBetweenCamPosCheck <= 0)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, calculatedCameraPosition, cameraMoveSpeed);
                timeBetweenCamPosCheck = startTimeBetweenCamPosCheck;
            }
            else
            {
                timeBetweenCamPosCheck -= Time.deltaTime;
            }
        }
    }
}
