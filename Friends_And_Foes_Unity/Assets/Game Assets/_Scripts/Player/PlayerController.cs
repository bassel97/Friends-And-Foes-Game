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
    float _yVelocity;

    // Wall Run Variables
    [Header("Movement")] [SerializeField] float _wallRayLength = 0.5f;
    [SerializeField] float _WallRunSpeed = 15;

    // Boundary
    Bounds _bounds;
    const float _skinWidth = 0.015f;

    // MonoBehaviour
    [Header("MonoBehaviour")] [SerializeField] Animator _animator;
    Collider _playerCollider;
    Rigidbody _rigidbody;
    PlayerInput _playerInput;

    Vector3 _playerInputVector, _playerSpeedVector, _rigidBodyVelocity, _rigidBodyVelocityBoost;

    bool _isMovmentPressed = false;
    bool _isJumpPressed = false;
    bool _isWallRunPresed = false;

    bool _isRunning = false;
    bool _isGrounded = false;

    bool _isWallRight, _isWallLeft;

    float _sqrRootTwo = Mathf.Sqrt(2.0f);

    public float PlayerSpeed { get { return _playerSpeed; } }
    public float WallRunSpeed { get { return _WallRunSpeed; } }
    public float Gravity { get { return _gravity; } }
    public float WallRayLength { get { return _wallRayLength; } }
    public float Y_Velocity { get { return _yVelocity; } set { _yVelocity = value; } }
    public float JumpInitialVelocity { get { return _jumpInitialVelocity; } set { _jumpInitialVelocity = value; } }

    public bool IsMovmentPressed { get { return _isMovmentPressed; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } set { _isJumpPressed = value; } }
    public bool IsWallRunPressed { get { return _isWallRunPresed; } }

    public bool IsRunning { get { return _isRunning; } }
    public bool IsGrounded { get { return _isGrounded; } }
    
    public bool IsWallRight { get { return _isWallRight; } }
    public bool IsWallLeft { get { return _isWallLeft; } }

    public LayerMask ObstaclesCollisionMask { get { return _obstaclesCollisionMask; } }

    public Vector3 PlayerSpeedVector { get { return _playerSpeedVector; } }
    public Vector3 RigidBodyVelocity { get { return _rigidBodyVelocity; } set { _rigidBodyVelocity = value; } }
    public Vector3 RigidBodyVelocityBoost { get { return _rigidBodyVelocityBoost; } set { _rigidBodyVelocityBoost = value; } }

    public Animator Animator { get { return _animator; } }
    public Rigidbody Rigidbody { get { return _rigidbody; } }

    public Bounds Bounds { get { return _bounds; } }

    private void Awake()
    {
        // Intializing the player inputs
        _playerInput = new PlayerInput();

        _playerInput.PlayerMove.Run.started += HandlePlayerInputRunning;
        _playerInput.PlayerMove.Run.canceled += HandlePlayerInputRunning;

        _playerInput.PlayerMove.Jump.started += HandlePlayerInputJumping;
        _playerInput.PlayerMove.Jump.canceled += HandlePlayerInputJumping;

        _playerInput.PlayerMove.Wall_Run.started += HandlePlayerInputWallRunning;
        _playerInput.PlayerMove.Wall_Run.canceled += HandlePlayerInputWallRunning;

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

        // Set wall Right
        Vector3 wallRayOrigin = _bounds.center;
        _isWallLeft = Physics.Raycast(wallRayOrigin, -transform.right, _wallRayLength, _obstaclesCollisionMask);
        _isWallRight = Physics.Raycast(wallRayOrigin, transform.right, _wallRayLength, _obstaclesCollisionMask);
        Debug.DrawRay(wallRayOrigin, -transform.right * _wallRayLength, Color.red);
        Debug.DrawRay(wallRayOrigin, transform.right * _wallRayLength, Color.red);

        // Set ground speed value
        _playerSpeedVector = Vector3.Lerp(_playerSpeedVector, _playerInputVector * (_isRunning ? _sqrRootTwo : 1.0f), Time.deltaTime * _acceleration);
        float groundSpeedValue = _playerSpeedVector.sqrMagnitude;
        _animator.SetFloat("Speed", groundSpeedValue);

        // Resetting Boost
        _rigidBodyVelocityBoost = Vector3.Lerp(_rigidBodyVelocityBoost, Vector3.zero, Time.deltaTime * 10.0f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _rigidBodyVelocity.y = _yVelocity;
        _rigidbody.velocity = _rigidBodyVelocity + _rigidBodyVelocityBoost;
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
        _isJumpPressed = context.ReadValueAsButton();
    }

    private void HandlePlayerInputWallRunning(InputAction.CallbackContext context)
    {
        _isWallRunPresed = context.ReadValueAsButton();
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
