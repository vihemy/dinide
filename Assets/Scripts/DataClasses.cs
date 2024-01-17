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
}