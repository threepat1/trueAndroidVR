using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButton : MonoBehaviour
{

    public string URL;
    // Start is called before the first frame update
   public void TrueIDURL()
    {
        Application.OpenURL(URL);
    }
}
