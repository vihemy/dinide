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
        EntryLoader.Instance.LoadLatestEntries();
        SlideshowController.Instance.InstantiateSlideshow();
    }
}
