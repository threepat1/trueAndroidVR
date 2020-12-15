using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> m_canvasGroups = new List<CanvasGroup>();
    public UnityEvent OnSplashStart;
    public UnityEvent OnSplashEnd;
    public UnityEvent OnSplashComplete;

    [SerializeField] private float displayTime = 2;
    [SerializeField] private float fadeInTime = 1;
    [SerializeField] private float fadeOutTime = 1;

    [SerializeField] private CanvasGroup m_containerCanvasGroup;

    [SerializeField] string m_nextScene;

    [SerializeField] private bool isSplashComplete = false;
    void Start()
    {
        HideAllSplash();
        StartCoroutine(Splashing());
        LoadNextScene();
    }

    private void HideAllSplash()
    {
        foreach (var cg in m_canvasGroups)
            cg.alpha = 0;
    }

    private IEnumerator Splashing()
    {
        if (OnSplashStart != null)
            OnSplashStart.Invoke();

        yield return new WaitForSeconds(0.1f);

        // Fade in and out each splash canvas
        foreach (var cg in m_canvasGroups)
        {
            while (cg.alpha < 1)
            {
                cg.alpha += Time.deltaTime / fadeInTime;
                yield return null;
            }
            yield return new WaitForSeconds(displayTime);
            while (cg.alpha > 0)
            {
                cg.alpha -= Time.deltaTime / fadeOutTime;
                yield return null;
            }
        }

        if (OnSplashEnd != null)
            OnSplashEnd.Invoke();

        // Fade out container
        if(m_containerCanvasGroup != null)
        {
            while (m_containerCanvasGroup.alpha > 0)
            {
                m_containerCanvasGroup.alpha -= Time.deltaTime / fadeOutTime;
                yield return null;
            }
        }

        isSplashComplete = true;
        if (OnSplashComplete != null)
            OnSplashComplete.Invoke();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_nextScene);
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if(isSplashComplete && !asyncLoad.allowSceneActivation)
            {
                yield return new WaitForSeconds(.25f);
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        //while(!isSplashComplete)
        //{
        //    //Debug.Log("ssss");

        //    yield return null;
        //}
        //asyncLoad.allowSceneActivation = true;
    }
}
