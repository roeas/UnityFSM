using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState {
    private float randomNum;
    private FSM fsm;
    private AnimatorStateInfo animeInfo;

    public IdleState(FSM fsmIn) {
        this.fsm = fsmIn;
    }
    public void OnEnter() {
        fsm.body.velocity = Vector2.zero;
        fsm.animator.Play("Rogue_Idle");
    }
    public void OnUpdate() {
        if(fsm.playerLAArea.bounds.Intersects(fsm.rogueCollider.bounds) || fsm.playerHAArea.bounds.Intersects(fsm.rogueCollider.bounds)) {//受伤
            fsm.ChangeState(StateType.Hurt);
        }
        if (Time.time - fsm.idleStartTime >= fsm.idleWaitTime) {//fsm.idleStartTime在除了Hurt和Idle的每个State结束时重置
            fsm.idleWaitTime = Random.Range(0.5f, 2);//决定下一次的等待时间

            randomNum = Random.value;//决定本次Idle结束后的行动
            if (fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[0].bounds) || fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[1].bounds)) {//近身
                if (randomNum > 0.2) {//0.8
                    fsm.ChangeState(StateType.Attack);
                }
                else {//0.2
                    fsm.ChangeState(StateType.SpecialAttack);
                }
            }
            else {//远离
                if (randomNum > 0.6f) {//0.4
                    fsm.ChangeState(StateType.Run);
                }
                else if (randomNum > 0.2f) {//0.4
                    fsm.ChangeState(StateType.Throw);
                }
                else {//0.2
                    fsm.ChangeState(StateType.SpecialAttack);
                }
            }
        }
    }
    public void OnFixedUpdate() {

    }
    public void OnExit() {

    }
}
