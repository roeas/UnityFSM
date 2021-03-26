using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("基本音效")]
    public AudioClip jumpClip;
    public AudioClip landingClip;
    public AudioClip[] footStepClips;
    [Header("攻击音效")]
    public AudioClip sheathSwordClip;
    public AudioClip swordSwooshClip;
    public AudioClip airSlamLandingClip;

    private AudioSource jumpSource;
    private AudioSource landingSource;
    private AudioSource footStepSource;
    private AudioSource sheathSwordSource;
    private AudioSource swordSwooshSource;
    private AudioSource airSlamLandingSource;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
        jumpSource = gameObject.AddComponent<AudioSource>();
        landingSource = gameObject.AddComponent<AudioSource>();
        footStepSource = gameObject.AddComponent<AudioSource>();
        sheathSwordSource = gameObject.AddComponent<AudioSource>();
        swordSwooshSource = gameObject.AddComponent<AudioSource>();
        airSlamLandingSource = gameObject.AddComponent<AudioSource>();
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
    public static void PlaySheathSwordClip() {
        instance.sheathSwordSource.clip = instance.sheathSwordClip;
        instance.sheathSwordSource.Play();
    }
    public static void PlaySwordSwooshClip() {
        instance.swordSwooshSource.clip = instance.swordSwooshClip;
        instance.swordSwooshSource.Play();
    }
    public static void PlayAirSlamLandingClip() {
        instance.airSlamLandingSource.clip = instance.airSlamLandingClip;
        instance.airSlamLandingSource.Play();
    }
}
