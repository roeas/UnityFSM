using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundLayer, enemyLayer;
    public float speed, jumpForce;
    [Header("打击感")]
    public int lightPauseFrame;
    public int heavyPauseFrame;
    [Header("冲刺")]
    public float dashSpeed;
    public float dashTime;
    public float slamSpeed;

    private Rigidbody2D body;
    private Animator animator;
    private Collider2D usualCollider, crouchCollider;
    private Transform footPoint, frontPoint;
    private int jumpCount, finalJumpCount = 2;
    private bool jumpPressed = false;
    private int runningID, isRunId, jumpId, jumpingID, isIdleID, lightAttackID, haveyAttackID, comboID, isAttackID, isCrouchID, isDashID;
    //isAttack在animator中为bool型，作为防止攻击动画被其他动画打断的标记符

    private int finalMaxCombo = 2, combo = 0;
    private float timeCount, interval = 2f;
    private bool uponGround, isAttack = false, isSlam = false, isCrouch = false, isdash = false, canDash = true;
    //isAttack用于限制普通攻击时的移动输入，为了保证手感与动画的流畅，它晚于animator中的isAttack重置。isSlamAttack用于限制下落攻击后摇时的跳跃。
    //当isDash用于触发冲刺状态，为真时会屏蔽所有移动相关的输入，canDash用于限制Dashing的触发频率，晚于isDash重置。
    private string attackName;
    void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        usualCollider = GetComponents<BoxCollider2D>()[0];
        crouchCollider = GetComponents<BoxCollider2D>()[1];
        footPoint = transform.Find("FootPoint");
        frontPoint = transform.Find("FrontPoint");

        runningID = Animator.StringToHash("Running");
        isRunId = Animator.StringToHash("isRun");
        isIdleID = Animator.StringToHash("isIdle");
        jumpId = Animator.StringToHash("Jump");
        jumpingID = Animator.StringToHash("Jumping");
        lightAttackID = Animator.StringToHash("LightAttack");
        haveyAttackID = Animator.StringToHash("HaveyAttack");
        comboID = Animator.StringToHash("Combo");
        isAttackID = Animator.StringToHash("isAttack");
        isCrouchID = Animator.StringToHash("isCrouch");
        isDashID = Animator.StringToHash("isDash");
    }
    void Update() {
        uponGround = Physics2D.OverlapCircle(footPoint.position, 0.1f, groundLayer);
        GetInput();
        Animation();
        Attack();
        Crouch();
    }
    void FixedUpdate() {
        Movement();
    }
    private void GetInput() {//在Update中调用以敏感地接受输入
        if (Input.GetButtonDown("Jump") && jumpCount > 0 && !isSlam) {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (!isdash && canDash && !isSlam) {
                isdash = true;
                canDash = false;
                StartCoroutine(Dash(dashTime));
            }
        }
    }
    private IEnumerator Dash(float dashTime) {
        yield return new WaitForSeconds(dashTime);
        isdash = false;
        yield return new WaitForSeconds(0.5f);
        canDash = true;//冲刺结束后经过一个短暂的CD才允许再次冲刺
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {//与敌人交互、触发帧冻结
            if (transform.localScale.x > 0) {
                collision.GetComponent<Enemy>().Hurt(Vector2.right);
            }
            else {
                collision.GetComponent<Enemy>().Hurt(Vector2.left);
            }
            if (attackName == "Light") {
                AttackSense.instance.FrameFreeze(lightPauseFrame);
            }
            if (attackName == "Heavy") {
                AttackSense.instance.FrameFreeze(heavyPauseFrame);
            }
        }
    }
    private void Movement() {
        float move = Input.GetAxisRaw("Horizontal");//-1, 0, 1
        if (isdash) {//冲刺移动，最高优先级
            body.velocity = new Vector2(dashSpeed * transform.localScale.x, body.velocity.y);
            AfterimagePool.instance.TakeFromPool();
            return;
        }
        if (move != 0) {
            transform.localScale = new Vector3(move, 1, 1);//转身
        }
        if (!isAttack && !isCrouch && !isdash) {//普通状态下的移动
            body.velocity = new Vector2(move * speed * Time.fixedDeltaTime, body.velocity.y);
        }
        else {
            if (isSlam) {//下落攻击时向下冲刺
                body.velocity = new Vector2(0, -slamSpeed);
            }
            else if (isCrouch) {//下蹲状态下的移动
                body.velocity = new Vector2(move * speed * Time.fixedDeltaTime * 0.15f, body.velocity.y);
            }
            else {//所有不允许移动的情况，随时用Vector2.zero覆盖velocity以免发生预期外的高速移动
                body.velocity = Vector2.zero;
            }
        }
        if (uponGround && body.velocity.y <= 0) {
            jumpCount = finalJumpCount;//落地时重置跳跃相关的参数，而且要避免刚起跳时OverlapCircle检测到地面
        }
        if (jumpPressed && jumpCount > 0) {//落地攻击后摇时jumpPressed不会为true
            jumpPressed = false;
            isAttack = false;//立刻解锁移动让跳跃更加灵活
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetTrigger(jumpId);
            jumpCount--;
        }
    }
    private void Crouch() {
        if (Input.GetButtonDown("Crouch")) {
            isCrouch = true;
            animator.SetBool(isCrouchID, true);
            usualCollider.enabled = false;
            crouchCollider.enabled = true;
        }
        if (Input.GetButtonUp("Crouch")) {
            isCrouch = false;
            animator.SetBool(isCrouchID, false);
            crouchCollider.enabled = false;
            usualCollider.enabled = true;
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
        animator.SetBool(isIdleID, uponGround);
        animator.SetBool(isDashID, isdash);
    }

    private Coroutine tmpCoroutine = null;//仅在Attack中使用
    private void Attack() {
        if (!animator.GetBool(isAttackID)) {
            if(uponGround) {
                if (Input.GetMouseButton(0)) {//左键轻攻击
                    if (tmpCoroutine != null) {
                        StopCoroutine(tmpCoroutine);
                    }
                    tmpCoroutine = StartCoroutine(WaitForAttackOver(4f / 14f));
                    //关闭协程确保这段时间内isAttack不会被设为false，启动协程倒计时使isAttack为false
                    body.velocity = Vector2.zero;
                    isAttack = true;
                    timeCount = interval;
                    combo++;
                    if (combo > finalMaxCombo) {//combo为1时播放一段攻击动画，为2时播放二段攻击动画
                        combo = 1;
                    }
                    animator.SetBool(isAttackID, true);
                    animator.SetTrigger(lightAttackID);
                    body.MovePosition(frontPoint.position);
                    animator.SetInteger(comboID, combo);
                    attackName = "Light";
                }
                if (Input.GetMouseButton(1)) {//右键重攻击
                    if (tmpCoroutine != null) {
                        StopCoroutine(tmpCoroutine);
                    }
                    tmpCoroutine = StartCoroutine(WaitForAttackOver(6f / 14f));
                    body.velocity = Vector2.zero;
                    isAttack = true;
                    animator.SetBool(isAttackID, true);
                    animator.SetTrigger(haveyAttackID);
                    attackName = "Heavy";
                }
            }
            else {//在空中
                if (Input.GetMouseButton(0)) {//空中攻击
                    animator.SetBool(isAttackID, true);
                    animator.SetTrigger(lightAttackID);
                    attackName = "Light";
                }
                if (Input.GetMouseButton(1)) {//下落攻击
                    if (tmpCoroutine != null) {
                        StopCoroutine(tmpCoroutine);
                    }
                    tmpCoroutine = StartCoroutine(WaitForAttackOver(6f / 14f));
                    body.velocity = Vector2.zero;
                    isAttack = true;
                    isSlam = true;
                    animator.SetBool(isAttackID, true);
                    animator.SetTrigger(haveyAttackID);
                    attackName = "";
                }
            }
        }//if (!animator.GetBool(isAttackID))
        if (timeCount != 0) {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0) {
                timeCount = 2f;
                combo = 0;
            }
        }
    }
    private IEnumerator WaitForAttackOver(float time) {
        yield return new WaitForSeconds(time);
        isAttack = false;
        isSlam = false;
    }
    public void AttackOver() {//在每个攻击动画快结束时调用
        animator.SetBool(isAttackID, false);
    }
}
