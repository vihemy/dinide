using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DalleAPICaller : BaseAPICaller
{
    public void RequestDalle(EntryData entry)
    {
        if (isRequesting)
        {
            Debug.LogWarning("Already requesting Dalle image");
        }
        else
        {
            string requestData = JsonUtility.ToJson(new DalleRequestData(entry.prompt));
            StartCoroutine(SendRequestCoroutine(requestData, (response) => ProcessResponse(response, entry), HandleRequestError));
        }
    }

    private void HandleRequestError(UnityWebRequest request)
    {
        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                GameManager.Instance.AbortProcessing(ErrorType.ConnectionError);
                break;
            case UnityWebRequest.Result.ProtocolError:
                GameManager.Instance.AbortProcessing(ErrorType.RequestError);
                break;
            default:
                GameManager.Instance.AbortProcessing(ErrorType.UnknownError);
                break;
        }
        Debug.LogError($"Dalle request failed: {request.error}");
    }

    private void ProcessResponse(string jsonResponse, EntryData entry)
    {
        DalleResponse response = JsonUtility.FromJson<DalleResponse>(jsonResponse);
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
