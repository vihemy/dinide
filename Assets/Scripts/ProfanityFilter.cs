using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProfanityFilter : Singleton<ProfanityFilter>
{
    [SerializeField] private TextAsset danishProfanityList;
    [SerializeField] private TextAsset englishProfanityList;
    [SerializeField] private TextAsset germanProfanityList;

    private HashSet<string> profanityWords;

    void Start()
    {
        profanityWords = new HashSet<string>();
        LoadProfanityWords(englishProfanityList);
        LoadProfanityWords(danishProfanityList);
        LoadProfanityWords(germanProfanityList);
    }

    private void LoadProfanityWords(TextAsset profanityList)
    {
        if (profanityList != null)
        {
            string[] lines = profanityList.text.Split('\n');
            foreach (string line in lines)
            {
                profanityWords.Add(line.Trim().ToLower());
            }
        }
        else
        {
            Debug.LogWarning("Profanity list asset is null");
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
