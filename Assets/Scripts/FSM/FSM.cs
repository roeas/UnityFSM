using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType {
    Idle, Chase, Attack, Hurt, Throw, Run, SpecialAttack
}
public class FSM : MonoBehaviour
{
    public RogueSO rogue;
    //以下参数在每个Stste中调用
    [HideInInspector] public List<Collider2D> playerColldiers;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Collider2D rogueCollider;
    [HideInInspector] public Collider2D playerLAArea;
    [HideInInspector] public Collider2D playerHAArea;
    [HideInInspector] public Collider2D attackLenth;
    [HideInInspector] public Collider2D throwLenth;
    [HideInInspector] public GameObject shuriken;
    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public float idleStartTime;
    [HideInInspector] public float idleWaitTime;

    private State currentState;
    private Dictionary<StateType, State> stateLise = new Dictionary<StateType, State>();
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLAArea = player.transform.Find("LightAttackArea").GetComponent<Collider2D>();
        playerHAArea = player.transform.Find("HeavyAttackArea").GetComponent<Collider2D>();
        playerColldiers.Add(player.GetComponents<Collider2D>()[0]);
        playerColldiers.Add(player.GetComponents<Collider2D>()[1]);
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rogueCollider = GetComponent<Collider2D>();
        attackLenth = transform.Find("AttackLenth").GetComponent<Collider2D>();
        throwLenth = transform.Find("ThrowLenth").GetComponent<Collider2D>();
        idleWaitTime = 1f;
        //注册状态并将FSM自身传入状态
        stateLise.Add(StateType.Idle, new IdleState(this));
        stateLise.Add(StateType.Attack, new AttackState(this));
        stateLise.Add(StateType.Hurt, new HurtState(this));
        stateLise.Add(StateType.Throw, new ThrowState(this));
        stateLise.Add(StateType.Run, new RunState(this));
        stateLise.Add(StateType.SpecialAttack, new SpecialAttackState(this));
        ChangeState(StateType.Idle);
    }
    private void Update() {
        FlipTo(player.transform);
        currentState.OnUpdate();
    }
    private void FixedUpdate() {
        currentState.OnFixedUpdate();
    }
    public void ChangeState(StateType type) {
        if (currentState != null) {
            currentState.OnExit();
        }
        currentState = stateLise[type];
        currentState.OnEnter();
    }
    //一些通用的方法
    public void FlipTo(Transform target) {
        if (target != null) {
            transform.localScale = target.position.x >= transform.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }
    }
    public void OnSpecialAttack() {//在SpecialAttack动画中调用
        transform.position = player.transform.position;
    }
}
