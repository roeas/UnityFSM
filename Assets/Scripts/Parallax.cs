using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public new Transform camera;
    public float moveRate;
    public bool lockY;

    private float startX, startY;
    void Start() {
        startX = transform.position.x;
        startY = transform.position.y;
    }
    void Update() {
        if (lockY) {
            transform.position = new Vector2(startX + camera.position.x * moveRate, transform.position.y);
        }
        else {
            transform.position = new Vector2(startX + camera.position.x * moveRate, startY + camera.position.y * moveRate);
        }
    }
}
