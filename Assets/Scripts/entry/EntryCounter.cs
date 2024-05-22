using System.IO;
using UnityEngine;
using TMPro;

public class EntryCounter : Singleton<EntryCounter>
{
    [SerializeField] private TextMeshProUGUI counterText;

    private void Start()
    {
        SlideshowController.Instance.OnNewEntryAdded += RefreshCounterDisplay;
        RefreshCounterDisplay();
    }

    public void RefreshCounterDisplay()
    {
        int entryCount = CountEntriesOnDisk();
        string formattedCount = FormatWithLeadingZeros(entryCount);
        counterText.text = formattedCount;
    }

    private int CountEntriesOnDisk()
    {
        int persistentDataEntries = CountJsonFilesInDirectory(Application.persistentDataPath);
        int streamingAssetsEntries = CountJsonFilesInDirectory(Path.Combine(Application.streamingAssetsPath, "FallbackEntries"));

        return persistentDataEntries + streamingAssetsEntries;
    }

    private int CountJsonFilesInDirectory(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            FileInfo[] jsonFiles = directoryInfo.GetFiles("*.json");
            return jsonFiles.Length;
        }
        return 0;
    }

    private string FormatWithLeadingZeros(int count)
    {
        return count.ToString("D5");
    }
}
