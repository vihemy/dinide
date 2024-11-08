using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class LightData
{
    public SpriteRenderer spriteRenderer;
    public float minIntensity = 0.0f;
    public float maxIntensity = 1.0f;
}

public class LightController : Singleton<LightController>
{
    public LightData[] lightsData;
    [Header("Flash")]
    public float intensityFactor = 1.0f; // Factor to scale the intensity of all lights
    [SerializeField] public float flashDuration = 0.5f;
    public float flashDelay = 1.0f;

    private void Start()
    {
        ResetLightsAlpha();
    }

    private void ResetLightsAlpha()
    {
        for (int i = 0; i < lightsData.Length; i++)
        {
            var lightData = lightsData[i];
            Color spriteColor = lightData.spriteRenderer.color;
            spriteColor.a = 0;
            lightData.spriteRenderer.color = spriteColor;
        }
    }

    public void Flash()
    {

        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        AudioManager.Instance.PlayOneShot("FlashBuild");
        // First half of the flash: increasing values
        float timer = 0.0f;
        while (timer < flashDuration / 2)
        {
            timer += Time.deltaTime;
            float lerpFactor = timer / (flashDuration / 2);

            UpdateSpriteAlpha(lerpFactor);

            yield return null;
        }

        AudioManager.Instance.PlayOneShot("FlashPayoff");

        // Second half of the flash: decreasing values
        timer = 0.0f;
        while (timer < flashDuration / 2)
        {
            timer += Time.deltaTime;
            float lerpFactor = 1 - (timer / (flashDuration / 2));

            UpdateSpriteAlpha(lerpFactor);

            yield return null;
        }
    }

    private void UpdateSpriteAlpha(float lerpFactor)
    {
        for (int i = 0; i < lightsData.Length; i++)
        {
            var lightData = lightsData[i];
            float alpha = Mathf.Lerp(lightData.minIntensity, lightData.maxIntensity, lerpFactor);
            Color spriteColor = lightData.spriteRenderer.color;
            spriteColor.a = alpha;
            lightData.spriteRenderer.color = spriteColor;
        }
    }
}
