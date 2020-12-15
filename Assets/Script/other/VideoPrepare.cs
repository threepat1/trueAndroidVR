using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.XR;

public class VideoPrepare : MonoBehaviour
{
   
    private VideoPlayer video;
    // Start is called before the first frame update
    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Prepare();
        video.loopPointReached += CheckOver;
        //StartCoroutine(LoadVideoCoroutine("MyVideo"));

    }
    IEnumerator LoadVideoCoroutine(string videoName)
    {
        Debug.Log("videoName: " + videoName);
        string path = AndroidPathManager.GetFriendlyFilesPath() + "/" + videoName + ".mp4";
      
        Debug.Log("Path: " + path);
        video.url = path;
        
        while (!video.isPrepared)
        {
      
            if (!gameObject.activeInHierarchy)
                yield break;

            yield return null;
        }

        
    }

    // Update is called once per frame
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {

        StartCoroutine(SwitchTo2D());
        SceneManager.LoadScene(1);//the scene that you want to load after the video has ended.
    }

    IEnumerator SwitchTo2D()
    {
        // Empty string loads the "None" device.
        XRSettings.LoadDeviceByName("");

        // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
        yield return null;

        // Not needed, since loading the None (`""`) device takes care of this.
        // XRSettings.enabled = false;

        // Restore 2D camera settings.
        ResetCameras();
    }

    // Resets camera transform and settings on all enabled eye cameras.
    void ResetCameras()
    {
        // Camera looping logic copied from GvrEditorEmulator.cs
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {

                // Reset local position.
                // Only required if you change the camera's local position while in 2D mode.
                cam.transform.localPosition = Vector3.zero;

                // Reset local rotation.
                // Only required if you change the camera's local rotation while in 2D mode.
                cam.transform.localRotation = Quaternion.identity;

                // No longer needed, see issue github.com/googlevr/gvr-unity-sdk/issues/628.
                // cam.ResetAspect();

                // No need to reset `fieldOfView`, since it's reset automatically.
            }
        }
    }
}
