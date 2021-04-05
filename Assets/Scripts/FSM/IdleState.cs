using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {
    private float randomNum;
    public IdleState(FSM fsmIn) : base (fsmIn){

    }
    public override void OnEnter() {
        fsm.body.velocity = Vector2.zero;
        fsm.animator.Play("Rogue_Idle");
    }
    public override void OnUpdate() {
        if(fsm.playerLAArea.bounds.Intersects(fsm.rogueCollider.bounds) || fsm.playerHAArea.bounds.Intersects(fsm.rogueCollider.bounds)) {//受伤
            fsm.ChangeState(StateType.Hurt);
        }
        if (Time.time - fsm.idleStartTime >= fsm.idleWaitTime) {//fsm.idleStartTime在除了Hurt和Idle的每个State结束时重置
            fsm.idleWaitTime = Random.Range(0, 2);
            randomNum = Random.value;
            if (fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[0].bounds) || fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[1].bounds)) {//近战攻击
                if (randomNum > 0.3) {
                    fsm.ChangeState(StateType.Attack);
                }
                else {
                    fsm.ChangeState(StateType.SpecialAttack);
                }
            }
            else {
                if (randomNum > 0.5f) {
                    fsm.ChangeState(StateType.Run);
                }
                else if (randomNum > 0.2f) {
                    fsm.ChangeState(StateType.Throw);
                }
                else {
                    fsm.ChangeState(StateType.SpecialAttack);
                }
            }
        }
    }
    public override void OnFixedUpdate() {

    }
    public override void OnExit() {

    }
}
