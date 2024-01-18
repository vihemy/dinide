using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class EntryDisplayer : Singleton<EntryDisplayer>
{
    private Transform entryContainer;
    private Transform entryTemplate;

    new private void Awake()
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

    public void CreateEntryDisplay(EntryData entry)
    {
        Transform displayedEntryTransform = CreateEntryTransform();
        FillDisplayedEntry(displayedEntryTransform, entry);
    }

    private Transform CreateEntryTransform()
    {
        Transform displayedEntryTransform = Instantiate(entryTemplate, entryContainer);
        displayedEntryTransform.gameObject.SetActive(true);
        return displayedEntryTransform;
    }

    private void FillDisplayedEntry(Transform displayedEntryTransform, EntryData entry)
    {
        displayedEntryTransform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = entry.prompt;
        displayedEntryTransform.Find("Name, age").GetComponent<TextMeshProUGUI>().text = entry.author + ", " + entry.age;

        var rect = new Rect(0, 0, entry.texture.width, entry.texture.height);
        var sprite = Sprite.Create(entry.texture, rect, new Vector2(0.5f, 0.5f), 100);
        displayedEntryTransform.Find("Image").GetComponent<Image>().sprite = sprite;
    }

}
