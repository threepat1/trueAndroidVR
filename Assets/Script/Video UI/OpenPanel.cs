using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanel : MonoBehaviour
{
    public Button yourButton;
    public GameObject panel;
    public CanvasGroup canvasGroup;


    public float sec;

    public float fadeSpeed = 2;
    public float autoCloseTimer = 0;
    public float autoCloseInterval = 5f;

    private bool inTransition = false;

    private void Start()
    {

        canvasGroup = panel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

    }

    private void Update()
    {
        if (canvasGroup.alpha == 1)
        {
            autoCloseTimer += Time.deltaTime;
            if (autoCloseTimer >= autoCloseInterval)
            {
                canvasGroup.alpha = 0.99f;
                StartCoroutine(Close());
                //ResetAutoCloseTimer();
            }
        }
    }



    public void Paneltemp()
    {
        Debug.Log("sss");

        if (inTransition)
        {
            Debug.Log("Not");
            return;
        }

        inTransition = true;
        if (canvasGroup.alpha == 0)
        {
            StartCoroutine(Open());
        }
        else
        {
            
            StartCoroutine(Close());
        }
        //if(canvasGroup.alpha == 0)
        //{
        //    StartCoroutine(TempPanel());
        //}
        //else if (canvasGroup.alpha > 0)
        //{
        //    canvasGroup.alpha = 0;
        //    StopCoroutine(TempPanel());
        //    sec = 5f;
        //}
    }

    IEnumerator Close()
    {
        Debug.Log("Closing");
        ResetAutoCloseTimer();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
           
        }
        inTransition = false;
    }

    IEnumerator Open()
    {
        ResetAutoCloseTimer();
        Debug.Log("Opening");
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        inTransition = false;
    }

    void ResetAutoCloseTimer()
    {
        autoCloseTimer = 0.00f;
    }
}

