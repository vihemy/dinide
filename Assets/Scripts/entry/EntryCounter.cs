using System.IO;
using UnityEngine;
using TMPro;

public class EntryCounter : Singleton<EntryCounter>
{
    [SerializeField] private TextMeshProUGUI counterText;

    private void Start()
    {
        RefreshCounterDisplay();
    }
    private int CountEntriesOnDisk()
    {
        string folderPath = Application.persistentDataPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] jsonFiles = directoryInfo.GetFiles("*.json");
        return jsonFiles.Length;
    }

    public void UpdateCounterOnScreen()
    {
        int entryCount = CountEntriesOnDisk();
        string formattedCount = FormatWithLeadingZeros(entryCount);
        counterText.text = formattedCount;
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

    // private string FormatWithLeadingZeros(int count)
    // {
    //     if (count < 10) // 1 digit
    //     {
    //         return count.ToString("0000");
    //     }
    //     else if (count < 100) // 2 digits
    //     {
    //         return count.ToString("000");
    //     }
    //     else if (count < 1000) // 3 digits
    //     {
    //         return count.ToString("00");
    //     }
    //     else // No leading zeros required for numbers with more than 3 digits
    //     {
    //         return count.ToString();
    //     }
    // }

    public void RefreshCounterDisplay()
    {
        UpdateCounterOnScreen();
    }
}