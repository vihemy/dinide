using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//TODO:
// save as texture-file in stead of png, if this is more space efficient
// display from disk (persistentDataPath), so old images can be displayed. (images on openai's server expires after 1-2 hours)
// download as jpg to save size
// download prompt and revised prompt as metadata to image
public class ImageDownloader : MonoBehaviour
{
    public Image displayImage; // Assign this in the inspector with your UI Image
    [SerializeField] private EntryDisplayer entryDisplayer;

    public void DownloadAndDisplayImage(string imageUrl, string prompt)
    {
        StartCoroutine(DownloadImage(imageUrl, prompt));
    }

    private IEnumerator DownloadImage(string imageUrl, string prompt)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
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
                entryDisplayer.CreateEntry(texture, prompt);
                SaveTextureAsPNG(texture, prompt);
            }
        }
    }

    // MOVED TO ENTRYDISPLAYER.CS
    // private void DisplayImage(Texture2D texture)
    // {
    //     var rect = new Rect(0, 0, texture.width, texture.height);
    //     var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100);
    //     displayImage.sprite = sprite;
    // }


    private void SaveTextureAsPNG(Texture2D texture, string filename)
    {
        byte[] bytes = texture.EncodeToPNG();
        filename = filename + ".png";
        var filePath = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("Saved image to: " + filePath);
    }
}