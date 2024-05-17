using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CompletionAPICaller : BaseAPICaller
{
    public void CheckPromptRelated(EntryData entry)
    {
        if (isRequesting)
        {
            Debug.LogWarning("Already checking prompt");
        }
        else
        {
            var requestData = new CompletionRequestData(entry.prompt);
            string requestJson = JsonUtility.ToJson(requestData);
            StartCoroutine(SendRequestCoroutine(requestJson, (response) => ProcessResponse(response, entry), HandleRequestError));
        }
    }

    private void HandleRequestError(UnityWebRequest request)
    {
        Debug.LogError($"Prompt check request failed: {request.error}");
        // Handle the error appropriately here
    }

    private void ProcessResponse(string jsonResponse, EntryData entry)
    {
        var response = JsonUtility.FromJson<CompletionResponse>(jsonResponse);
        string result = response.choices[0].message.content.Trim();
        Debug.Log("Result: " + result);

        // Update the EntryData instance based on the result
        entry.isRelavant = result.Equals("Related", System.StringComparison.OrdinalIgnoreCase) ? true : (bool?)false;
    }
}
