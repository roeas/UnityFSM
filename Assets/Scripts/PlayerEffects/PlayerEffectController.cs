using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    public void OnJump() {
        AudioManager.PlayJumpClip();
        PlayEffect("JumpDust");
    }
    public void OnLanding() {
        AudioManager.PlayLandingClip();
        PlayEffect("LandingDust");
    }
    public void OnRunSop() {
        PlayEffect("RunStopDust");
    }
    public void FootStepAudio() {
        AudioManager.PlayFootStepClip();
    }
    public void SheathSwordAudio() {
        AudioManager.PlaySheathSwordClip();
    }
    public void SwordSwooshAudio() {
        AudioManager.PlaySwordSwooshClip();
    }
    public void OnAirSlamLanding() {
        AudioManager.PlayAirSlamLandingClip();
        PlayEffect("AirSlamDust");
    }
    public void DrawSwordAudio() {
        AudioManager.PlayDrawSwordClip();
    }
    public void DashAudio() {
        AudioManager.PlayDashClip();
    }
    private void PlayEffect(string effectName) {
        GameObject effect = transform.Find("Effects").Find(effectName).gameObject;
        effect.SetActive(true);
        effect.transform.SetParent(null);
    }
}
