using UnityEditor;
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
    public bool? isRelevant; // Nullable boolean to allow for null value if not evaluated

    public EntryData(
        string prompt = null,
        string author = null,
        string age = null,
        string created = null,
        string imageUrl = null,
        string revisedPrompt = null,
        Texture2D texture = null,
        bool? isRelevant = null)
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
    public string model = "gpt-4";
    public RequestMessage[] messages;

    public CompletionRequestData(string prompt)
    {
        string systemContext = "Your job is to determine if the prompted sentence is related to one or more of the following themes: \"ocean\", \"beach\", \"pollution\", \"garbage\", \"sea animals\", \"farming\", \"fishing\". The prompted sentence can be in multiple languages. Answer with 'Related' or 'Not related'.";

        messages = new RequestMessage[]
        {
            new RequestMessage { role = "system", content = systemContext },
            new RequestMessage { role = "user", content = prompt }
        };
    }

    [System.Serializable]
    public class RequestMessage
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
