using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Transform backPoint;
    private Vector2 hurtDirection;
    private AnimatorStateInfo animatorState;
    private Animator animator;
    private Rigidbody2D body;
    private int HurtID;
    private bool isHurt;

    void Awake() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        HurtID = Animator.StringToHash("Hurt");
    }
    void Update() {
        animatorState = animator.GetCurrentAnimatorStateInfo(0);
        if (isHurt) {
            body.velocity = hurtDirection * speed;
            if (animatorState.normalizedTime > 0.6f) {
                isHurt = false;
            }
        }
    }
    public void Hurt(Vector2 directionIn) {
        transform.localScale = new Vector3(-directionIn.x, 1, 1);
        isHurt = true;
        this.hurtDirection = directionIn;
        animator.SetTrigger(HurtID);
    }
}
