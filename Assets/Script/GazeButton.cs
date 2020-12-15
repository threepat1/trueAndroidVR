using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class GazeButton : MonoBehaviour
{
    public Image imageButton;
    public float gazeTime = 2;
    bool gvrStatus;
    float timer =0f;

 

    // Start is called before the first frame update
    void Start()
    {
        imageButton = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gvrStatus)
        {
            timer += Time.deltaTime;
            imageButton.fillAmount = timer / gazeTime;

            if(timer >= gazeTime)
            {
                timer = 0f;
                ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);

                
            }
           

        }
       
        
    }
    public void PointerEnter()
    {
        gvrStatus = true;
        Debug.Log("PointerEnter");
    }
    public void PointerExit()
    {
        gvrStatus = false;
        timer = 0;
        imageButton.fillAmount = 0;
        Debug.Log("PointerExit");
    }
    public void PointerDown()
    {
        Debug.Log("Do something");
    }
}
