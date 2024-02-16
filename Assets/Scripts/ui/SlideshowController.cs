using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SlideshowController : Singleton<SlideshowController>
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI authorInfoText;
    [SerializeField] private float displayDuration = 5f;
    private int currentIndex = 0;
    private Coroutine slideshowCoroutine;

    void Start()
    {
        LightController.Instance.OnFlashHalfway += InstantiateSlideshow;
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

    // VIRKER IKKE - LAVER ROD I SLIDESHOWCOROUTINEN!
    public void UpdateSlideShowAndCounter(EntryData newEntry)
    {
        StopCoroutine(slideshowCoroutine); // Stop the current slideshow
        TriggerFlash(); // Lightcontroller trigger OnFlashHalfway event
        SetCurrentIndexToEntry(newEntry);
        EntryCounter.Instance.RefreshCounterDisplay();
    }

    private void SetCurrentIndexToEntry(EntryData newEntry)
    {
        currentIndex = EntryCache.Instance.entries.IndexOf(newEntry);
    }

    private void DisplayEntry(EntryData entryData)
    {
        spriteRenderer.sprite = Sprite.Create(entryData.texture, new Rect(0, 0, entryData.texture.width, entryData.texture.height), new Vector2(0.5f, 0.5f));
        promptText.text = "\"" + entryData.prompt + "\"";
        authorInfoText.text = entryData.author + ", " + entryData.age;
    }

    private void TriggerFlash()
    {
        LightController.Instance.Flash();
    }

    void OnDestroy()
    {
        LightController.Instance.OnFlashHalfway -= InstantiateSlideshow;
    }
}
