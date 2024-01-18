using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : Singleton<ImageDownloader>
{
    public Image displayImage; // Assign in the inspector
    [SerializeField] private EntryDisplayer entryDisplayer;

    public void DownloadAndDisplayImage(EntryData entry)
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
        }
        else
        {
            Debug.LogError($"Error downloading image {entry.prompt}: {request.error}");
        }
    }

    private void ProcessDownloadedTexture(UnityWebRequest request, EntryData entry)
    {
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        entry.image = texture;
        entryDisplayer.CreateAndDisplayEntry(entry);
        SaveTextureAsJPG(entry); // Updated to save as JPG
    }

    private void SaveTextureAsJPG(EntryData entry) // Renamed method
    {
        byte[] bytes = entry.image.EncodeToJPG(); // Encoding to JPG
        string filePath = GenerateJPGFilePath(entry.prompt); // Updated method name
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"Saved image to: {filePath}");
    }

    private string GenerateJPGFilePath(string filename) // Renamed method
    {
        filename = $"{filename}.jpg"; // Updated extension to JPG
        return Path.Combine(Application.persistentDataPath, filename);
    }

    private static bool IsWebRequestSuccessful(UnityWebRequest request)
    {
        return request.result == UnityWebRequest.Result.Success;
    }
}
