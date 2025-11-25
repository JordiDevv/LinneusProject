using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;

public class DBManager : MonoBehaviour
{
    public static DBManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private IEnumerator SendRequest(object data, string methodAPI)
    {
        UnityWebRequest request = new UnityWebRequest("http://localhost:5000/api/requests", methodAPI);

        if (methodAPI == "POST" || methodAPI == "PUT")
        {
            string jsonData = JsonConvert.SerializeObject(data);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Data sent correctly: " + request.downloadHandler.text);
        else
            Debug.LogError("Error sending data: " + request.error);
    }

    public void CreateUser()
    {
        var data = new
        {
            service = "user-service",
            username = GlobalInfo.USERNAME
        };
        StartCoroutine(SendRequest(data, "POST"));
    }

    public void ExitLog()
    {
        var data = new
        {
            service = "session-service",
            username = GlobalInfo.USERNAME,
            action = "CloseSession"
        };
        StartCoroutine(SendRequest(data, "PUT"));
    }
}
