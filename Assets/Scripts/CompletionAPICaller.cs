using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CompletionAPICaller : BaseAPICaller
{
    protected override void Start()
    {
        base.Start();
        apiURL = ConfigLoader.Instance.LoadFromConfig("API_CHAT_URL");
    }

    public void CheckPromptRelated(EntryData entry, System.Action<EntryData> callback)
    {
        if (isRequesting)
        {
            Debug.LogWarning("Already checking prompt");
        }
        else
        {
            var requestData = new CompletionRequestData(entry.prompt);
            string requestJson = JsonUtility.ToJson(requestData);
            Debug.Log("Request JSON: " + requestJson); // Log the JSON payload
            StartCoroutine(SendRequestCoroutine(requestJson, (response) => ProcessResponse(response, entry, callback), HandleRequestError));
        }
    }

    private void HandleRequestError(UnityWebRequest request)
    {
        Debug.LogError($"Prompt check request failed: {request.error}");
        // Handle the error appropriately here
    }

    private void ProcessResponse(string jsonResponse, EntryData entry, System.Action<EntryData> callback)
    {
        Debug.Log("Response received: " + jsonResponse);
        var response = JsonUtility.FromJson<CompletionResponse>(jsonResponse);
        if (response != null && response.choices != null && response.choices.Length > 0)
        {
            string result = response.choices[0].message.content.Trim();
            Debug.Log("Result: " + result);

            // Update the EntryData instance based on the result
            entry.isRelevant = result.Equals("Related", System.StringComparison.OrdinalIgnoreCase);
            Debug.Log($"Is relevant for {entry.prompt}: {entry.isRelevant}");

            // Invoke the callback with the updated entry
            callback?.Invoke(entry);
        }
        else
        {
            Debug.LogError("Invalid response structure or no choices found");
        }
    }
}
