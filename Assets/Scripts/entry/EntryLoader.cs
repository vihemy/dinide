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
        var relevantEntries = GetRelevantEntriesFromPersistentData(maxEntries);

        if (relevantEntries.Count < maxEntries)
        {
            int remainingEntries = maxEntries - relevantEntries.Count;
            var fallbackEntries = GetFallbackEntries(remainingEntries);
            relevantEntries.AddRange(fallbackEntries);
        }

        foreach (var entryData in relevantEntries)
        {
            AddToCache(entryData);
        }

        Log($"Total entries loaded: {EntryCache.Instance.entries.Count}");
    }

    private List<EntryData> GetRelevantEntriesFromPersistentData(int maxEntries)
    {
        string userGeneratedPath = Application.persistentDataPath;
        FileInfo[] userGeneratedFiles = GetJsonFiles(userGeneratedPath);
        List<EntryData> relevantUserEntries = LoadRelevantEntries(userGeneratedFiles, maxEntries);
        Log($"User generated entries loaded: {relevantUserEntries.Count}");
        return relevantUserEntries;
    }

    private List<EntryData> GetFallbackEntries(int maxEntries)
    {
        string fallbackPath = Path.Combine(Application.streamingAssetsPath, "FallbackEntries");
        if (!Directory.Exists(fallbackPath))
        {
            Log($"Fallback directory does not exist: {fallbackPath}");
            return new List<EntryData>();
        }

        FileInfo[] fallbackFiles = GetJsonFiles(fallbackPath);
        List<EntryData> fallbackEntries = LoadRelevantEntries(fallbackFiles, maxEntries);
        Log($"Fallback entries loaded: {fallbackEntries.Count}");
        return fallbackEntries;
    }

    private FileInfo[] GetJsonFiles(string folderPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        return directoryInfo.GetFiles("*.json")
                           .OrderByDescending(file => file.CreationTime) // Order by creation time
                           .ToArray();
    }

    private List<EntryData> LoadRelevantEntries(FileInfo[] files, int maxEntries)
    {
        List<EntryData> relevantEntries = new List<EntryData>();

        foreach (var file in files)
        {
            if (relevantEntries.Count >= maxEntries)
            {
                break;
            }

            EntryData entryData = LoadJsonIfRelevant(file.FullName);
            if (entryData != null)
            {
                entryData.texture = LoadImage(Path.ChangeExtension(file.FullName, ".jpg"));
                relevantEntries.Add(entryData);
            }
        }
        return relevantEntries;
    }

    private EntryData LoadJsonIfRelevant(string jsonPath)
    {
        try
        {
            if (File.Exists(jsonPath))
            {
                string jsonContent = File.ReadAllText(jsonPath);

                // Skip JSON files that do not contain the "isRelevant" field
                if (!jsonContent.Contains("\"isRelevant\""))
                {
                    return null;
                }

                EntryData entryData = JsonUtility.FromJson<EntryData>(jsonContent);

                // Check if the entry is relevant
                if (entryData.isRelevant)
                {
                    return entryData;
                }
            }
        }
        catch (System.Exception ex)
        {
            Log($"Error reading JSON file {jsonPath}: {ex.Message}");
        }
        return null;
    }

    private Texture2D LoadImage(string imagePath)
    {
        try
        {
            if (File.Exists(imagePath))
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                return texture;
            }
        }
        catch (System.Exception ex)
        {
            Log($"Error reading image file {imagePath}: {ex.Message}");
        }
        return null;
    }

    private void AddToCache(EntryData entryData)
    {
        if (entryData != null && entryData.texture != null)
        {
            EntryCache.Instance.AddEntryIfRelevant(entryData);
        }
    }

    private void Log(object message)
    {
        logger.Log(message);
    }
}
