using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DalleAPIManager : Singleton<DalleAPIManager>
{
    private string apiURL;
    private string apiKey;
    private bool isRequesting = false;

    private void Start()
    {
        apiURL = ConfigLoader.Instance.LoadFromConfig("API_URL");
        apiKey = ConfigLoader.Instance.LoadFromConfig("API_KEY");
    }

    public void RequestDalle(string prompt)
    {
        if (isRequesting)
        {
            Debug.LogWarning("Already requesting Dalle image");
        }
        else
        {
            StartCoroutine(SendDalleRequestCoroutine(prompt));
        }
    }

    private IEnumerator SendDalleRequestCoroutine(string prompt)
    {
        isRequesting = true;
        UnityWebRequest request = CreateDalleWebRequest(prompt);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ProcessResponse(request.downloadHandler.text, prompt);
        }
        else
        {
            Debug.LogError($"Error in Dalle Request: {request.error}");
        }

        isRequesting = false;
    }

    private UnityWebRequest CreateDalleWebRequest(string prompt)
    {
        object requestData = new DalleRequestData(prompt);
        string requestJson = JsonUtility.ToJson(requestData);
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestJson)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");
        ConfigureMemoryManagement(request);

        return request;
    }

    private void ProcessResponse(string jsonResponse, string prompt)
    {
        string imageUrl = ExtractImageUrl(jsonResponse);
        if (!string.IsNullOrEmpty(imageUrl))
        {
            var entryData = new EntryData { prompt = prompt, imageUrl = imageUrl };
            ImageDownloader.Instance.DownloadAndDisplayImage(entryData);
        }
    }

    private string ExtractImageUrl(string jsonResponse)
    {
        DalleResponse response = JsonUtility.FromJson<DalleResponse>(jsonResponse);
        if (response != null && response.data != null && response.data.Length > 0)
        {
            return response.data[0].url;
        }

        Debug.LogError("Invalid response or no image URL found");
        return null;
    }

    private static void ConfigureMemoryManagement(UnityWebRequest request)
    {
        request.disposeUploadHandlerOnDispose = request.disposeDownloadHandlerOnDispose = true;
    }
}
