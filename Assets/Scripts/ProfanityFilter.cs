using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProfanityFilter : Singleton<ProfanityFilter>
{
    private HashSet<string> profanityWords;

    void Start()
    {
        profanityWords = new HashSet<string>();
        LoadProfanityWords("da.txt");
        LoadProfanityWords("en.txt");
        LoadProfanityWords("de.txt");
    }

    private void LoadProfanityWords(string fileName)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(fullPath))
        {
            string[] lines = File.ReadAllLines(fullPath);
            foreach (string line in lines)
            {
                profanityWords.Add(line.Trim().ToLower());
            }
        }
        else
        {
            Debug.LogWarning($"Profanity list file not found at {fullPath}");
        }
    }

    public bool ContainsProfanity(string input)
    {
        string[] words = input.ToLower().Split(' ', ',', '.', '!', '?', ';', ':', '\n', '\t');
        foreach (string word in words)
        {
            if (profanityWords.Contains(word))
            {
                return true;
            }
        }
        return false;
    }
}
