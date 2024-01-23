using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConfigLoader : Singleton<ConfigLoader>
{
    private Dictionary<string, string> configValues;

    new void Awake()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        string configFilePath = Path.Combine(Application.streamingAssetsPath, "config.txt");

        if (!File.Exists(configFilePath))
        {
            Debug.LogError("Config file not found in StreamingAssets");
            return;
        }

        configValues = new Dictionary<string, string>();

        string[] lines = File.ReadAllLines(configFilePath);
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
