using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class FlameController : MonoBehaviour
{
    [Header("Heads")]
    public GameObject leftHead;
    public GameObject rightHead;
    ParticleSystem leftPs;
    ParticleSystem rightPs;
    Animator leftAnim;
    Animator rightAnim;

    [Header("Haptic impulses")]
    [SerializeField] HapticImpulsePlayer leftHaptic;
    [SerializeField] HapticImpulsePlayer rightHaptic;
    [SerializeField] float hapticImpulse;


    [Header("Audio")]
    [SerializeField] AudioSource leftRoarAudio;
    [SerializeField] AudioSource rightRoarAudio;
    [SerializeField] AudioSource leftFireAudio;
    [SerializeField] AudioSource rightFireAudio;
    [SerializeField] AudioSource boostAudio;
    [SerializeField] AudioSource exhaustAudio;


    [Header("Stamina")]
    [SerializeField] float maxStamina;
    [SerializeField] float currentStamina;
    bool exhausted;
    [SerializeField] CanvasGroup exhaustMask;

    [Header("Boost")]
    public bool boosted;
    [SerializeField] float boostTime;
    public int killsForBooost;
    float boostCounter;
    [SerializeField] ParticleSystem.MinMaxGradient normalFire;
    [SerializeField] ParticleSystem.MinMaxGradient boostedFire;

    [Header("Miscelaneous")]
    bool leftFlamesPlaying;
    bool rightFlamesPlaying;
    EnemyGenerator eg;

    private void Awake()
    {
        //Get references
        leftPs = leftHead.GetComponent<ParticleSystem>();
        rightPs = rightHead.GetComponent<ParticleSystem>();

        leftAnim = leftHead.GetComponent<Animator>();
        rightAnim = rightHead.GetComponent<Animator>();

        eg = GetComponent<EnemyGenerator>();

        //Set initial variables
        leftFlamesPlaying = false;
        rightFlamesPlaying = false;

        exhausted = false;
        currentStamina = maxStamina;

        DeactivateBoost();
        boostCounter = 0;
    }

    private void Update()
    {
        //Update currentStamina
        if (!boosted && leftFlamesPlaying)
        {
            currentStamina -= Time.deltaTime;
        }
        if (!boosted && rightFlamesPlaying)
        {
            currentStamina -= Time.deltaTime;
        }
        if(boosted || (!leftFlamesPlaying && !rightFlamesPlaying))
        {
            currentStamina += Time.deltaTime;
            if(currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                if (exhausted)
                {
                    exhausted = false;
                }
            }
        }
        SetMaskAlpha();

        //Stop flames if stamina depleted
        if(currentStamina <= 0)
        {
            exhausted = true;
            StopLeftFlames();
            StopRightFlames();
            exhaustAudio.Play();
        }

        //Boosted time counter
        if (boosted)
        {
            boostCounter += Time.deltaTime;
            if(boostCounter >= boostTime)
            {
                boostCounter = 0;
                DeactivateBoost();
            }
        }

        //Send haptic impulses
        if (leftFlamesPlaying)
        {
            leftHaptic.SendHapticImpulse(hapticImpulse, 0.1f);
        }
        if (rightFlamesPlaying)
        {
            rightHaptic.SendHapticImpulse(hapticImpulse, 0.1f);
        }
    }

    public void PlayLeftFlames()
    {
        if (exhausted || !eg.IsGenerationActive()) { return; }

        leftPs.Play();
        leftRoarAudio.Play();
        leftFireAudio.Play();
        leftAnim.SetBool("OnFire", true);
        leftFlamesPlaying = true;
    }
    public void PlayRightFlames()
    {
        if (exhausted || !eg.IsGenerationActive()) { return; }

        rightPs.Play();
        rightRoarAudio.Play();
        rightFireAudio.Play();
        rightAnim.SetBool("OnFire", true);
        rightFlamesPlaying = true;
    }

    public void StopLeftFlames()
    {
        leftPs.Stop();
        leftFireAudio.Stop();
        leftAnim.SetBool("OnFire", false);
        leftFlamesPlaying = false;
    }
    public void StopRightFlames()
    {
        rightPs.Stop();
        rightFireAudio.Stop();
        rightAnim.SetBool("OnFire", false);
        rightFlamesPlaying = false;
    }

    public void ActivateBoost()
    {
        ParticleSystem.ColorOverLifetimeModule colorLifetime;
        colorLifetime =  leftPs.colorOverLifetime;
        colorLifetime.color = boostedFire;
        colorLifetime = rightPs.colorOverLifetime;
        colorLifetime.color = boostedFire;

        boostAudio.Play();

        rightHaptic.SendHapticImpulse(1, 1f);
        leftHaptic.SendHapticImpulse(1, 1f);

        boosted = true;
    }
    void DeactivateBoost()
    {
        ParticleSystem.ColorOverLifetimeModule colorLifetime;
        colorLifetime = leftPs.colorOverLifetime;
        colorLifetime.color = normalFire;
        colorLifetime = rightPs.colorOverLifetime;
        colorLifetime.color = normalFire;

        boosted = false;
    }

    void SetMaskAlpha()
    {
        exhaustMask.alpha = 1 - currentStamina / maxStamina;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public int GetKillsForBoost()
    {
        return killsForBooost;
    }
    public bool IsFlameBoosted()
    {
        return boosted;
    }
}
