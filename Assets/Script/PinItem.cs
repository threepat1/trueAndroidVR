using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PinItem : MonoBehaviour
{
    [SerializeField] private Image pinItem, pinEmptyItem;
    [SerializeField] private TextMeshProUGUI pinNumber;
    public string pin;
    
    void Start()
    {
        pin = "";
        DisplayValue();
    }

    public void DisplayValue()
    {
        bool hasValue = pin != "";
        pinNumber.text = hasValue ? pin : "-";
        if (SigninManager.Instance.showPin)
        {
            pinNumber.gameObject.SetActive(true);
            pinEmptyItem.gameObject.SetActive(false);
            pinItem.gameObject.SetActive(false);
        }
        else
        {
            pinNumber.gameObject.SetActive(false);
            pinEmptyItem.gameObject.SetActive(!hasValue);
            pinItem.gameObject.SetActive(hasValue);
        }
    }

    public void SetValue(string v)
    {
        pin = v;
        DisplayValue();
    }
}
