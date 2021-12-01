using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [Header("State Machine")] public State CurrentState;

    protected virtual void Update()
    {
        CurrentState.UpdateState(this);
    }
    protected virtual void FixedUpdate()
    {
        CurrentState.FixedUpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        CurrentState.OnExitAction(this);
        CurrentState = nextState;
        CurrentState.OnEnterAction(this);
    }

    void OnDrawGizmos()
    {
        if (CurrentState != null)
        {
            Gizmos.color = CurrentState.SceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 0.333f);
        }
    }
}
