using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FSM fsm;
    protected AnimatorStateInfo animeInfo;
    public State(FSM fsmIn) {
        this.fsm = fsmIn;
    }
    public virtual void OnEnter() {

    }
    public virtual void OnUpdate() {

    }
    public virtual void OnExit() {

    }
}
