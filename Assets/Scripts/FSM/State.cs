using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected FSM fsm;
    protected RogueSO rogue;
    protected GameObject plaer;
    public State(FSM fsmIn) {
        this.fsm = fsmIn;
        this.rogue = fsmIn.rogue;
        this.plaer = fsmIn.player;
    }
    public virtual void OnEnter() {

    }
    public virtual void OnUpdate() {

    }
    public virtual void OnExit() {

    }
}
