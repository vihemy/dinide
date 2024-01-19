using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        InstantiateEntrySystem();
    }

    void InstantiateEntrySystem()
    {
        SlideshowController.Instance.InstantiateSlideshow(); // slideshow needs to be instantiated before entries are loaded
        EntryLoader.Instance.LoadLatestEntries();
    }
}
