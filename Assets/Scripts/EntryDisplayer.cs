using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryDisplayer : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake()
    {
        entryContainer = this.transform;
        ReferenceAndHideTemplate();
    }

    private void ReferenceAndHideTemplate()
    {
        // entryTemplate = entryContainer.transform.Find("EntryTemplate");
        entryTemplate = entryContainer.transform.Find("EntryTemplate2");
        entryTemplate.gameObject.SetActive(false);
    }

    public void CreateAndDisplayEntry(EntryData entry)
    {
        Transform displayedEntryTransform = CreateAndDisplayEntryTransform();
        FillDisplayedEntry(displayedEntryTransform, entry.texture, entry.prompt);
    }

    private Transform CreateAndDisplayEntryTransform()
    {
        Transform displayedEntryTransform = Instantiate(entryTemplate, entryContainer);
        displayedEntryTransform.gameObject.SetActive(true);
        return displayedEntryTransform;
    }

    private void FillDisplayedEntry(Transform displayedEntryTransform, Texture2D texture, string prompt)
    {
        displayedEntryTransform.Find("Text").GetComponent<TextMeshProUGUI>().text = prompt;
        var rect = new Rect(0, 0, texture.width, texture.height);
        var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100);
        displayedEntryTransform.Find("Image").GetComponent<Image>().sprite = sprite;
    }

}
