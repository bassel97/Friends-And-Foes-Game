using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Apply Gravity")]
public class CharacterApplyGravityAction : Action
{
    Vector3 _rigidBodyVelocityValue;
    public override void FixedUpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;
        playerController.Y_Velocity = playerController.IsGrounded ? 0 : (playerController.Y_Velocity + playerController.Gravity * Time.deltaTime);
    }

    public override void UpdateAction(StateController controller)
    {
        return;
    }
}
