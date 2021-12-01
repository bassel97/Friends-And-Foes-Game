using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    //public float jumpHeight = 4;
    //public float timeToJumpApex = .4f;

    //float gravity;
    //float jumpVelocity;
    //bool jumped = false;

    public float skinWidth = 0.015f;

    public LayerMask collisionMask;

    public bool isGrounded = false;

    public Vector3 velocity;

    public Rigidbody rb;

    private void Awake()
    {
        //gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);        
    }

    Bounds bounds;

    private void Update()
    {
        bounds = GetComponent<Collider>().bounds;
        bounds.Expand(skinWidth * -2);        
    }

    /*private void FixedUpdate()
    {
        float rayLength = 0.025f;

        Vector3 groundedRayOrigin = new Vector3((bounds.min.x + bounds.max.x) / 2, bounds.min.y, (bounds.min.z + bounds.max.z) / 2);

        bool hit = Physics.Raycast(groundedRayOrigin, Vector3.down, rayLength, collisionMask);

        Debug.DrawRay(groundedRayOrigin, Vector3.down * rayLength, Color.red);

        isGrounded = hit;

        if (isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (jumped)
        {
            Debug.Log("Before Jump " + velocity);
            velocity.y = jumpVelocity;
            Debug.Log("After Jump " + velocity);
            jumped = false;
        }
        rb.velocity = velocity;
    }*/

    public void JumpPlayer(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            Debug.Log("Jump pressed");
            //jumped = true;
        }
    }
}
