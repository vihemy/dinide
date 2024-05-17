using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public abstract class BaseAPICaller : MonoBehaviour
{
    protected string apiURL;
    protected string apiKey;
    protected bool isRequesting = false;
    protected Logger logger;

    protected virtual void Awake()
    {
        logger = Logger.Instance;
    }

    protected virtual void Start()
    {
        apiKey = ConfigLoader.Instance.LoadFromConfig("API_KEY");
    }

    protected IEnumerator SendRequestCoroutine(string requestData, System.Action<string> onSuccess, System.Action<UnityWebRequest> onError)
    {
        isRequesting = true;
        UnityWebRequest request = CreateWebRequest(requestData);

        logger.Log($"Request sent with data: {requestData}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            logger.Log("Request successful");
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Request failed: {request.error}");
            HandleRequestError(request);
            onError?.Invoke(request);
        }

        isRequesting = false;
    }

    protected UnityWebRequest CreateWebRequest(string requestData)
    {
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestData)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");
        ConfigureMemoryManagement(request);

        return request;
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

    private static void ConfigureMemoryManagement(UnityWebRequest request)
    {
        request.disposeUploadHandlerOnDispose = request.disposeDownloadHandlerOnDispose = true;
    }
}
