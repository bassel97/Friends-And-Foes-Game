using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Actions/Set RigidBody Velocity")]
public class CharacterSetRigidBodyVelocity : Action
{
    public Vector3 RigidBodyVelocity;
    public override void FixedUpdateAction(StateController controller)
    {

    }

    public override void UpdateAction(StateController controller)
    {
        PlayerController playerController = (PlayerController)controller;

        playerController.RigidBodyVelocity = RigidBodyVelocity;
        playerController.Y_Velocity = RigidBodyVelocity.y;
    }
}
