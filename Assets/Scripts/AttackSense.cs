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
    public void FrameFreeze(int pauseFrame) {
        StartCoroutine(PauseOnAttack(pauseFrame));
    }
    IEnumerator PauseOnAttack(int pauseFrame) {
        float pauseTime = pauseFrame / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }
    public void ShakeScreen(float strength, float duration) {
        if (!isShake) {
            StartCoroutine(ShakeCamera(strength, duration));
        }
    }
    IEnumerator ShakeCamera(float strength, float duration) {
        isShake = true;
        Transform camera = Camera.main.transform;
        Vector3 startPosition = camera.position;
        while (duration > 0) {
            camera.position = Random.insideUnitSphere * strength + startPosition;
            duration -= Time.deltaTime;
            yield return null;
        }
        camera.position = startPosition;
        isShake = false;
    }
}
