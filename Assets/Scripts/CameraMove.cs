using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Transform cam;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        cam = transform.GetChild(0);
    }
    void Update()
    {
        if(target != null)
        {
            cam.transform.position = target.position + offset;
            if (Input.GetMouseButton(0))
            {

            }
        }
    }
}
