
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BackHandler : MonoBehaviour
{
    //private void Awake()
    //{
    //    Input.backButtonLeavesApp = true;
    //}


    // Use this for initialization
    void Start()
    {
        Debug.Log("Starting up BackHandler");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Goodbye cruel world!");
            XRSettings.enabled = false;
        }
    }
}
 

