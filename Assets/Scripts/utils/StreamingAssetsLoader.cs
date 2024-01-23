using System;
using System.IO;
using UnityEngine;

public static class StreamingAssetsLoader
{
    /// <summary>
    /// Loads the content of a file from the StreamingAssets folder.
    /// </summary>
    /// <param name="fileName">Name of the file in the StreamingAssets folder.</param>
    /// <returns>The content of the file as a string.</returns>
    public static string LoadFileContent(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }

        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error reading file: " + ex.Message);
            return null;
        }
    }
}
