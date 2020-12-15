using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject panel, panel1, webView;
    [SerializeField] Text videoTitle;
    [SerializeField] Text videoDescription;

    public string nextScene;

    private void Start()
    {

    }
    public void Cancel()
    {
        panel.SetActive(false);
        panel1.SetActive(false);
        webView.SetActive(false); 

    }
    public void GoToVideo1()
    {
       
        SceneManager.LoadScene(nextScene);
    }
 

}
