using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    public HurtState(FSM fsmIn) : base(fsmIn) {

    }
    public override void OnEnter() {
        fsm.animator.Play("Rogue_Hurt");
    }
    public override void OnUpdate() {
        animeInfo = fsm.animator.GetCurrentAnimatorStateInfo(0);
        if (animeInfo.normalizedTime >= 0.99f) {
            fsm.ChangeState(StateType.Idle);
        }
    }
    public override void OnExit() {

    }
}
