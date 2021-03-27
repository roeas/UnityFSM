using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [HideInInspector]
    public Transform originalParent;
    [HideInInspector]
    public Vector2 originalPosition;
    private void OnEnable() {
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
    }
}
