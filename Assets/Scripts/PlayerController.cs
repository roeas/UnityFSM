using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform footPoint, frontPoint;
    public Collider2D usualCollider, crouchCollider;
    public LayerMask groundLayer, enemyLayer;
    public float speed, jumpForce, attackMove;
    public int finalJumpCount, finalMaxCombo;
    [Header("打击感")]
    public int lightPauseFrame;
    public int heavyPauseFrame;

    private Rigidbody2D body;
    private Animator animator;
    private int jumpCount;
    private bool jumpPressed = false;
    private int runningID, isRunId, jumpId, jumpingID, isIdleID, lightAttackID, haveyAttackID, comboID, isAttackID;
    //isAttack在animator中为bool型，作为防止攻击动画被其他动画打断的标记符

    private int combo = 0;
    private float timeCount, interval = 2f;
    private bool uponGround, isAttack = false, isSlamAttack = false, canPlayJumpEffect = true;
    //isAttack用于限制普通攻击时的移动输入，isSlamAttack用于限制下落攻击后摇时的跳跃，canPlayJumpEffect用于防止跳跃动画物件在初始化之前被调用
    private string attackName;
    void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        runningID = Animator.StringToHash("Running");
        isRunId = Animator.StringToHash("isRun");
        isIdleID = Animator.StringToHash("isIdle");
        jumpId = Animator.StringToHash("Jump");
        jumpingID = Animator.StringToHash("Jumping");
        lightAttackID = Animator.StringToHash("LightAttack");
        haveyAttackID = Animator.StringToHash("HaveyAttack");
        comboID = Animator.StringToHash("Combo");
        isAttackID = Animator.StringToHash("isAttack");
    }
    void Update() {
        uponGround = Physics2D.OverlapCircle(footPoint.position, 0.1f, groundLayer);
        if (Input.GetButtonDown("Jump") && jumpCount > 0 && !isSlamAttack) {
            jumpPressed = true;
        }
        Animation();
        Attack();
    }
    void FixedUpdate() {
        Movement();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
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
        if (!isAttack) {
            body.velocity = new Vector2(move * speed * Time.fixedDeltaTime, body.velocity.y);//非攻击状态下的移动
        }
        if (move != 0) {
            transform.localScale = new Vector3(move, 1, 1);//转身
        }
        if (body.velocity.y <= 0 && (uponGround)) {
            jumpCount = finalJumpCount;//落地时重置跳跃相关的参数，而且要避免刚起跳时OverlapCircle检测到地面
        }
        if (jumpPressed && jumpCount > 0 && !isSlamAttack) {//非落地攻击后摇内的跳跃
            jumpPressed = false;
            isAttack = false;
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetTrigger(jumpId);
            jumpCount--;
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
    }
    private Coroutine tmpCoroutine = null;
    private void Attack() {
        if (!animator.GetBool(isAttackID)) {
            if(uponGround) {
                if (Input.GetMouseButton(0)) {//左键轻攻击
                    if (tmpCoroutine != null) {
                        StopCoroutine(tmpCoroutine);
                    }
                    tmpCoroutine = StartCoroutine(setIsAttackFalse(4f / 14f));
                    //启动协程倒计时使isAttack为false，关闭协程确保这段时间内isAttack不会被设为false
                    body.velocity = Vector2.zero;
                    animator.SetBool(isAttackID, true);
                    isAttack = true;
                    combo++;
                    if (combo > finalMaxCombo) {//combo为1时进行一段攻击，为2时进行二段攻击
                        combo = 1;
                    }
                    timeCount = interval;
                    animator.SetTrigger(lightAttackID);
                    animator.SetInteger(comboID, combo);
                    body.MovePosition(frontPoint.position);
                    attackName = "Light";
                }
                if (Input.GetMouseButton(1)) {//右键重攻击
                    if (tmpCoroutine != null) {
                        StopCoroutine(tmpCoroutine);
                    }
                    tmpCoroutine = StartCoroutine(setIsAttackFalse(6f / 14f));
                    body.velocity = Vector2.zero;
                    animator.SetBool(isAttackID, true);
                    isAttack = true;
                    animator.SetTrigger(haveyAttackID);
                    attackName = "Heavy";
                }
            }
            else {
                if (Input.GetMouseButton(0)) {//空中攻击
                    animator.SetBool(isAttackID, true);
                    animator.SetTrigger(lightAttackID);
                    attackName = "Light";
                }
                if (Input.GetMouseButton(1)) {//下落攻击
                    if (tmpCoroutine != null) {
                        StopCoroutine(tmpCoroutine);
                    }
                    tmpCoroutine = StartCoroutine(setIsAttackFalse(9f / 14f));
                    body.velocity = Vector2.zero;
                    animator.SetBool(isAttackID, true);
                    isAttack = true;
                    isSlamAttack = true;
                    animator.SetTrigger(haveyAttackID);
                    attackName = "";
                }
            }
        }
        if (timeCount != 0) {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0) {
                timeCount = 2f;
                combo = 0;
            }
        }
    }
    IEnumerator setIsAttackFalse(float time) {
        yield return new WaitForSeconds(time);
        isAttack = false;
        isSlamAttack = false;
    }
    public void AttackOver() {//在每个攻击动画快结束时调用
        animator.SetBool(isAttackID, false);
    }

    public void OnJump() {
        AudioManager.PlayJumpClip();
        if (canPlayJumpEffect) {
            canPlayJumpEffect = false;
            GameObject effect = transform.Find("Effects").Find("JumpDust").gameObject;
            StartCoroutine(PlayEffect(effect, 3f / 14f));
        }
    }
    public void OnLanding() {
        AudioManager.PlayLandingClip();
        GameObject effect = transform.Find("Effects").Find("LandingDust").gameObject;
        StartCoroutine(PlayEffect(effect, 2f / 14f));
    }
    public void OnRunSop() {
        GameObject effect = transform.Find("Effects").Find("RunStopDust").gameObject;
        StartCoroutine(PlayEffect(effect, 3f / 14f));
    }
    public void FootStepAudio() {
        AudioManager.PlayFootStepClip();
    }
    public void SheathSwordAudio() {
        AudioManager.PlaySheathSwordClip();
    }
    public void SwordSwooshAudio() {
        AudioManager.PlaySwordSwooshClip();
    }
    public void OnAirSlamLanding() {
        AudioManager.PlayAirSlamLandingClip();
        GameObject effect = transform.Find("Effects").Find("AirSlamDust").gameObject;
        StartCoroutine(PlayEffect(effect, 5f / 14f));
    }
    public void DrawSwordAudio() {
        AudioManager.PlayDrawSwordClip();
    }
    private IEnumerator PlayEffect(GameObject effectIn, float time) {//在对应需要特效的动画中被调用
        effectIn.SetActive(true);
        effectIn.transform.SetParent(null);
        yield return new WaitForSecondsRealtime(time);//传入一个x/14，x即该动画所需帧数
        effectIn.transform.SetParent(effectIn.GetComponent<Effect>().originalParent);//初始化参数在Effect脚本中的OnEnable中被赋值
        effectIn.transform.localPosition = effectIn.GetComponent<Effect>().originalPosition;
        effectIn.SetActive(false);
        canPlayJumpEffect = true;
    }
}
