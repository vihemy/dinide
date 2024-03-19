using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderWatcher : MonoBehaviour
{
    private Slider targetSlider; // The Slider component to watch.
    public float threshold = 0.1f; // Threshold for slider value change.
    private float lastSliderValue;
    private AudioManager audioManager;
    [SerializeField] private string soundName;

    void Start()
    {
        audioManager = AudioManager.Instance;
        targetSlider = GetComponent<Slider>();
        if (targetSlider == null)
        {
            Debug.LogError("SliderWatcher: No target Slider assigned!");
            return;
        }

        // Initialize lastSliderValue with the current value of the Slider.
        lastSliderValue = targetSlider.value;
    }

    void Update()
    {
        float currentSliderValue = targetSlider.value;

        // Check if the slider value difference exceeds the threshold.
        if (Mathf.Abs(currentSliderValue - lastSliderValue) > threshold)
        {
            // Call the method to handle the threshold exceeded.
            OnThresholdExceeded();

            // Update lastSliderValue to the new slider value after calling the method.
            lastSliderValue = currentSliderValue;
        }
    }

    private void OnThresholdExceeded()
    {
        audioManager.PlayOneShot(soundName);
    }
}
