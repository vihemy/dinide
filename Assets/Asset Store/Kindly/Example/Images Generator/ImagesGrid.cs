using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADD;
using System;
using UnityEngine.UI;

public class ImagesGrid : MonoBehaviour
{
    public GameObject _imagePrefab;

    void OnEnable()
    {
        Kindly.Instance.OnImagesReceived += SetupGrid;
    }

    private void OnDisable()
    {
        if (!Kindly.Quitting)
            Kindly.Instance.OnImagesReceived -= SetupGrid;
    }

    private void SetupGrid()
    {
        // clear previous images
        foreach (RawImage img in GetComponentsInChildren<RawImage>())
        {
            Destroy(img.gameObject);
        }

        // get generator
        Kindly gen = Kindly.Instance;

        // spawn images
        Array.ForEach(gen._generations, generation =>
        {
            GameObject go = Instantiate(_imagePrefab, transform);
            RawImage img = go.GetComponent<RawImage>();
            Kindly.Instance.DownloadImage(generation.image.url, img);
        });
    }
}
