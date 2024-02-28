using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UltimateClean;
public class ButtonListener : MonoBehaviour
{
    private CleanButton button => GetComponent<CleanButton>();
    [SerializeField] private Image glow;
    [SerializeField] private float glowMaxAlpha = 1f;
    [SerializeField] private float glowSpeed = 1f;

    void Start()
    { // used instead of inspector On Click event for better searchability in vs code
        button.onClick.AddListener(() =>
        {
            OnButtonPress();
        });

        MakeInteractable(false);
    }

    public void OnButtonPress()
    {

        GameManager.Instance.SubmitIdea();
    }

    public void MakeInteractable(bool state)
    {
        button.interactable = state;

        if (state)
        {
            StartCoroutine(AnimateGlow());
        }
        else
        {
            StopAllCoroutines();
            glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, 0f);
        }
    }

    IEnumerator AnimateGlow()
    {
        glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, 0);
        while (true)
        {
            float t = Mathf.PingPong(Time.time * glowSpeed, 1f);
            float alpha = Mathf.Lerp(0, glowMaxAlpha, t);
            glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, alpha);
            yield return null;
        }
    }

}
