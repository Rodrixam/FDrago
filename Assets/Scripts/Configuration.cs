using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class Configuration : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Header("Audio")]
    float maxSoundsVolume = 1;
    [SerializeField] List<AudioSource> sounds;
    float maxMusicVolume = 1;
    [SerializeField] List<AudioSource> music;

    [Header("Debug Panel")]
    [SerializeField] GameObject debugPanel;

    [Header("Turning with stick")]
    [SerializeField] SnapTurnProvider turnProvider;

    public void SetGlobalVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetSoundsVolume(float value)
    {
        maxSoundsVolume = value;
        foreach (AudioSource audio in sounds)
        {
            audio.volume = maxSoundsVolume;
        }
    }
    public void SetMusicVolume(float value)
    {
        maxMusicVolume = value;
        foreach (AudioSource audio in music)
        {
            audio.volume = maxMusicVolume;
        }
    }

    public void StartFadeMusic()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeMusic(1));
    }


    private IEnumerator FadeMusic(float duration)
    {
        if(duration <= 0)
        {
            foreach (AudioSource audio in music)
            {
                audio.volume = 0;
            }

            yield break;
        }


        float elapsedTime = 0.0f;

        while (music[0].volume > 0)
        {
            foreach (AudioSource audio in music)
            {
                audio.volume = maxMusicVolume - elapsedTime * maxMusicVolume / duration;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    public void ToggleDebug(bool value)
    {
        debugPanel.SetActive(value);
    }
    public void ToggleTurn(bool value) 
    {
        turnProvider.enabled = value;
    }
}
