using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryCache : Singleton<EntryCache>
{
    public List<EntryData> entries = new List<EntryData>();

    public void AddEntry(EntryData entry)
    {
        entries.Add(entry);
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
