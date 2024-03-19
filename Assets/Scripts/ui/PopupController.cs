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
    [SerializeField] private PopupOpener requestErrorPopupOpener;
    [SerializeField] private PopupOpener networkErrorPopupOpener;
    [SerializeField] private PopupOpener unknownErrorPopupOpener;

    public void DisplayPopup(ErrorType type)
    {
        switch (type)
        {
            case ErrorType.EmptyField:
                emptyFieldPopupOpener.OpenPopup();
                break;
            case ErrorType.Profanity:
                profanityPopupOpener.OpenPopup();
                break;
            case ErrorType.ConnectionError:
                networkErrorPopupOpener.OpenPopup();
                break;
            case ErrorType.RequestError:
                requestErrorPopupOpener.OpenPopup();
                break;
            case ErrorType.UnknownError:
                unknownErrorPopupOpener.OpenPopup();
                break;
            default:
                break;
        }
    }
}

public enum ErrorType
{
    EmptyField,
    Profanity,
    ConnectionError,
    RequestError,
    UnknownError,
}