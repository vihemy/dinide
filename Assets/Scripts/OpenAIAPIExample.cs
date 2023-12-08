using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenAIAPIExample : MonoBehaviour
{
    // URL for OpenAI Image Generation endpoint
    private readonly string openAIURL = "https://api.openai.com/v1/images/generations";

    // Your OpenAI API Key
    private readonly string apiKey = "YOUR_API_KEY_HERE";

    // Function to call the OpenAI API
    public IEnumerator GenerateImage(string prompt)
    {
        // Creating the request body
        var requestBody = new
        {
            model = "image-alpha-001", // Or other supported models
            prompt = prompt,
            n = 1,
            size = "1024x1024"
        };

        // Convert the request body to JSON
        string requestBodyJson = JsonUtility.ToJson(requestBody);

        // Create a UnityWebRequest for API call
        using (UnityWebRequest webRequest = new UnityWebRequest(openAIURL, "POST"))
        {
            // Convert JSON to byte array
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requestBodyJson);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            // Set the content type and authorization headers
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Handle the response (assuming JSON response)
                Debug.Log("Response: " + webRequest.downloadHandler.text);
                // Process the response to extract the image URL or image data
            }
        }
    }

    // Example usage
    void Start()
    {
        string userPrompt = "A futuristic cityscape"; // Replace with actual user prompt
        StartCoroutine(GenerateImage(userPrompt));
    }
}
