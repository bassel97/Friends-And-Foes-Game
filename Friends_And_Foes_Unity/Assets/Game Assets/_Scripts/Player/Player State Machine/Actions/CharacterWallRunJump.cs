using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Wall Run Jump")]
public class CharacterWallRunJump : Action
{
    public float WallJumpVelocity = 5;
    public override void FixedUpdateAction(StateController controller)
    {

    }

    public override void UpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        if (playerController.IsJumpPressed)
        {
            Vector3 _rigidBodyVelocityBoost = playerController.transform.forward;

            if (playerController.IsWallLeft)
            {
                _rigidBodyVelocityBoost.x = WallJumpVelocity;
            }
            else if (playerController.IsWallLeft)
            {
                _rigidBodyVelocityBoost.x = -WallJumpVelocity;
            }

            _rigidBodyVelocityBoost.y = WallJumpVelocity;
            _rigidBodyVelocityBoost.z *= WallJumpVelocity;

            playerController.RigidBodyVelocityBoost = _rigidBodyVelocityBoost;
        }
    }
}
