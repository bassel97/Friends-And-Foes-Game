using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Jump")]
public class CharacterJumpAction : Action
{
    Vector3 _rigidBodyVelocityValue;
    public override void FixedUpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        if (playerController.IsJumpPressed)
        {
            playerController.Y_Velocity = playerController.JumpInitialVelocity;
            playerController.Animator.SetTrigger("Jump");
        }
    }

    public override void UpdateAction(StateController controller)
    {
    }
}
