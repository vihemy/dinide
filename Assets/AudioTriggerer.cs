using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerer : MonoBehaviour
{
    public void PlayAudio(string name)
    {
        AudioManager.Instance.PlayOneShot(name);
    }
}
