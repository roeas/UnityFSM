using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType {
    Idle, Chase, Attack
}
public class FSM : MonoBehaviour
{
    public RogueSO rogue;
    public GameObject player;
    public Collider2D attackLenth;
    public Animator animator;

    private State currentState;
    private Dictionary<StateType, State> stateLise = new Dictionary<StateType, State>();
    void Awake() {
        stateLise.Add(StateType.Idle, new IdleState(this));
        stateLise.Add(StateType.Attack, new AttackState(this));

        ChangeState(StateType.Idle);
        animator = GetComponent<Animator>();
    }
    void Update() {
        FlipTo(player.transform);
        currentState.OnUpdate();
        if (attackLenth.bounds.Intersects(player.GetComponents<Collider2D>()[0].bounds) || attackLenth.bounds.Intersects(player.GetComponents<Collider2D>()[1].bounds)) {
            ChangeState(StateType.Attack);
        }
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
