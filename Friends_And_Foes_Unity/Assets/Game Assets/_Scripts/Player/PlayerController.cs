using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float PlayerSpeed = 1;
    [Header("Movement")] [SerializeField] float _acceleration = 10.0f;

    // Jump Variables
    [SerializeField] LayerMask _obstaclesCollisionMask;
    [SerializeField] float _jumpHeight = 4.0f;
    [SerializeField] float _timeToJumpApex = 0.4f;
    float _gravity, _jumpInitialVelocity;
    bool _jumped = false;
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

    private void Awake()
    {
        // Intializing the player inputs
        _playerInput = new PlayerInput();

        _playerInput.PlayerMove.Move.started += HandlePlayerInputMovement;
        _playerInput.PlayerMove.Move.performed += HandlePlayerInputMovement;
        _playerInput.PlayerMove.Move.canceled += HandlePlayerInputMovement;

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

    private void Update()
    {
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

        Vector3 positionToLookAt = _playerSpeedVector;
        if (_isMovmentPressed)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 15.0f);
        }
    }

    private void FixedUpdate()
    {
        _yVelocity = _isGrounded ? 0 : (_yVelocity + _gravity * Time.deltaTime);

        // Handle Jumping
        if (_jumped)
        {
            _yVelocity = _jumpInitialVelocity;
            _jumped = false;
        }

        // Setup Rigid body velocity
        _rigidBodyVelocity.y = _yVelocity;
        _rigidBodyVelocity.x = _playerSpeedVector.x * PlayerSpeed;
        _rigidBodyVelocity.z = _playerSpeedVector.z * PlayerSpeed;
        _rigidbody.velocity = _rigidBodyVelocity;
    }


    private void Jump()
    {
        _jumped = true;
        _animator.SetTrigger("Jump");
    }

    private void HandlePlayerInputMovement(InputAction.CallbackContext context)
    {
        Vector2 _playerInput = context.ReadValue<Vector2>();

        _playerInputVector.x = _playerInput.x;
        _playerInputVector.z = _playerInput.y;

        _isMovmentPressed = _playerInput.sqrMagnitude > 0.0f;
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
