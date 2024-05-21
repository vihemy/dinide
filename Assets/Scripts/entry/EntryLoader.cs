using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Log("Loading relevant entries...");
        int maxEntries = EntryCache.Instance.maxEntries;
        string userGeneratedPath = Application.persistentDataPath;
        FileInfo[] userGeneratedFiles = GetJsonFiles(userGeneratedPath);

        if (!HasRelevantEntries(userGeneratedFiles))
        {
            Log("No relevant user-generated entries found, loading fallback entries...");
            LoadFallbackEntries(maxEntries);
        }
        else
        {
            LoadEntries(userGeneratedFiles, maxEntries);
            Log($"Loaded {EntryCache.Instance.entries.Count} entries");
        }
    }

    private bool HasRelevantEntries(FileInfo[] files)
    {
        foreach (var file in files)
        {
            EntryData entryData = LoadJson(file.FullName);
            if (entryData != null && entryData.isRelevant)
            {
                return true;
            }
        }
        return false;
    }

    private void LoadFallbackEntries(int maxEntries)
    {
        string fallbackPath = Path.Combine(Application.streamingAssetsPath, "FallbackEntries");
        FileInfo[] fallbackFiles = GetJsonFiles(fallbackPath);
        LoadEntries(fallbackFiles, maxEntries);
        Log($"Loaded fallback entries. Total entries: {EntryCache.Instance.entries.Count}");
    }

    private FileInfo[] GetJsonFiles(string folderPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        return directoryInfo.GetFiles("*.json").OrderBy(file => file.Name).ToArray();
    }

    private void LoadEntries(FileInfo[] files, int maxEntries)
    {
        int entriesToLoad = Mathf.Min(files.Length, maxEntries);

        for (int i = 0; i < entriesToLoad; i++)
        {
            string jsonPath = files[i].FullName;
            string imagePath = Path.ChangeExtension(jsonPath, ".jpg");
            EntryData entryData = LoadJsonAndImage(jsonPath, imagePath);
            AddToCache(entryData);
        }
    }

    private EntryData LoadJsonAndImage(string jsonPath, string imagePath)
    {
        EntryData entryData = LoadJson(jsonPath);
        if (entryData != null)
        {
            entryData.texture = LoadImage(imagePath);
        }
        return entryData;
    }

    private EntryData LoadJson(string jsonPath)
    {
        if (File.Exists(jsonPath))
        {
            string jsonContent = File.ReadAllText(jsonPath);
            return JsonUtility.FromJson<EntryData>(jsonContent);
        }
        Debug.LogWarning($"JSON file not found: {jsonPath}");
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
        Debug.LogWarning($"Image file not found: {imagePath}");
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

    private void Log(object message)
    {
        logger.Log(message);
    }
}
