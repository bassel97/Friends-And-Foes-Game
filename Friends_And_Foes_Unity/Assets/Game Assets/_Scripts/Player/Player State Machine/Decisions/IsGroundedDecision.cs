using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Decisions/Is Grounded")]
public class IsGroundedDecision : Decision
{
    public bool IsInverted = false;
    public override bool Decide(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;
        return playerController.IsGrounded ^ IsInverted;
    }
}
