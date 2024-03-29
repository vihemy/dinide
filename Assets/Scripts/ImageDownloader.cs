using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : Singleton<ImageDownloader>
{

    private Logger logger;
    private DashboardManager dashboardManager;

    private EntryData finalizedEntry;

    new void Awake()
    {
        logger = Logger.Instance;
        dashboardManager = DashboardManager.Instance; // assign as private variable to avoid bug in OnDestroy when nullchecking DashboardManager.Instance
        DashboardManager.Instance.OnDashboardAnimationEnd += OnDashboardAnimationEnded;
    }

    public void DownloadImage(EntryData entry)
    {
        StartCoroutine(DownloadImageCoroutine(entry));
    }

    private IEnumerator DownloadImageCoroutine(EntryData entry)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(entry.imageUrl))
        {
            yield return request.SendWebRequest();
            HandleWebRequestResult(request, entry);
        }
    }

    private void HandleWebRequestResult(UnityWebRequest request, EntryData entry)
    {
        if (IsWebRequestSuccessful(request))
        {
            ProcessDownloadedTexture(request, entry);
            logger.Log($"Download succesfull. Image with prompt: {entry.prompt}");
        }
        else
        {
            Debug.LogError($"Error downloading image {entry.prompt}: {request.error}");
        }
    }

    private void ProcessDownloadedTexture(UnityWebRequest request, EntryData entry)
    {
        entry.texture = DownloadHandlerTexture.GetContent(request);
        finalizedEntry = entry;
        SaveTextureAsJPG(entry); // Save image as JPG
        SaveEntryDataAsJson(entry); // Save metadata as JSON
    }

    private void SaveTextureAsJPG(EntryData entry)
    {
        byte[] bytes = entry.texture.EncodeToJPG();
        string filePath = GenerateJPGFilePath(entry.prompt);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"Saved image to: {filePath}");
    }

    private void SaveEntryDataAsJson(EntryData entry)
    {
        string json = JsonUtility.ToJson(entry);
        string jsonFilePath = GenerateJsonFilePath(entry.prompt);
        File.WriteAllText(jsonFilePath, json);
        Debug.Log($"Saved metadata to: {jsonFilePath}");
    }

    private string GenerateJPGFilePath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, $"{filename}.jpg");
    }

    private string GenerateJsonFilePath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, $"{filename}.json");
    }

    private void OnDashboardAnimationEnded()
    {
        Debug.Log("ImageDownloader.OnDashboardAnimationEnded called");
        AddToCache(finalizedEntry);
        GameManager.Instance.FinishProcessing();
    }

    private void AddToCache(EntryData entryData)
    {
        if (entryData != null && entryData.texture != null)
        {
            EntryCache.Instance.AddEntryDuringRuntime(entryData);
        }
        else
        {
            Debug.LogWarning("Entry data or texture is null");
        }
    }

    private static bool IsWebRequestSuccessful(UnityWebRequest request)
    {
        return request.result == UnityWebRequest.Result.Success;
    }

    private void OnDestroy()
    {
        if (dashboardManager != null)
        {
            dashboardManager.OnDashboardAnimationEnd -= OnDashboardAnimationEnded;
        }
    }

}
