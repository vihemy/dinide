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
}