using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class EntryLoader : Singleton<EntryLoader>
{
    new private void Awake()
    {
        LoadAllEntries();
    }

    private void LoadAllEntries()
    {
        string folderPath = Application.persistentDataPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] jsonFiles = directoryInfo.GetFiles("*.json");

        foreach (FileInfo file in jsonFiles)
        {
            string jsonPath = file.FullName;
            string imagePath = Path.ChangeExtension(jsonPath, ".jpg");
            EntryData entryData = LoadJsonAndImageIntoEntry(jsonPath, imagePath);
            // DisplayEntry(entryData);
            AddToCache(entryData);
        }
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
            EntryCache.Instance.AddEntry(entryData);
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
}
