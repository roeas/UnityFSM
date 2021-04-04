using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {
    private float startTime;
    private float waiteTime;
    public IdleState(FSM fsmIn) : base (fsmIn){

    }
    public override void OnEnter() {
        fsm.animator.Play("Rogue_Idle");
        startTime = Time.time;
        waiteTime = Random.Range(0, 4);
    }
    public override void OnUpdate() {
        if(fsm.playerLAArea.bounds.Intersects(fsm.rogueCollider.bounds) || fsm.playerHAArea.bounds.Intersects(fsm.rogueCollider.bounds)) {
            fsm.ChangeState(StateType.Hurt);
        }
        if (Time.time - startTime >= waiteTime) {
            if (fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[0].bounds) || fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[1].bounds)) {
                fsm.ChangeState(StateType.Attack);
            }
            
        }
    }
    public override void OnExit() {

    }
}
