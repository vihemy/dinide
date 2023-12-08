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
        entryTemplate = entryContainer.transform.Find("EntryTemplate");
        entryTemplate.gameObject.SetActive(false);
    }

    public void CreateEntry(Texture2D texture, string prompt)
    {
        Transform entryTransform = CreateEntryTransform();
        FillEntry(entryTransform, texture, prompt);
    }

    private Transform CreateEntryTransform()
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        entryTransform.gameObject.SetActive(true);
        return entryTransform;
    }

    private void FillEntry(Transform entryTransform, Texture2D texture, string prompt)
    {
        entryTransform.Find("Text").GetComponent<TextMeshProUGUI>().text = prompt;
        var rect = new Rect(0, 0, texture.width, texture.height);
        var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100);
        entryTransform.Find("Image").GetComponent<Image>().sprite = sprite;
    }

    [System.Serializable]
    public class EntryData
    {
        public string prompt;
        public string image;
    }
}
