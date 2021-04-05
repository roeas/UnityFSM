using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    public RunState(FSM fsmIn) : base(fsmIn) {

    }
    public override void OnEnter() {
        fsm.animator.Play("Rogue_Run");
    }
    public override void OnUpdate() {
        if (fsm.playerLAArea.bounds.Intersects(fsm.rogueCollider.bounds) || fsm.playerHAArea.bounds.Intersects(fsm.rogueCollider.bounds)) {//ÊÜÉË
            fsm.ChangeState(StateType.Hurt);
        }
        if (fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[0].bounds) || fsm.attackLenth.bounds.Intersects(fsm.playerColldiers[1].bounds)) {//½üÕ½¹¥»÷
            fsm.ChangeState(StateType.Attack);
        }
    }
    public override void OnFixedUpdate() {
        fsm.body.velocity = new Vector2(fsm.rogue.Speed * fsm.transform.localScale.x * Time.fixedDeltaTime, fsm.body.velocity.y);
    }
    public override void OnExit() {

    }
}
