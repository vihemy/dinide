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
        string folderPath = Application.persistentDataPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] jsonFiles = directoryInfo.GetFiles("*.json");
        return jsonFiles.Length;
    }

    private string FormatWithLeadingZeros(int count)
    {
        string countString = count.ToString();

        if (countString.Length == 1) // 1 digit
        {
            return $"000{countString}";
        }
        else if (countString.Length == 2) // 2 digits
        {
            return $"00{countString}";
        }
        else if (countString.Length == 3) // 3 digits
        {
            return $"0{countString}";
        }
        else // No leading zeros required for numbers with more than 3 digits
        {
            return countString;
        }
    }
}