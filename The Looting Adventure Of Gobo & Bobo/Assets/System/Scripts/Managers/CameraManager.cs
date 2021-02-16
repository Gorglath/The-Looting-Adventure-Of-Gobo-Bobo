using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [Header("References")]
    [SerializeField]
    private Transform goboTransform = null;
    [Header("Variables")]
    [SerializeField]
    private Vector3 offsetFromGobo = Vector3.zero;
    [SerializeField]
    private float cameraMovementSmooth = 1f;
    private void OnDestroy()
    {
        _instance = null;
    }
    //helpers
    private Transform cacheTransform = null;
    void Start()
    {
        cacheTransform = transform;
    }
    void LateUpdate()
    {
        cacheTransform.position = Vector3.Lerp(cacheTransform.position, goboTransform.position + offsetFromGobo, Time.deltaTime * cameraMovementSmooth);   
    }
}
