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

        //Vector3 positionToLookAt = playerController.PlayerSpeedVector;
        Vector3 positionToLookAt = playerController.PlayerSpeedVector.x * playerController.CameraTransform.right
            + playerController.PlayerSpeedVector.z * playerController.CameraTransform.forward;
        positionToLookAt.y = 0.0f;

        if (playerController.IsMovmentPressed)
        {
            Quaternion currentRotation = playerController.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            playerController.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 5.0f);
        }
    }

    public override void FixedUpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        // TO-DO : if worked copy to a function with 'out'
        Vector3 _cameraRight = playerController.CameraTransform.right;
        _cameraRight.y = 0;
        _cameraRight.Normalize();
        Vector3 _cameraForward = playerController.CameraTransform.forward;
        _cameraForward.y = 0;
        _cameraForward.Normalize();

        _rigidBodyVelocityValue = playerController.PlayerSpeedVector.x * playerController.PlayerSpeed * _cameraRight
            + playerController.PlayerSpeedVector.z * playerController.PlayerSpeed * _cameraForward;

        _rigidBodyVelocityValue.y = playerController.RigidBodyVelocity.y;

        playerController.RigidBodyVelocity = _rigidBodyVelocityValue;
    }
}
