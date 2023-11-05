using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : SingletonMonoBehaviour<AudioController>
{
    public AudioSource[] audioSources;

    [SerializeField] private int currentFocus = 0;

    public float transitionDuration = 1;

    public float normalVolume = 0.5f;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void InstantFocus(int numFocus)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if(i == numFocus)
            {
                audioSources[i].volume = normalVolume;
            }
            else
            {
                audioSources[i].volume = 0;
            }
        }
    }

    /// Set the audio source Focus
    // 0 is normal, 1 is flat
    public void SetFocus(int numFocus)
    {
        if (currentFocus == numFocus)
            return;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if(i == numFocus)
            {
                StartCoroutine(AudioTransition(audioSources[i], 0, normalVolume));
            }
            else
            {
                StartCoroutine(AudioTransition(audioSources[i], normalVolume, 0));
            }
        }
        currentFocus = numFocus;

    }

    [ContextMenu("Set Focus 0")]
    void Focus0()
    {
        SetFocus(0);
    }

    [ContextMenu("Set Focus 1")]
    void Focus1()
    {
        SetFocus(1);
    }

    IEnumerator AudioTransition(AudioSource aSource, float startVolume, float endVolume)
    {

        for (float i = 0; i <= 1; i+= 1f/ transitionDuration*Time.deltaTime)
        {

            aSource.volume = Mathf.Lerp(startVolume, endVolume, i);

            yield return null;
        }

        yield return null;
        aSource.volume = endVolume;

    }
}
