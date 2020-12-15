using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Analytics;

public class FlexibleUIbutton : FlexibleUI
{
    Button button;
    Image image;
    Text text;

    public GameObject panel,panel1;
    public Text title;
    public Text description;

    protected override void OnSkinUI()
    {

        base.OnSkinUI();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        


        button.targetGraphic = image;
        image.sprite = skinData.videoImage;


        text.text = skinData.title;

  
    }

    public void PopUp()
    {

        if (string.IsNullOrEmpty(skinData.pathLink))
        {
            button.enabled = false;
        }
        else
        {
            PlayerPrefs.SetString("path", skinData.pathLink);
            panel.SetActive(true);
        }
        
        
        title.text = skinData.title;
        description.text = skinData.description;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSelectItem,
            new Parameter(FirebaseAnalytics.ParameterItemName, skinData.title));

    }

}
