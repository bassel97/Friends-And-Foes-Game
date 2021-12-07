using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Set Animator Trigger")]
public class SetAnimatorTriggerAction : Action
{
    public string TriggerName;
    public override void FixedUpdateAction(StateController controller)
    {
    }

    public override void UpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        playerController.Animator.SetTrigger(TriggerName);
    }
}
