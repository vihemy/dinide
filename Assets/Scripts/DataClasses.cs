using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
public class DalleRequestData
{
    public string model = "dall-e-3";
    public string prompt;
    public int n = 1;
    public string size = "1024x1024";
    public DalleRequestData(string prompt)
    {
        this.prompt = prompt;
    }
}

[System.Serializable]
public class DalleResponse
{
    public string created;
    public ImageData[] data; // list used to support potential future multi-image requests
}

[System.Serializable]
public class ImageData
{
    public string url;
    public string revised_prompt;
}

[System.Serializable]
public class EntryData
{
    public string prompt;
    public string created; // unix time stamp formatted as string
    public string author;
    public string age;
    public string imageUrl;
    public string revisedPrompt;
    public Texture2D texture;

    public EntryData(
    string prompt = null,
    string author = null,
    string age = null,
    string created = null,
    string imageUrl = null,
    string revisedPrompt = null,
    Texture2D texture = null)

    {
        this.prompt = prompt;
        this.author = author;
        this.age = age;
        this.created = created;
        this.imageUrl = imageUrl;
        this.revisedPrompt = revisedPrompt;
        this.texture = texture;
    }
}

