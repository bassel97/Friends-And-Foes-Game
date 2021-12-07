using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Decisions/Can Wall Run")]
public class CanWallRunDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        bool canWallRun = playerController.IsWallRight || playerController.IsWallLeft;

        if (canWallRun && playerController.IsWallRunPressed)
        {
            playerController.IsJumpPressed = false;
            return true;
        }
        return false;
    }
}
