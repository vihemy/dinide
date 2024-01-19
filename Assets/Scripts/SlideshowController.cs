using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SlideshowController : MonoBehaviour
{
    public Image imageDisplay;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI authorInfoText;
    public float displayDuration = 5f;
    private int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(DisplaySlideshow());
    }

    private IEnumerator DisplaySlideshow()
    {
        List<EntryData> entries = EntryCache.Instance.entries;
        while (entries.Count > 0)
        {
            EntryData currentEntry = entries[currentIndex];
            DisplayEntry(currentEntry);
            yield return new WaitForSeconds(displayDuration);
            currentIndex = (currentIndex + 1) % entries.Count;
        }
    }

    private void DisplayEntry(EntryData entryData)
    {
        imageDisplay.sprite = Sprite.Create(entryData.texture, new Rect(0, 0, entryData.texture.width, entryData.texture.height), new Vector2(0.5f, 0.5f));
        promptText.text = "\"" + entryData.prompt + "\"";
        authorInfoText.text = entryData.author + ", " + entryData.age;
    }
}
