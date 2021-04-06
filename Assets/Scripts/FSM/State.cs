using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    public void OnEnter();
    public void OnUpdate();
    public void OnFixedUpdate();
    public void OnExit();
}
