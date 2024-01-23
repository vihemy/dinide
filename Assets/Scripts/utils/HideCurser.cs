using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideCurser : MonoBehaviour
{
    [SerializeField] private bool isVisible;
    private void Awake()
    {
        Cursor.visible = isVisible;
    }

}
