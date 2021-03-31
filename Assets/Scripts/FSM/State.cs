using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected FSM fsm;
    protected RogueSO rogue;
    public State(FSM fsmIn) {
        this.fsm = fsmIn;
        this.rogue = fsmIn.rogue;
    }
    public virtual void OnEnter() {

    }
    public virtual void OnUpdate() {

    }
    public virtual void OnExit() {

    }
}
