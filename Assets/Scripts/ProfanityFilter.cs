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
        LoadProfanityWords("profanity_en.txt");
        LoadProfanityWords("profanity_de.txt");
        LoadProfanityWords("profanity_da.txt");
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
        string[] words = input.Trim().ToLower().Split(new char[] { ' ', ',', '.', '!', '?', ';', ':', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && profanityWords.Contains(word))
            {
                return true;
            }
        }
        return false;
    }

}
