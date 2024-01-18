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
    public ImageData[] data;
}

[System.Serializable]
public class ImageData
{
    public string url;
}

[System.Serializable]
public class EntryData
{
    public string prompt;
    public string imageUrl;
    public Texture2D image;
    public string author;
    public int age;

    public EntryData(string prompt = null, string author = null, int age = 0, string imageUrl = null, Texture2D image = null)
    {
        this.prompt = prompt;
        this.author = author;
        this.age = age;
        this.imageUrl = imageUrl;
        this.image = image;
    }
}

