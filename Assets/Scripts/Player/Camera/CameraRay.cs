using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    public Camera MainCamera;
    public RaycastHit hit;
    public float distance = 4f;
    public LookDiff LD;
    private void Awake()
    {
        LD = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LookDiff>();
        MainCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
    }
    void FixedUpdate()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        if(Physics.Raycast(MainCamera.transform.position,transform.TransformDirection(Vector3.forward), out hit,distance, layerMask))
        {
            LD.TakeAndDiffItem(hit.transform.tag, hit);
        }
    }

}
