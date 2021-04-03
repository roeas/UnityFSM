using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {
    public IdleState(FSM fsmIn) : base (fsmIn){

    }
    public override void OnEnter() {
        rogue.animator.Play("Rogue_Idle");
    }
    public override void OnUpdate() {

    }
    public override void OnExit() {

    }
}
