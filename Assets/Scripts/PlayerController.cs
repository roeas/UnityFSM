using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, jumpForce;
    public LayerMask ground;
    public Collider2D usualCollider, crouchCollider;
    public Transform footPoint;
    public int finalJumpCount;

    private Rigidbody2D body;
    private Animator animator;
    private int jumpCount;
    private int runningID, isRunId, jumpId, jumpingID, isIdleID;
    private bool isHurt = false, jumpPressed = false;
    void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        runningID = Animator.StringToHash("Running");
        isRunId = Animator.StringToHash("isRun");
        isIdleID = Animator.StringToHash("isIdle");
        jumpId = Animator.StringToHash("Jump");
        jumpingID = Animator.StringToHash("Jumping");
    }
    void Update() {
        if (Input.GetButtonDown("Jump") && jumpCount > 0) {
            jumpPressed = true;
        }
        Animation();
    }
    void FixedUpdate() {
        Movement();
    }
    private void Movement() {
        float move = Input.GetAxisRaw("Horizontal");//-1, 0, 1
        body.velocity = new Vector2(move * speed * Time.fixedDeltaTime, body.velocity.y);//移动
        if (move != 0) {
            transform.localScale = new Vector3(move, 1, 1);//转身
        }
        if (Physics2D.OverlapCircle(footPoint.position, 0.1f, ground) && body.velocity.y <= 0) {
            //落地时重置跳跃相关的参数，而且要避免刚起跳时OverlapCircle检测到地面
            jumpCount = finalJumpCount;
        }
        if (jumpPressed && jumpCount > 0) {//跳跃
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetTrigger(jumpId);
            jumpCount--;
            jumpPressed = false;
        }
    }
    private void Animation() {
        float flMove = Input.GetAxis("Horizontal");//-1f ~ 1f
        float ySpeed = body.velocity.y;
        if (Input.GetAxisRaw("Horizontal") != 0) {
            animator.SetBool(isRunId, true);
        }
        else {
            animator.SetBool(isRunId, false);
        }
        animator.SetFloat(runningID, Mathf.Abs(flMove));
        animator.SetFloat(jumpingID, ySpeed);
        animator.SetBool(isIdleID, Physics2D.OverlapCircle(footPoint.position, 0.1f, ground));
        //Debug.Log(flMove);
    }
}
