using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button => GetComponent<Button>();
    void Start()
    { // used instead of inspector On Click event for better searchability in vs code
        button.onClick.AddListener(() =>
        {
            OnButtonPress();
        });
    }

    private void OnButtonPress()
    {
        AudioManager.Instance.PlayOneShot("ButtonPress");
        GameManager.Instance.StartGame();
    }
}
