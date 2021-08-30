using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSense : MonoBehaviour
{
    public static AttackSense instance;

    private bool isShake;
    private void Awake() {
        instance = this;
    }
    public void FrameFreeze(int pauseFrame) {//在PlayerControler中调用
        StartCoroutine(PauseOnAttack(pauseFrame));
    }
    IEnumerator PauseOnAttack(int pauseFrame) {
        float pauseTime = pauseFrame / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }
    //屏幕抖动的功能由Cinemachine实现
}
