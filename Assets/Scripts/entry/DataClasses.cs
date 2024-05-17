using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DalleRequestData
{
    public string model;
    public string prompt;
    public int n;
    public string size;

    public DalleRequestData(string prompt)
    {
        this.prompt = prompt;
        this.model = ConfigLoader.Instance.LoadFromConfig("DALLE_MODEL");
        int.TryParse(ConfigLoader.Instance.LoadFromConfig("DALLE_N"), out this.n);
        this.size = ConfigLoader.Instance.LoadFromConfig("DALLE_SIZE");
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
    public bool isRelevant;

    public EntryData(
        string prompt = null,
        string author = null,
        string age = null,
        string created = null,
        string imageUrl = null,
        string revisedPrompt = null,
        Texture2D texture = null,
        bool isRelevant = false)
    {
        this.prompt = prompt;
        this.author = author;
        this.age = age;
        this.created = created;
        this.imageUrl = imageUrl;
        this.revisedPrompt = revisedPrompt;
        this.texture = texture;
        this.isRelevant = isRelevant;
    }
}


[System.Serializable]
public class CompletionRequestData
{
    public string model;
    public Message[] messages;

    public CompletionRequestData(string prompt)
    {
        model = ConfigLoader.Instance.LoadFromConfig("GPT_MODEL");
        string systemContext = ConfigLoader.Instance.LoadFromConfig("SYSTEM_CONTEXT");

        messages = new Message[]
        {
            new Message { role = "system", content = systemContext },
            new Message { role = "user", content = prompt }
        };
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }
}

[System.Serializable]
public class CompletionResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public ResponseMessage message;
}

[System.Serializable]
public class ResponseMessage
{
    public string content;
}
