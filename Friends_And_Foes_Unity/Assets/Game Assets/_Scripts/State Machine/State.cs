using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State")]
public class State : ScriptableObject
{
    public Color SceneGizmoColor = Color.grey;

    public Action[] Actions;
    public Action[] OnEnterActions;
    public Action[] OnExitActions;

    public Transition[] Transitions;

    public void OnExitAction(StateController controller)
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            OnExitActions[i].UpdateAction(controller);
        }
    }

    public void OnEnterAction(StateController controller)
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            OnEnterActions[i].UpdateAction(controller);
        }
    }

    public void UpdateState(StateController controller)
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i].UpdateAction(controller);
        }
        CheckTransitions(controller);
    }

    public void FixedUpdateState(StateController controller)
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i].FixedUpdateAction(controller);
        }
    }

    private void CheckTransitions(StateController controller)
    {
        for (int i = 0; i < Transitions.Length; i++)
        {
            bool decisionSucceeded = Transitions[i].decision.Decide(controller);

            if (decisionSucceeded)
                controller.TransitionToState(Transitions[i].trueState);
        }
    }
}
