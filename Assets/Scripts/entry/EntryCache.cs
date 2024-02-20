using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryCache : Singleton<EntryCache>
{
    public List<EntryData> entries = new List<EntryData>();
    public int maxEntries = 10; // Set your desired maximum size

    private Logger logger;

    new void Awake()
    {
        logger = Logger.Instance;
    }

    public void AddEntryPreRuntime(EntryData entry)
    {
        if (entries.Count >= maxEntries)
        {
            entries.RemoveAt(0); // Remove the oldest entry
        }
        entries.Add(entry);
    }

    public void AddEntryDuringRuntime(EntryData entry)
    {
        if (entries.Count >= maxEntries)
        {
            entries.RemoveAt(0); // Remove the oldest entry
        }
        entries.Add(entry);
        
        SlideshowController.Instance.DisplayNewEntryAndRestartSlideShow(entry);
    }

    public void RemoveEntry(EntryData entry)
    {
        entries.Remove(entry);
    }

    public void RemoveEntry(int index)
    {
        entries.RemoveAt(index);
    }

    public void ClearEntries()
    {
        entries.Clear();
    }

}
