using System.Collections;
using UnityEngine;
using TMPro;

public class SlideshowController : Singleton<SlideshowController>
{
    [Header("UI Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI authorInfoText;
    [Header("Configuration")]
    [SerializeField] private float standardDisplayDuration = 5f;
    [SerializeField] private float newEntryDisplayDuration = 10f;
    [SerializeField] private float fadeDuration = 1f;

    private int currentIndex = 0;
    private Coroutine slideshowCoroutine;
    private bool isDisplayingNewEntry = false;

    public void StartSlideshow()
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
            yield return DisplayEntry(EntryCache.Instance.entries[currentIndex], standardDisplayDuration, true);
            MoveToNextEntry();
        }
    }

    private void MoveToNextEntry()
    {
        currentIndex = (currentIndex + 1) % EntryCache.Instance.entries.Count;
    }

    private IEnumerator DisplayEntry(EntryData entry, float displayDuration, bool useFadeIn)
    {
        // Update the entry display before starting the fade-in
        UpdateEntryDisplay(entry);

        // Fade in the new entry if required
        if (useFadeIn)
        {
            yield return Fade(1f, fadeDuration);
        }

        else
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        // Keep the entry displayed for the specified duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out the entry at the end of its display period if required
        yield return Fade(0f, fadeDuration);
    }

    private IEnumerator Fade(float targetAlpha, float duration)
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

    private void UpdateEntryDisplay(EntryData entry)
    {
        spriteRenderer.sprite = Sprite.Create(entry.texture, new Rect(0, 0, entry.texture.width, entry.texture.height), new Vector2(0.5f, 0.5f));
        promptText.text = entry.prompt;
        authorInfoText.text = entry.author + ", " + entry.age;
    }

    public void DisplayNewEntryAndRestartSlideshow(EntryData newEntry)
    {
        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
        }
        isDisplayingNewEntry = true;
        StartCoroutine(DisplayNewEntry(newEntry));
    }

    private IEnumerator DisplayNewEntry(EntryData newEntry)
    {
        LightController.Instance.Flash(); // Invoke the flash method
        float flashDuration = LightController.Instance.flashDuration;
        yield return new WaitForSeconds(flashDuration / 2);

        // Display the new entry for its unique duration, without fade
        yield return DisplayEntry(newEntry, newEntryDisplayDuration, false);

        isDisplayingNewEntry = false;
        // Restart the slideshow after the new entry has been displayed
        StartSlideshow();
        GameManager.Instance.ResetGame();
    }

    void OnDestroy()
    {
        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
        }
    }
}
