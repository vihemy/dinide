using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[System.Serializable]
public class LightData
{
    public Light2D light;
    public float minIntensity = 0.0f;
    public float maxIntensity = 1.0f;

}

public class LightController : Singleton<LightController>
{
    public LightData[] lightsData;

    [Header("SpriteRenderer")]
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    public float minAlpha = 0.0f; // Minimum alpha value for the sprite
    public float maxAlpha = 1.0f;
    [Header("Flash")]
    public float intensityFactor = 1.0f; // Factor to scale the intensity of all lights
    [SerializeField] private float flashDuration = 0.2f; // Timer to track the flashing

    public event Action OnFlashHalfway;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flash();
        }
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // First half of the flash: increasing values
        float timer = 0.0f;
        while (timer < flashDuration / 2)
        {
            timer += Time.deltaTime;
            float lerpFactor = timer / (flashDuration / 2);

            UpdateLightsAndSprite(lerpFactor);

            yield return null;
        }

        OnFlashHalfway?.Invoke();

        // Second half of the flash: decreasing values
        timer = 0.0f;
        while (timer < flashDuration / 2)
        {
            timer += Time.deltaTime;
            float lerpFactor = 1 - (timer / (flashDuration / 2));

            UpdateLightsAndSprite(lerpFactor);

            yield return null;
        }
    }

    private void UpdateLightsAndSprite(float lerpFactor)
    {
        foreach (var lightData in lightsData)
        {
            lightData.light.intensity = Mathf.Lerp(lightData.minIntensity, lightData.maxIntensity, lerpFactor);
        }

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, lerpFactor);
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = alpha;
        spriteRenderer.color = spriteColor;
    }
}