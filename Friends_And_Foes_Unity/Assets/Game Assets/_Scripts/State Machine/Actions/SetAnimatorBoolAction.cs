using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Set Animator Bool")]
public class SetAnimatorBoolAction : Action
{
    public string BoolName;
    public bool Value;
    public override void FixedUpdateAction(StateController controller)
    {
    }

    public override void UpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        playerController.Animator.SetBool(BoolName, Value);
    }
}
