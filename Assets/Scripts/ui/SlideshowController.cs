using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class SlideshowController : Singleton<SlideshowController>
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI authorInfoText;
    [SerializeField] private float displayDuration = 5f; // Easily adjustable
    [SerializeField] private float fadeDuration = 1f;    // Total fade duration (in and out)

    private int currentIndex = 0;
    private Coroutine slideshowCoroutine;

    public void RestartSlideshow()
    {
        StopSlideShowCoroutine();
        slideshowCoroutine = StartCoroutine(DisplaySlideshow());
    }

    private void StopSlideShowCoroutine()
    {
        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
        }
    }

    private IEnumerator DisplaySlideshow()
    {
        while (true)
        {
            yield return StartCoroutine(DisplayEntry(EntryCache.Instance.entries[currentIndex]));
            currentIndex = (currentIndex + 1) % EntryCache.Instance.entries.Count;
        }
    }

    private IEnumerator DisplayEntry(EntryData entry)
    {
        yield return StartCoroutine(FadeSprite(0f, fadeDuration / 2));
        UpdateEntryDisplay(entry);
        yield return StartCoroutine(FadeSprite(1f, fadeDuration / 2));
        yield return new WaitForSeconds(displayDuration);
    }

    private void UpdateEntryDisplay(EntryData entry)
    {
        spriteRenderer.sprite = Sprite.Create(entry.texture, new Rect(0, 0, entry.texture.width, entry.texture.height), new Vector2(0.5f, 0.5f)); ;
        promptText.text = entry.prompt;
        authorInfoText.text = authorInfoText.text = entry.author + ", " + entry.age; ;
    }

    private IEnumerator FadeSprite(float targetAlpha, float duration)
    {
        float startAlpha = spriteRenderer.color.a;
        float timer = 0f;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void DisplayNewEntryAndRestartSlideShow(EntryData newEntry)
    {
        StopSlideShowCoroutine();
        LightController.Instance.Flash();
        StartCoroutine(WaitForFlashAndDisplayEntry(newEntry));
    }

    private IEnumerator WaitForFlashAndDisplayEntry(EntryData newEntry)
    {
        float flashDuration = LightController.Instance.flashDuration;

        // Wait for the halfway point of the flash
        yield return new WaitForSeconds(flashDuration / 2);

        // Update the display to the new entry
        UpdateEntryDisplay(newEntry);

        yield return new WaitForSeconds((flashDuration / 2) + displayDuration);

        // Restart the slideshow after the new entry has been displayed
        RestartSlideshow();
        GameManager.Instance.ResetGame();
    }

    void OnDestroy()
    {
        StopSlideShowCoroutine();
    }
}
