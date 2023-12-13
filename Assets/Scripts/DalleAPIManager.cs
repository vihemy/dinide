using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class DalleAPIManager : Singleton<DalleAPIManager>
{
    // Modified from this: https://community.openai.com/t/unity-request-to-openai-api-returns-empty-text/135714
    private readonly string apiURL = "https://api.openai.com/v1/images/generations";
    private readonly string apiKey = "***REMOVED***";

    public void RequestDalle(string prompt)
    {
        StartCoroutine(SendWebRequestCoroutine(prompt));
    }

    private IEnumerator SendWebRequestCoroutine(string prompt)
    {
        UnityWebRequest request = SetupRequest(prompt);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log(request.error);
        }
        else
        {
            string response = request.downloadHandler.text; // EXTRACT AS METHOD
            Debug.Log("Response: " + response); // INCLUDE IN METHOD
            EntryData entryData = new EntryData();
            entryData.prompt = prompt;
            entryData.imageUrl = ExtractUrl(response);

            ImageDownloader.Instance.DownloadAndDisplayImage(entryData);
        }
    }

    private UnityWebRequest SetupRequest(string prompt)
    {
        string requestData = "{\"model\": \"dall-e-3\", " +
                             "\"prompt\": \"" + prompt + "\", " +
                             "\"n\": 1, " +
                            "\"size\": \"1024x1024\"}";

        UnityWebRequest request = UnityWebRequest.Post(apiURL, ""); // seconed argument is empty string, because the data will be sent as raw bytes in the request body
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw); // uses raw to achieve greater control of data format (json), greater effeciency in data   transmission, and bypassing unity built-in serialization.
        request.downloadHandler = new DownloadHandlerBuffer();

        ManageMemory(request);
        SetRequestHeaders(request);
        return request;
    }

    private static void ManageMemory(UnityWebRequest request)
    {
        request.disposeUploadHandlerOnDispose = true;
        request.disposeCertificateHandlerOnDispose = true;
        request.disposeDownloadHandlerOnDispose = true;
    }
    private void SetRequestHeaders(UnityWebRequest request)
    {
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");
    }

    private string ExtractUrl(string jsonResponse)
    {
        DalleResponse response = JsonUtility.FromJson<DalleResponse>(jsonResponse);

        string imageUrl = null;
        if (response != null && response.data != null && response.data.Length > 0)
        {
            imageUrl = response.data[0].url;
        }
        else
        {
            Debug.LogError("Invalid response or no image URL found");
        }

        return imageUrl;
    }

    [System.Serializable]
    public class DalleResponse
    {
        public ImageData[] data; // uses array because multiple images can be returned if n > 1 (dalle-2)
    }

    [System.Serializable]
    public class ImageData
    {
        public string url;
    }

    [System.Serializable]
    public class EntryData
    {
        public string prompt;
        public string imageUrl;
    }
}
