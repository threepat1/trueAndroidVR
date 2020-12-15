using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DownloadHandlerFile : DownloadHandlerScript
{

    // Standard scripted download handler - allocates memory on each ReceiveData callback

    public DownloadHandlerFile() : base()
    {
    }

    // Pre-allocated scripted download handler
    // reuses the supplied byte array to deliver data.
    // Eliminates memory allocation.

    public DownloadHandlerFile(byte[] buffer) : base(buffer)
    {
    }

    // Required by DownloadHandler base class. Called when you address the 'bytes' property.

    protected override byte[] GetData() { return null; }

    // Called once per frame when data has been received from the network.

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || data.Length < 1)
        {
            Debug.Log("LoggingDownloadHandler :: ReceiveData - received a null/empty buffer");
            return false;
        }

        Debug.Log(string.Format("LoggingDownloadHandler :: ReceiveData - received {0} bytes", dataLength));
        return true;
    }

    // Called when all data has been received from the server and delivered via ReceiveData.

    protected override void CompleteContent()
    {

        Debug.Log("LoggingDownloadHandler :: CompleteContent - DOWNLOAD COMPLETE!");
    }



}
