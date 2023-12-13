using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using static DalleAPIManager; // somehow cant reference EntryData without this namespace-decleration
public class ImageDownloader : Singleton<ImageDownloader>
{
    public Image displayImage; // Assign this in the inspector with your UI Image
    [SerializeField] private EntryDisplayer entryDisplayer;

    public void DownloadAndDisplayImage(EntryData entryData)
    {
        StartCoroutine(DownloadImage(entryData));
    }

    private IEnumerator DownloadImage(EntryData entryData)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(entryData.imageUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error While Sending: " + uwr.error);
            }
            else
            {
                // Get the downloaded image
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                entryDisplayer.CreateEntry(texture, entryData.prompt);
                SaveTextureAsPNG(texture, entryData.prompt);
            }
        }
    }

    private void SaveTextureAsPNG(Texture2D texture, string filename)
    {
        byte[] bytes = texture.EncodeToPNG();
        filename = filename + ".png";
        var filePath = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("Saved image to: " + filePath);
    }
}