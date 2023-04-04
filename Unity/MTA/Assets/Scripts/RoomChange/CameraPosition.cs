using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Vector3 currentCameraPosition;
    [Range(0.05f, 1)]
    public float cameraMoveSpeed;
    public float maxTimeForTransition;

    private void Awake() 
    {
        currentCameraPosition = this.transform.position;    
    }
}
