using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SigninManager : MonoBehaviour
{
    public static SigninManager Instance;
    public bool showPin = false;

    [SerializeField] private Toggle showPinToggle;

    [SerializeField] private List<PinItem> pinItems;

    [SerializeField] private Sprite showPinIcon, hidePinIcon;
    [SerializeField] private Image showPinToggleBg;

    private string[] pin;
    private int pinMaxLength;

    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private GameObject signinPanel;

    private SiginResult siginResult;
    [SerializeField] private TextMeshProUGUI siginResultText;

    [SerializeField] private List<Button> signinBtn;

    private string verifyPinUrl {
        get {
            return "http://webdashboard.online/vr/api/app/verify-pin.php";
        }
    }

    void Awake()
    {
        siginResultText.text = "";
        pinMaxLength = pinItems.Count;
        pin = new string[pinMaxLength];
        for (int i = 0; i < pin.Length; i++) pin[i] = "";
        if (Instance == null) Instance = this;
        showPinToggle.isOn = showPin;
        showPinToggle.onValueChanged.AddListener((value) => OnToggleShowPinChanged(value));
        ChangeShowPinToggleIcon();
    }

    private void OnToggleShowPinChanged(bool val)
    {
        showPin = val;
        foreach (var item in pinItems)
        {
            item.DisplayValue();
        }
        ChangeShowPinToggleIcon();
    }

    private void ChangeShowPinToggleIcon()
    {
        showPinToggleBg.sprite = showPin ? hidePinIcon : showPinIcon;
    }

    public void EnterPin(string p)
    {
        siginResultText.text = "";

        // Pin is all input
        if (IsPinAllFilled())
            return;

        for (int x = 0; x < pin.Length; x++)
        {
            if (pin[x] == "")
            {
                pin[x] = p;
                break;
            }
        }
        UpdatePinItems();
    }

    public void DeletePin()
    {
        for (int x = pin.Length-1; x >= 0; x--)
        {
            if (pin[x] != "")
            {
                pin[x] = "";
                break;
            }
        }
        UpdatePinItems();
    }

    public void SubmitPin()
    {
        if (!IsPinAllFilled())
        {
            siginResultText.text = string.Format("Plase enter pin {0} digits", pin.Length);
            return;
        }
        siginResultText.text = "";
        StartCoroutine(VerifyPin());
    }

    private IEnumerator VerifyPin()
    {
        ActivateSiginUI(false);
        string fullPin = "";
        for (int x = 0; x < pin.Length; x++)
            fullPin += pin[x];
            
        WWWForm form = new WWWForm();
        form.AddField("pin", fullPin);
        using (UnityWebRequest request = UnityWebRequest.Post(verifyPinUrl, form))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.useHttpContinue = false;
            //request.chunkedTransfer = false;
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                siginResult = new SiginResult
                {
                    error = "Request error",
                    message = "Error! Please try again later."
                };
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                siginResult = JsonUtility.FromJson<SiginResult>(request.downloadHandler.text);
                if (siginResult.error == "none")
                {
                    OnVerifyPinSuccessfully();
                }
                else
                {
                    OnVerifyPinFailed();
                }
            }
            ActivateSiginUI(true);
        }
    }

    private void OnVerifyPinSuccessfully()
    {
        StartCoroutine(FadeOutAlpha());
        SceneManager.LoadScene(1);
    }

    private void OnVerifyPinFailed()
    {
        siginResultText.text = siginResult.message;
    }

    IEnumerator FadeOutAlpha()
    {
        m_CanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(0.1f);
        while (m_CanvasGroup.alpha > 0)
        {
            m_CanvasGroup.alpha -= Time.deltaTime / fadeOutTime;
            yield return null;
        }
        signinPanel.SetActive(false);
        yield return null;
    }

    private void UpdatePinItems()
    {
        for (int i = 0; i < pinItems.Count; i++)
        {
            pinItems[i].SetValue(pin[i]);
        }
    }

    public bool IsPinAllFilled()
    {
        for (int x = 0; x < pin.Length; x++)
        {
            if (pin[x] == "")
            {
                return false;
            }
        }
        return true;
    }

    private void ActivateSiginUI(bool active)
    {
        foreach (var item in signinBtn)
        {
            item.interactable = active;
        }
    }
}

public class SiginResult
{
    public string error;
    public string message;
}
