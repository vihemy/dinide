using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigLoader : Singleton<ConfigLoader>
{
    private Dictionary<string, string> configValues;

    void Start()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        string path = Path.Combine(Application.dataPath, "config.txt");
        configValues = new Dictionary<string, string>();

        if (!File.Exists(path))
        {
            Debug.LogError("Config file not found");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line) && line.Contains("="))
            {
                var splitLine = line.Split(new char[] { '=' }, 2);
                if (splitLine.Length == 2)
                {
                    configValues[splitLine[0].Trim()] = splitLine[1].Trim();
                }
            }
        }
    }

    public string LoadFromConfig(string key)
    {
        if (configValues.TryGetValue(key, out string value))
        {
            return value;
        }

        Debug.LogError($"Key '{key}' not found in config file.");
        return string.Empty;
    }
}
