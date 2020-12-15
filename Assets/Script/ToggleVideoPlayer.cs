using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;


public class ToggleVideoPlayer : MonoBehaviour
{
    private VideoPlayer nonVRVideo;
    public Sprite playImage, pauseImage;
    public Button toggle;

    private void Start()
    {
        nonVRVideo = GetComponent<VideoPlayer>();
    }

    void FixedUpdate()
    {
        // Check if there is a touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if finger is over a UI element 
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (nonVRVideo.isPlaying)
                {

                    nonVRVideo.Pause();
                    toggle.GetComponent<Image>().sprite = pauseImage;
                  
                }
                else
                {
                    nonVRVideo.Play();
                    toggle.GetComponent<Image>().sprite = playImage;

                }
            }
           
        }
    }
}
