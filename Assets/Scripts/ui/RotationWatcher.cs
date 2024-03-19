using UnityEngine;

public class RotationWatcher : MonoBehaviour
{
    public float threshold = 10f; // The rotation threshold in degrees.
    private float lastZRotation;
    private AudioManager audioManager;
    [SerializeField] private string soundName;

    void Start()
    {
        // Initialize lastZRotation with the current Z-axis rotation of the GameObject.
        lastZRotation = transform.eulerAngles.z;
        audioManager = AudioManager.Instance;
    }

    void Update()
    {
        float currentZRotation = transform.eulerAngles.z;

        // Check if the rotation difference exceeds the threshold.
        if (Mathf.Abs(currentZRotation - lastZRotation) > threshold)
        {
            // Call the desired method.
            OnThresholdExceeded();

            // Update lastZRotation to the new rotation after calling the method.
            lastZRotation = currentZRotation;
        }
    }

    private void OnThresholdExceeded()
    {
        // Add any additional functionality here.
        audioManager.PlayOneShot(soundName);
    }
}
