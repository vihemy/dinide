using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerer : MonoBehaviour
{
    public void PlayOneshot(string name)
    {
        AudioManager.Instance.PlayOneShot(name);
    }

    public void PlayLoop(string name)
    {
        AudioManager.Instance.Play(name);
    }

    public void PlayWithRandomPitch(string name)
    {
        AudioManager.Instance.PlayWithRandomPitch(name);
    }
}
