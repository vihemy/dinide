using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : Singleton<ImageDownloader>
{
    public Image displayImage; // Assign in the inspector
    [SerializeField] private EntryDisplayer entryDisplayer;

    public void DownloadAndDisplayImage(EntryData entryData)
    {
        StartCoroutine(DownloadImageCoroutine(entryData));
    }

    private IEnumerator DownloadImageCoroutine(EntryData entryData)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(entryData.imageUrl))
        {
            yield return request.SendWebRequest();
            HandleWebRequestResult(request, entryData);
        }
    }

    private void HandleWebRequestResult(UnityWebRequest request, EntryData entryData)
    {
        if (IsWebRequestSuccessful(request))
        {
            ProcessDownloadedTexture(request, entryData);
        }
        else
        {
            Debug.LogError($"Error downloading image {entryData.prompt}: {request.error}");
        }
    }

    private void ProcessDownloadedTexture(UnityWebRequest request, EntryData entryData)
    {
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        entryDisplayer.CreateEntry(texture, entryData.prompt);
        SaveTextureAsPNG(texture, entryData.prompt);
    }

    private void SaveTextureAsPNG(Texture2D texture, string filename)
    {
        byte[] bytes = texture.EncodeToPNG();
        string filePath = GeneratePNGFilePath(filename);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"Saved image to: {filePath}");
    }

    private string GeneratePNGFilePath(string filename)
    {
        filename = $"{filename}.png";
        return Path.Combine(Application.persistentDataPath, filename);
    }

    private static bool IsWebRequestSuccessful(UnityWebRequest request)
    {
        return request.result == UnityWebRequest.Result.Success;
    }
}
