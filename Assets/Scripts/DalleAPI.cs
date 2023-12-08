using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

// TODOS:
// Extract apiKey from code (set in file or in environment variable)
// Offline functionality: Save images to disk and use as backup, if connection failes or daily limit has been exceeded
// Make sure that multiple requests are not send at the time to avoid overspending.
// extract url from jsonresponse and pass to ImageDownloader
// extract prompt and pass as name or text to ImageDownloader (maybe create an object consisting of image/texture, prompt, and url)

public class DalleAPI : MonoBehaviour
{
    // Modified from this: https://community.openai.com/t/unity-request-to-openai-api-returns-empty-text/135714
    private readonly string apiURL = "https://api.openai.com/v1/images/generations";
    private readonly string apiKey = "***REMOVED***";

    [SerializeField] private ImageDownloader imageDownloader;

    public void DalleRequest(string prompt)
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
        SendWebRequest(prompt, request);
    }

    private static void ManageMemory(UnityWebRequest request)
    {
        //Memory management
        request.disposeUploadHandlerOnDispose = true;
        request.disposeCertificateHandlerOnDispose = true;
        request.disposeDownloadHandlerOnDispose = true;
    }
    private void SetRequestHeaders(UnityWebRequest request)
    {
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");
    }

    private void SendWebRequest(string prompt, UnityWebRequest request)
    {
        request.SendWebRequest().completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string responseInfo = request.downloadHandler.text;
                HandleResponse(responseInfo, prompt);
                Debug.Log("Response: " + responseInfo);
            }
        };
    }

    public void HandleResponse(string jsonResponse, string prompt)
    {
        DalleResponse response = JsonUtility.FromJson<DalleResponse>(jsonResponse);
        if (response != null && response.data != null && response.data.Length > 0)
        {

            string imageUrl = response.data[0].url;
            imageDownloader.DownloadAndDisplayImage(imageUrl, prompt);
        }
        else
        {
            Debug.LogError("Invalid response or no image URL found");
        }
    }

    [System.Serializable]
    public class DalleResponse
    {
        public ImageData[] data;
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
        public string image;
    }
}
