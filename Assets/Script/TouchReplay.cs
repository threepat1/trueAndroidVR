using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;


public class TouchReplay : MonoBehaviour
{
    public VideoPlayer nonVRVideo;
 
    void Update()
    {
        // Check if there is a touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if finger is over a UI element 
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                StartCoroutine(Replay());
        
            }
            
        }
    }

    IEnumerator Replay()
    {
        nonVRVideo.Stop();
        yield return new WaitForSeconds(0.3f);
        nonVRVideo.Play();
    }

}
