using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip jumpClip;
    public AudioClip landingClip;
    public AudioClip[] footStepClips;

    private AudioSource jumpSource;
    private AudioSource landingSource;
    private AudioSource footStepSource;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
        jumpSource = gameObject.AddComponent<AudioSource>();
        landingSource = gameObject.AddComponent<AudioSource>();
        footStepSource = gameObject.AddComponent<AudioSource>();
    }
    public static void PlayJumpClip() {
        instance.jumpSource.clip = instance.jumpClip;
        instance.jumpSource.Play();
    }
    public static void PlayLandingClip() {
        instance.landingSource.clip = instance.landingClip;
        instance.landingSource.Play();
    }
    public static void PlayFootStepClip() {
        instance.footStepSource.clip = instance.footStepClips[Random.Range(0, instance.footStepClips.Length)];
        instance.footStepSource.Play();
    }
}
