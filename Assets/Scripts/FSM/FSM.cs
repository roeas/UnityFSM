using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType {
    Idle, Chase, Attack, Hurt, Throw
}
public class FSM : MonoBehaviour
{
    public RogueSO rogue;
    [HideInInspector] public List<Collider2D> playerColldiers;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Collider2D rogueCollider;
    [HideInInspector] public Collider2D playerLAArea;
    [HideInInspector] public Collider2D playerHAArea;
    [HideInInspector] public Collider2D attackLenth;

    private State currentState;
    private Dictionary<StateType, State> stateLise = new Dictionary<StateType, State>();
    void Awake() {
        stateLise.Add(StateType.Idle, new IdleState(this));
        stateLise.Add(StateType.Attack, new AttackState(this));
        stateLise.Add(StateType.Hurt, new HurtState(this));
        stateLise.Add(StateType.Throw, new ThrowState(this));
        ChangeState(StateType.Idle);

        player = GameObject.FindGameObjectWithTag("Player");
        playerLAArea = player.transform.Find("LightAttackArea").GetComponent<Collider2D>();
        playerHAArea = player.transform.Find("HeavyAttackArea").GetComponent<Collider2D>();
        playerColldiers.Add(player.GetComponents<Collider2D>()[0]);
        playerColldiers.Add(player.GetComponents<Collider2D>()[1]);

        animator = GetComponent<Animator>();
        rogueCollider = GetComponent<Collider2D>();
        attackLenth = transform.Find("AttackLenth").GetComponent<Collider2D>();
    }
    void Update() {
        FlipTo(player.transform);
        currentState.OnUpdate();
        
    }
    public void ChangeState(StateType type) {
        if (currentState != null) {
            currentState.OnExit();
        }
        currentState = stateLise[type];
        currentState.OnEnter();
    }
    public void FlipTo(Transform target) {
        if (target != null) {
            if (target.position.x >= transform.position.x) {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
