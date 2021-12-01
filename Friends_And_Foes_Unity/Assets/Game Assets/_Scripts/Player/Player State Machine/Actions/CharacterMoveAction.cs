using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Move")]
public class CharacterMoveAction : Action
{
    Vector3 _rigidBodyVelocityValue;
    public override void UpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        Vector3 positionToLookAt = playerController.PlayerSpeedVector;
        if (playerController.IsMovmentPressed)
        {
            Quaternion currentRotation = playerController.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            playerController.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 15.0f);
        }
    }

    public override void FixedUpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        _rigidBodyVelocityValue.y = playerController.RigidBodyVelocity.y;
        _rigidBodyVelocityValue.x = playerController.PlayerSpeedVector.x * playerController.PlayerSpeed;
        _rigidBodyVelocityValue.z = playerController.PlayerSpeedVector.z * playerController.PlayerSpeed;

        playerController.RigidBodyVelocity = _rigidBodyVelocityValue;
    }
}
