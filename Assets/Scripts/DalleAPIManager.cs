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

    public void RequestDalle(EntryData entry)
    {
        if (isRequesting)
        {
            Debug.LogWarning("Already requesting Dalle image");
        }
        else
        {
            StartCoroutine(SendDalleRequestCoroutine(entry));
        }
    }

    private IEnumerator SendDalleRequestCoroutine(EntryData entry)
    {
        isRequesting = true;
        UnityWebRequest request = CreateDalleWebRequest(entry.prompt);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ProcessResponse(request.downloadHandler.text, entry);
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


    private void ProcessResponse(string jsonResponse, EntryData entry)
    {
        DalleResponse response = JsonUtility.FromJson<DalleResponse>(jsonResponse);
        if (response != null && response.data != null && response.data.Length > 0)
        {
            entry.created = response.created;
            entry.imageUrl = response.data[0].url;
            entry.revisedPrompt = response.data[0].revised_prompt; // Extracting the enhanced prompt
            ImageDownloader.Instance.DownloadImage(entry);
        }
        else
        {
            Debug.LogError("Invalid response or no image URL found");
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
