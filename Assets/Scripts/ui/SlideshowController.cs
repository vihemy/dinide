using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SlideshowController : Singleton<SlideshowController>
{
    public Image imageDisplay;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI authorInfoText;
    public float displayDuration = 5f;
    private int currentIndex = 0;
    private Coroutine slideshowCoroutine;
    private Logger logger;

    new void Awake()
    {
        logger = Logger.Instance;
    }

    public void InstantiateSlideshow()
    {
        slideshowCoroutine = StartCoroutine(DisplaySlideshow());
    }

    private IEnumerator DisplaySlideshow()
    {
        List<EntryData> entries = EntryCache.Instance.entries;

        while (true)
        {
            if (entries.Count > 0)
            {
                DisplayEntry(entries[currentIndex]);
                currentIndex = (currentIndex + 1) % entries.Count;
                yield return new WaitForSeconds(displayDuration);
            }
            else
            {
                yield return null; // Wait for next frame if no entries
            }
        }
    }

    public void UpdateAndDisplayNewEntry(EntryData newEntry)
    {
        StopCoroutine(slideshowCoroutine); // Stop the current slideshow
        currentIndex = EntryCache.Instance.entries.IndexOf(newEntry);
        DisplayEntry(newEntry);
        slideshowCoroutine = StartCoroutine(DisplaySlideshow()); // Restart the slideshow
    }

    private void DisplayEntry(EntryData entryData)
    {
        imageDisplay.sprite = Sprite.Create(entryData.texture, new Rect(0, 0, entryData.texture.width, entryData.texture.height), new Vector2(0.5f, 0.5f));
        promptText.text = "\"" + entryData.prompt + "\"";
        authorInfoText.text = entryData.author + ", " + entryData.age;
    }
}
