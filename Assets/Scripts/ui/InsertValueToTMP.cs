using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InsertValueToTMP : MonoBehaviour
{
    public TextMeshProUGUI TMPObject;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("Slider component not found", this);
        }

        if (TMPObject == null)
        {
            Debug.LogError("TMPObject is not assigned", this);
        }
    }

    void OnSliderValueChanged(float value)
    {
        TMPObject.text = value.ToString();
    }

    void OnDestroy()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
