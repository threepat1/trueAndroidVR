using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LoadVideoStream : MonoBehaviour
{
    public string urlName;
    public string videoName;
    IEnumerator Start()
    {
        string url = urlName;

        string vidSavePath = AndroidPathManager.GetFriendlyFilesPath() + "/" + videoName + ".mp4";
        print(vidSavePath);
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(vidSavePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(vidSavePath));
        }

        var uwr = new UnityWebRequest(url);
        uwr.method = UnityWebRequest.kHttpVerbGET;
        var dh = new UnityEngine.Networking.DownloadHandlerFile(vidSavePath);
        dh.removeFileOnAbort = true;
        uwr.downloadHandler = dh;
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
         
        }
        else 
        {
            Debug.Log("Download saved to: " + vidSavePath.Replace("/", "\\") + "\r\n" + uwr.error);
            File.WriteAllBytes(vidSavePath, uwr.downloadHandler.data);
        }
           

    }
}
