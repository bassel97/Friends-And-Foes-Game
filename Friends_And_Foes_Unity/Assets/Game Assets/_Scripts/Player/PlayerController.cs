using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterStateController
{
    // Movement variables
    [Header("Movement")] [SerializeField] float _playerSpeed = 1;
    [SerializeField] float _acceleration = 10.0f;

    // Jump Variables
    [SerializeField] LayerMask _obstaclesCollisionMask;
    [SerializeField] float _jumpHeight = 4.0f;
    [SerializeField] float _timeToJumpApex = 0.4f;
    float _gravity, _jumpInitialVelocity;
    bool _jumpPressed = false;
    float _yVelocity;

    // Boundary
    Bounds _bounds;
    const float _skinWidth = 0.015f;

    // MonoBehaviour
    Collider _playerCollider;
    Rigidbody _rigidbody;
    PlayerInput _playerInput;
    [SerializeField] Animator _animator;

    Vector3 _playerInputVector, _playerSpeedVector, _rigidBodyVelocity;

    bool _isMovmentPressed = false;
    bool _isRunning = false;
    bool _isGrounded = false;

    float _sqrRootTwo = Mathf.Sqrt(2.0f);
    public float PlayerSpeed { get { return _playerSpeed; } }
    public float Gravity { get { return _gravity; } }
    public float Y_Velocity { get { return _yVelocity; } set { _yVelocity = value; } }
    public float JumpInitialVelocity { get { return _jumpInitialVelocity; } set { _jumpInitialVelocity = value; } }
    public bool IsMovmentPressed { get { return _isMovmentPressed; } }
    public bool JumpPressed { get { return _jumpPressed; } set { _jumpPressed = value; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public Vector3 PlayerSpeedVector { get { return _playerSpeedVector; } }
    public Vector3 RigidBodyVelocity { get { return _rigidBodyVelocity; } set { _rigidBodyVelocity = value; } }

    private void Awake()
    {
        // Intializing the player inputs
        _playerInput = new PlayerInput();

        _playerInput.PlayerMove.Run.started += HandlePlayerInputRunning;
        _playerInput.PlayerMove.Run.canceled += HandlePlayerInputRunning;

        _playerInput.PlayerMove.Jump.started += HandlePlayerInputJumping;

        // Intializing the monobehaviours
        _rigidbody = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<Collider>();

        //Calculate Gravity values
        _gravity = -(2 * _jumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
        _jumpInitialVelocity = Mathf.Abs(_gravity) * _timeToJumpApex;
    }

    protected override void Update()
    {
        base.Update();

        // Handle Player Input
        HandlePlayerInputMovement(_playerInput.PlayerMove.Move.ReadValue<Vector2>());

        // Setup bounds
        _bounds = _playerCollider.bounds;
        _bounds.Expand(_skinWidth * -2);

        // Set isGrounded
        float rayLength = _skinWidth * 2.0f;
        Vector3 groundedRayOrigin = new Vector3((_bounds.min.x + _bounds.max.x) / 2, _bounds.min.y, (_bounds.min.z + _bounds.max.z) / 2);
        bool hit = Physics.Raycast(groundedRayOrigin, Vector3.down, rayLength, _obstaclesCollisionMask);
        Debug.DrawRay(groundedRayOrigin, Vector3.down * rayLength, Color.red);
        _isGrounded = hit;

        // Set ground speed value
        _playerSpeedVector = Vector3.Lerp(_playerSpeedVector, _playerInputVector * (_isRunning ? _sqrRootTwo : 1.0f), Time.deltaTime * _acceleration);
        float groundSpeedValue = _playerSpeedVector.sqrMagnitude;
        _animator.SetFloat("Speed", groundSpeedValue);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _rigidBodyVelocity.y = _yVelocity;
        _rigidbody.velocity = _rigidBodyVelocity;
    }


    private void Jump()
    {
        _jumpPressed = true;
        _animator.SetTrigger("Jump");
    }

    private void HandlePlayerInputMovement(Vector2 playerInput)
    {
        _playerInputVector.x = playerInput.x;
        _playerInputVector.z = playerInput.y;

        _isMovmentPressed = playerInput.sqrMagnitude > 0.0f;
    }

    private void HandlePlayerInputRunning(InputAction.CallbackContext context)
    {
        _isRunning = (float)context.ReadValueAsObject() > 0.0f;
    }

    private void HandlePlayerInputJumping(InputAction.CallbackContext context)
    {
        Jump();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }
    private void OnDisable()
    {
        _playerInput.Disable();
    }
}
