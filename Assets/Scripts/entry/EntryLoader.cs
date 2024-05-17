using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class EntryLoader : Singleton<EntryLoader>
{
    private Logger logger;

    new void Awake()
    {
        logger = Logger.Instance;
    }

    public void LoadLatestEntries()
    {
        Log("Loading entries...");
        int maxEntries = EntryCache.Instance.maxEntries;
        string folderPath = Application.persistentDataPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] jsonFiles = directoryInfo.GetFiles("*.json");

        // Sort the files by last write time (most recent first)
        var sortedFiles = jsonFiles.OrderByDescending(file => file.LastWriteTime).ToArray();

        // Load only the number of entries specified in EntryCache.MaxEntries
        for (int i = 0; i < Mathf.Min(sortedFiles.Length, maxEntries); i++)
        {
            FileInfo file = sortedFiles[i];
            Debug.Log($"Entry {file.Name} is relevant");
            string jsonPath = file.FullName;
            string imagePath = Path.ChangeExtension(jsonPath, ".jpg");
            EntryData entryData = LoadJsonAndImageIntoEntry(jsonPath, imagePath);
            AddToCache(entryData);
        }
        Log($"Loaded {EntryCache.Instance.entries.Count} entries");
    }

    private EntryData LoadJsonAndImageIntoEntry(string jsonPath, string imagePath)
    {
        EntryData entryData = LoadJson(jsonPath);
        entryData.texture = LoadImage(imagePath);
        return entryData;
    }

    private EntryData LoadJson(string jsonPath)
    {
        if (File.Exists(jsonPath))
        {
            string jsonContent = File.ReadAllText(jsonPath);
            return JsonUtility.FromJson<EntryData>(jsonContent);
        }
        Debug.LogWarning("JSON file not found: " + jsonPath);
        return null;
    }

    private Texture2D LoadImage(string imagePath)
    {
        if (File.Exists(imagePath))
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return texture;
        }
        Debug.LogWarning("Image file not found: " + imagePath);
        return null;
    }

    private static void AddToCache(EntryData entryData)
    {
        if (entryData != null && entryData.texture != null)
        {
            EntryCache.Instance.AddEntryIfRelevant(entryData);

        }
        else
        {
            Debug.LogWarning("Entry data or texture is null");
        }
    }

    private void DisplayEntry(EntryData entryData)
    {
        if (entryData != null && entryData.texture != null)
        {
            EntryDisplayer.Instance.CreateEntryDisplay(entryData);
        }
        else
        {
            Debug.LogWarning("Entry data or texture is null");
        }
    }

    private void Log(object message)
    {
        logger.Log(message);
    }
}
