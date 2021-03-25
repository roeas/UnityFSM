using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform mainCamera;
    public float moveRate;
    public bool lockY;

    private float startX, startY;
    void Awake() {
        startX = transform.position.x;
        startY = transform.position.y;
    }
    void FixedUpdate() {
        if (lockY) {
            transform.position = new Vector2(startX + mainCamera.position.x * moveRate, transform.position.y);
        }
        else {
            transform.position = new Vector2(startX + mainCamera.position.x * moveRate, startY + mainCamera.position.y * moveRate);
        }
    }
}
