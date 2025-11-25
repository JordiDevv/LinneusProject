using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class IntroSceneBehavior : MonoBehaviour
{
    private VideoPlayer startupVideoUI;
    private VideoPlayer introVideoUI;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text welcomeMessage;

    [SerializeField] GameObject startupVideoGO;
    [SerializeField] GameObject splashScreenGO;
    [SerializeField] GameObject introVideoGO;
    [SerializeField] GameObject registrationGatewayGO;
    [SerializeField] GameObject welcomeMessageGO;

    void Start()
    {
        startupVideoUI = startupVideoGO.GetComponent<VideoPlayer>();
        introVideoUI = introVideoGO.GetComponent<VideoPlayer>();
        startupVideoUI.loopPointReached += (VideoPlayer) => CloseVideo(startupVideoGO, splashScreenGO);
    }

    void CloseVideo(GameObject videoObjectGO, GameObject nextObjectGO)
    {
        videoObjectGO.SetActive(false);
        nextObjectGO.SetActive(true);
    }

    public void ClickToContinue()
    {
        splashScreenGO.gameObject.SetActive(false);
        introVideoGO.SetActive(true);
        introVideoUI.loopPointReached += (VideoPlayer) => CloseVideo(introVideoGO, registrationGatewayGO);
    }

    public void SkipIntro()
    {
        introVideoGO.SetActive(false);
        registrationGatewayGO.SetActive(true);
    }

    public void GetInputText()
    {
        GlobalInfo.USERNAME = inputField.text;
        registrationGatewayGO.SetActive(false);
        welcomeMessageGO.SetActive(true);
        DBManager.instance.CreateUser();
        welcomeMessage.text = "Welcome, " + GlobalInfo.USERNAME;
    }
}
