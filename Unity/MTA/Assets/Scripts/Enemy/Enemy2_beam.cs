using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_beam : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField]
    private Texture[] textures;
    private int animationStep;
    [SerializeField]
    private float fps = 30f;
    private float fpsCounter;
    private Transform target;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void AssignTarget(Vector3 startPosition, Transform newTarget)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        target = newTarget;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(1, target.position);

        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            animationStep++;
            if (animationStep == textures.Length)
                animationStep = 0;
            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);
            fpsCounter = 0f;
        }
    }
}
