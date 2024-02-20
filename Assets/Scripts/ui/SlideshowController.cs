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

    void Start()
    {
        RestartSlideshow();
    }

    public void RestartSlideshow()
    {
        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
        }

        slideshowCoroutine = StartCoroutine(DisplaySlideshow());
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
        // Fade out current entry
        yield return StartCoroutine(FadeSprite(0f, fadeDuration / 2));
        // Update to new entry
        UpdateEntryDisplay(entry);
        // Fade in new entry
        yield return StartCoroutine(FadeSprite(1f, fadeDuration / 2));
        // Wait for display duration
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

    public void DisplayEntryAndDisplay(EntryData newEntry)
    {

        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
        }
        LightController.Instance.Flash();
        Wait(LightController.Instance.flashDuration / 2);

        currentIndex = EntryCache.Instance.entries.IndexOf(newEntry);

        StartCoroutine(DisplayEntry(newEntry));
        RestartSlideshow();
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
    void OnDestroy()
    {
        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
        }
    }
}
