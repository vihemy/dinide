using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateClean;
using UnityEngine.UI;
using TMPro;

public class PopupController : Singleton<PopupController>
{
    [SerializeField] private PopupOpener profanityPopupOpener;
    [SerializeField] private PopupOpener emptyFieldPopupOpener;


    public void DisplayEmptyFieldPopup()
    {
        emptyFieldPopupOpener.OpenPopup();
    }

    public void DisplayProfanityPopup()
    {
        profanityPopupOpener.OpenPopup();
    }
}