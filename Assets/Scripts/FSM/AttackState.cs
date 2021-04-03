using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State {
    private AnimatorStateInfo animeInfo;
    public AttackState(FSM fsmIn) : base(fsmIn) {

    }
    public override void OnEnter() {
        rogue.animator.Play("Rogue_Attack");
    }
    public override void OnUpdate() {
        animeInfo = rogue.animator.GetCurrentAnimatorStateInfo(0);
        if (animeInfo.normalizedTime >= 0.99f) {
            fsm.ChangeState(StateType.Idle);
        }
    }
    public override void OnExit() {

    }
}
