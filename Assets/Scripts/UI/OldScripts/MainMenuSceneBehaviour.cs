using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class MainMenuSceneBehaviour : MonoBehaviour
{
    [SerializeField] GameObject blockerGO;

    public GameObject[] allSections;

    [SerializeField] CanvasScaler canvasScalerMainMenu;
    [SerializeField] GameObject funnelFX;

    public void OnIntroAnimationEnd()
    {
        blockerGO.SetActive(false);
    }

    public void ChangeSection()
    {
        GameObject senderGO = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        blockerGO.SetActive(true);
        funnelFX.SetActive(true);
        StartCoroutine(ZoomIn());
        ActivateNextSectionEvent(senderGO);
    }

    IEnumerator ZoomIn()
    {
        float elapsedTime = 0, targetScale = 3f, duration = 0.8f, startScale = canvasScalerMainMenu.scaleFactor;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            canvasScalerMainMenu.scaleFactor = Mathf.Lerp(startScale, targetScale, lerp);
            yield return null;
        }
    }

    void ActivateNextSectionEvent(GameObject senderGO)
    {
        if (senderGO == null)
        {
            Debug.LogWarning("No button detected.");
            return;
        }

        string targetSection = senderGO.name.Replace("Button", "Section");
        GameObject actualSectionGO = senderGO.transform.parent.gameObject;

        foreach (GameObject panel in allSections)
        {
            if (panel.name == targetSection)
            {
                panel.SetActive(true);
                GameObject panelBackgroundGO = panel.transform.Find("UI_Background").gameObject;
                StartCoroutine(Fade(panelBackgroundGO, 0f, 1f, () => {funnelFX.SetActive(false); blockerGO.SetActive(false); actualSectionGO.SetActive(false);}));
                break;
            }
        }
    }

    IEnumerator Fade(GameObject objectToFadeGO, float startAlpha, float targetAlpha, Action onComplete = null)
    {
        RawImage objectToFadeIMG = objectToFadeGO.GetComponent<RawImage>();
        Color objectToFadeCOL = objectToFadeIMG.color;
        float elapsedTime = 0f, fadeDuration = 0.5f;

        objectToFadeCOL.a = startAlpha;
        objectToFadeIMG.color = objectToFadeCOL;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float smoothT = Mathf.Lerp(0, 1, t);

            objectToFadeCOL.a = Mathf.Lerp(startAlpha, targetAlpha, smoothT);
            objectToFadeIMG.color = objectToFadeCOL;

            yield return null;
        }

        onComplete?.Invoke();
    }

    public void ExitGame()
    {
        Debug.Log("Sale");
        DBManager.instance.ExitLog();
        Application.Quit();
    }
}
