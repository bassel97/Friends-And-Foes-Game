using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Wall Run")]
public class CharacterWallRun : Action
{
    public float GravityMultiplier = 0.1f;
    public override void FixedUpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        Vector3 wallRayOrigin = playerController.Bounds.center;

        float wallRayLength = playerController.WallRayLength * 0.5f;

        if (playerController.IsWallLeft)
        {
            Debug.DrawRay(wallRayOrigin, (-playerController.transform.right + playerController.transform.forward * 2.0f) * wallRayLength, Color.blue);

            bool _isWallLeftFront = Physics.Raycast(wallRayOrigin, ((-playerController.transform.right + playerController.transform.forward * 2.0f)).normalized,
            wallRayLength * Mathf.Sqrt(2.0f), playerController.ObstaclesCollisionMask);

            if (_isWallLeftFront)
            {
                playerController.transform.Rotate(Vector3.up, Time.deltaTime * 180.0f);
            }

            Vector3 wallRunSpeed = playerController.transform.forward * playerController.WallRunSpeed;
            playerController.RigidBodyVelocity = wallRunSpeed;

            playerController.Y_Velocity += (playerController.Gravity * GravityMultiplier) * Time.deltaTime;
        }

        if (playerController.IsWallRight)
        {
            Debug.DrawRay(wallRayOrigin, (playerController.transform.right + playerController.transform.forward * 2.0f) * wallRayLength, Color.blue);

            bool _isWallLeftFront = Physics.Raycast(wallRayOrigin, ((playerController.transform.right + playerController.transform.forward * 2.0f)).normalized,
            wallRayLength * Mathf.Sqrt(2.0f), playerController.ObstaclesCollisionMask);

            if (_isWallLeftFront)
            {
                playerController.transform.Rotate(Vector3.up, Time.deltaTime * -180.0f);
            }

            Vector3 wallRunSpeed = playerController.transform.forward * playerController.WallRunSpeed;
            playerController.RigidBodyVelocity = wallRunSpeed;

            playerController.Y_Velocity += (playerController.Gravity * GravityMultiplier) * Time.deltaTime;
        }

        playerController.Animator.SetBool("IsWallLeft", playerController.IsWallLeft);
    }

    public override void UpdateAction(StateController controller)
    {
    }
}
