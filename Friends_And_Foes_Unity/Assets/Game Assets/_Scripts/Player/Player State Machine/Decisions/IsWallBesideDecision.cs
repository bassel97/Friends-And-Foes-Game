using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Decisions/Is Wall Beside Decision")]
public class IsWallBesideDecision : Decision
{
    public bool Inverted = false;
    public override bool Decide(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        return (playerController.IsWallRight || playerController.IsWallLeft) ^ Inverted;
    }
}
