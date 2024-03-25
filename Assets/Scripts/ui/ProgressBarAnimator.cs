using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMeshPro
using UltimateClean;
using System.Collections;

public class ProgressBarAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SlicedFilledImage fillImage;
    [SerializeField] private TextMeshProUGUI percentageText; // TextMeshPro UI component
    [SerializeField] private string animationStateName;
    [SerializeField, Range(0f, 5f)] private float stutterChance = 0.05f; // Chance of a stutter occurring
    [SerializeField, Range(0f, 0.5f)] private float maxStutterAmount = 0.05f; // Maximum amount of stutter

    private float lastFillAmount = 0f;

    // Update is called once per frame
    void Update()
    {
        if (animator == null || fillImage == null || percentageText == null)
            return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if we are currently in the specified animation state
        if (stateInfo.IsName(animationStateName))
        {
            // Update the fill amount based on the normalized time of the animation
            float fillAmount = stateInfo.normalizedTime % 1;
            fillAmount = ApplyStutterEffect(fillAmount);
            fillImage.fillAmount = fillAmount;

            // Update the percentage text
            int percentage = Mathf.RoundToInt(fillAmount * 100); // Calculate the percentage
            percentageText.text = percentage.ToString() + "%"; // Set the text
        }
    }

    private float ApplyStutterEffect(float currentFillAmount)
    {
        // Check if a stutter should occur
        if (Random.value < stutterChance && currentFillAmount > lastFillAmount)
        {
            // Apply a random stutter within the specified range
            currentFillAmount = Mathf.Max(lastFillAmount, currentFillAmount - Random.Range(0f, maxStutterAmount));
        }

        lastFillAmount = currentFillAmount;
        return currentFillAmount;
    }
}
