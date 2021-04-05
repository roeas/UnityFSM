using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackState : State
{
    public SpecialAttackState(FSM fsmIn) : base(fsmIn) {

    }
    public override void OnEnter() {
        fsm.body.velocity = Vector2.zero;
        fsm.animator.Play("Rogue_SpecialAttack");
    }
    public override void OnUpdate() {
        animeInfo = fsm.animator.GetCurrentAnimatorStateInfo(0);
        if (animeInfo.normalizedTime >= 0.99f) {
            fsm.ChangeState(StateType.Idle);
        }
    }
    public override void OnFixedUpdate() {

    }
    public override void OnExit() {
        fsm.idleStartTime = Time.time;
    }
}
