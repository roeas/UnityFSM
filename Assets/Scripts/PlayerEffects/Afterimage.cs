using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    public float activeTime;
    [Range(0f, 1f)]
    public float startAlpha;

    private Transform player;
    private SpriteRenderer thisSR, playerSR;
    private float alpha, startTime;

    private void OnEnable() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSR = GetComponent<SpriteRenderer>();
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = startAlpha;
        startTime = Time.time;

        thisSR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        StartCoroutine(DecreaseAlpha());
    }
    void Update() {
        thisSR.color = new Color(0.5f, 0.5f, 1, alpha);
        if (Time.time - startTime >= activeTime) {
            AfterimagePool.instance.ReturnToPool(gameObject);
        }
    }
    private IEnumerator DecreaseAlpha() {
        while (alpha > 0) {
            //alpha需要在activeTime秒（activeTime * 50次FixedUpdate）内从startAlpha减为0
            alpha -= (startAlpha / (activeTime * 50));
            yield return new WaitForFixedUpdate();
        }
    }
}
