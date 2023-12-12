using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADD;
using System;
using UnityEngine.UI;

public class ModelsList : MonoBehaviour
{
    public GameObject _listItemPrefab;
    public Scrollbar _scrollbar;
    private List<GameObject> _listElements = new List<GameObject>();

    void OnEnable()
    {
        Kindly.Instance.OnModelsReceived += SetupList;
    }

    private void OnDisable()
    {
        if (!Kindly.Quitting)
            Kindly.Instance.OnModelsReceived -= SetupList;
    }

    private void SetupList()
    {
        // clear previous list
        foreach (GameObject go in _listElements)
        {
            Destroy(go);
        }

        // get generator
        Kindly gen = Kindly.Instance;

        // spawn items
        foreach(Model model in gen._models)
        {
            GameObject go = Instantiate(_listItemPrefab, transform);
            _listElements.Add(go);
            RawImage img = go.GetComponentInChildren<RawImage>();
            if (model.image_url.Length > 0) Kindly.Instance.DownloadImage(model.image_url, img);
            go.GetComponentsInChildren<Text>()[0].text = model.name;
            go.GetComponentsInChildren<Text>()[1].text = model.description;
            // setup model selection through onClick listener
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                FindObjectOfType<ImageGenerator>().SetModel(model, img.texture);
                // setup the triggers in the input field
                string triggersString = "";
                foreach (string trigger in model.triggers)
                    triggersString += trigger + " ";
                FindObjectOfType<InputField>().text = triggersString;
            });
        };
        // scroll to top
        _scrollbar.SetValueWithoutNotify(0f);
    }
}
