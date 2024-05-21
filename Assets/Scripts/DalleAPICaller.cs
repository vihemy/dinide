using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DalleAPICaller : BaseAPICaller
{
    protected override void Start()
    {
        base.Start();
        apiURL = ConfigLoader.Instance.LoadFromConfig("API_IMAGE_URL");
    }

    public void RequestDalle(EntryData entry)
    {
        if (isRequesting)
        {
            Debug.LogWarning("Already requesting Dalle image");
        }
        else
        {
            var requestData = new DalleRequestData(entry.prompt);
            string requestJson = JsonUtility.ToJson(requestData);
            StartCoroutine(SendRequestCoroutine(requestJson, (response) => ProcessResponse(response, entry), HandleRequestError));
        }
    }

    private void HandleRequestError(UnityWebRequest request)
    {
        Debug.LogError($"Dalle request failed: {request.error}");
        // Handle the error appropriately here
    }

    private void ProcessResponse(string jsonResponse, EntryData entry)
    {
        Debug.Log("Response received: " + jsonResponse);
        var response = JsonUtility.FromJson<DalleResponse>(jsonResponse);
        if (response != null && response.data != null && response.data.Length > 0)
        {
            entry.created = response.created;
            entry.imageUrl = response.data[0].url;
            entry.revisedPrompt = response.data[0].revised_prompt;
            ImageDownloader.Instance.DownloadImage(entry);
        }
        else
        {
            Debug.LogError("Invalid response or no image URL found");
        }
    }
}
