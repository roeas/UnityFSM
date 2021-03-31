using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType {
    Idle, Chase, Attack
}
public class FSM : MonoBehaviour
{
    public RogueSO rogue;

    private State currentState;
    private Dictionary<StateType, State> stateLise = new Dictionary<StateType, State>();
    void Awake() {
        stateLise.Add(StateType.Idle, new IdleState(this));
        stateLise.Add(StateType.Idle, new AttackState(this));

        ChangeState(StateType.Idle);
        rogue.animator = GetComponent<Animator>();
    }
    void Update() {
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
