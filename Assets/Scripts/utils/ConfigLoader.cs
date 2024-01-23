using System.Collections.Generic;
using UnityEngine;

public class ConfigLoader : Singleton<ConfigLoader>
{
    private Dictionary<string, string> configValues;

    new void Awake() // needs to run before LoadFromConfig is called
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        TextAsset configFile = Resources.Load<TextAsset>("config");
        if (configFile == null)
        {
            Debug.LogError("Config file not found in Resources");
            return;
        }

        configValues = new Dictionary<string, string>();
        string[] lines = configFile.text.Split('\n');
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
