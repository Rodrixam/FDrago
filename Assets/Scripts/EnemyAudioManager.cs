using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] float pitchVariation;

    AudioSource audioSource;
    [SerializeField] AudioClip battlecry;
    [SerializeField] AudioClip step;

    bool walking;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        walking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangePitch();
        audioSource.clip = battlecry;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (!walking)
            {
                walking = true;
                audioSource.clip = step;
            }

            ChangePitch();
            audioSource.Play();
        }
    }

    void ChangePitch()
    {
        audioSource.pitch = 1 + pitchVariation * (Random.value - 0.5f);
    }

}
