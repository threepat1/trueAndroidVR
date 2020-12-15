using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatewithCamera : MonoBehaviour
{

    private float speed = 10.0f;//a speed modifier
    public Transform cameraMain;

    void FixedUpdate()
    {//Set up things on the start method
        transform.rotation = cameraMain.rotation;
    }
}
