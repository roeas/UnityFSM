using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    private Transform originalParent;
    private Vector2 originalPosition;
    private void OnEnable() {
        originalPosition = transform.localPosition;
        originalParent = transform.parent;
    }
    public void EffectOver() {
        transform.parent = originalParent;
        transform.localPosition = originalPosition;
        gameObject.SetActive(false);
    }
}
