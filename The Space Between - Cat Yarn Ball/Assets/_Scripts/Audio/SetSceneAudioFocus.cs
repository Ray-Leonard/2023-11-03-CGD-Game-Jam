using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSceneAudioFocus : MonoBehaviour
{
    public int audioFocus;
    public bool gradualChange = true;
    void Start()
    {
        if(gradualChange)
            AudioController.Instance?.SetFocus(audioFocus);
        else
            AudioController.Instance?.InstantFocus(audioFocus);
        Destroy(gameObject);
    }
}


