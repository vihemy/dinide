using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class DalleAPIManager : Singleton<DalleAPIManager>
{
    private string apiURL;
    private string apiKey;

    private void Start()
    {
        LoadConfiguration();
    }

    public void RequestDalle(string prompt)
    {
        StartCoroutine(SendDalleRequest(prompt));
    }

    private IEnumerator SendDalleRequest(string prompt)
    {
        UnityWebRequest request = CreateDalleRequest(prompt);

        yield return request.SendWebRequest();

        if (IsRequestSuccessful(request))
        {
            HandleSuccessfulRequest(request, prompt);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    private UnityWebRequest CreateDalleRequest(string prompt)
    {
        string requestData = CreateRequestData(prompt);
        UnityWebRequest request = UnityWebRequest.Post(apiURL, "");
        SetRequestBody(request, requestData);
        SetRequestHeaders(request);
        ConfigureMemoryManagement(request);

        return request;
    }

    private void LoadConfiguration()
    {
        apiURL = ConfigLoader.Instance.LoadFromConfig("API_URL");
        apiKey = ConfigLoader.Instance.LoadFromConfig("API_KEY");
    }

    private static string CreateRequestData(string prompt)
    {
        return "{\"model\": \"dall-e-3\", " +
               "\"prompt\": \"" + prompt + "\", " +
               "\"n\": 1, " +
               "\"size\": \"1024x1024\"}";
    }

    private static void SetRequestBody(UnityWebRequest request, string data)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
    }

    private void SetRequestHeaders(UnityWebRequest request)
    {
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");
    }

    private static void ConfigureMemoryManagement(UnityWebRequest request)
    {
        request.disposeUploadHandlerOnDispose = true;
        request.disposeCertificateHandlerOnDispose = true;
        request.disposeDownloadHandlerOnDispose = true;
    }

    private static bool IsRequestSuccessful(UnityWebRequest request)
    {
        return request.result != UnityWebRequest.Result.ConnectionError &&
               request.result != UnityWebRequest.Result.DataProcessingError;
    }

    private void HandleSuccessfulRequest(UnityWebRequest request, string prompt)
    {
        string response = request.downloadHandler.text;
        Debug.Log("Response: " + response);

        string imageUrl = ExtractUrlFromResponse(response);
        if (!string.IsNullOrEmpty(imageUrl))
        {
            var entryData = new EntryData { prompt = prompt, imageUrl = imageUrl };
            ImageDownloader.Instance.DownloadAndDisplayImage(entryData);
        }
    }

    private string ExtractUrlFromResponse(string jsonResponse)
    {
        DalleResponse response = JsonUtility.FromJson<DalleResponse>(jsonResponse);
        if (response != null && response.data != null && response.data.Length > 0)
        {
            return response.data[0].url;
        }

        Debug.LogError("Invalid response or no image URL found");
        return null;
    }
}
