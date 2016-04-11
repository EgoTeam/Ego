using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterState : MonoBehaviour {
    //Define Inout Settings
    private Vector2 _directionalMovement = new Vector2();
    private Vector2 _cameraMovement = new Vector2();
    private bool _sprintButtonPressed;
    private bool _jumpButtonPressed;
    private bool _actionButtonPressed;
    private bool _actionButtonHeld;
    private bool _fireButtonPressed;
    private bool _fireButtonHeld;
    private List<bool> _weaponSelectButtonPressed = new List<bool>(9) { false, false, false, false, false, false, false, false, false };
    //Define State Settings.
    [SerializeField] private bool _wasGrounded;                  //True if the player was previously grounded.
    [SerializeField] private bool _isGrounded;                   //True if the player is on a surface, false otherwise.
    [SerializeField] private bool _isJumping;                    //True if the player is jumping, false otherwise.
    [SerializeField] private bool _isFalling;                    //True if the player is falling, false otherwise.
    [SerializeField] private bool _isIdle;                       //True if the player is idle, false otherwise.
    [SerializeField] private bool _isWalking;                    //True if the player is walking, false otherwise.
    [SerializeField] private bool _isSprinting;                  //True if the player is sprinting, false otherwise.
    [SerializeField] private bool _isDrawingWeapon;
    [SerializeField] private bool _isHolsteringWeapon;
    [SerializeField] private bool _isShooting;
    [SerializeField] private bool _isReloading;
    [SerializeField] private bool _isDoingAction;
    [SerializeField] private bool _isDoingHoldAction;

    public Vector2 DirectionMovement {
        get { return _directionalMovement; }
        set { _directionalMovement = value; }
    }

    public Vector2 CameraMovement {
        get { return _cameraMovement; }
        set { _cameraMovement = value; }
    }

    public float DirectionMovementX {
        get { return _directionalMovement.x; }
        set { _directionalMovement.x = value; }
    }

    public float DirectionMovementY {
        get { return _directionalMovement.y; }
        set { _directionalMovement.y = value; }
    }

    public float CameraMovementX {
        get { return _cameraMovement.x; }
        set { _cameraMovement.x = value; }
    }

    public float CameraMovementY {
        get { return _cameraMovement.y; }
        set { _cameraMovement.y = value; }
    }

    public bool SprintButtonPressed {
        get { return _sprintButtonPressed; }
        set { _sprintButtonPressed = value; }
    }

    public bool JumpButtonPressed {
        get { return _jumpButtonPressed; }
        set { _jumpButtonPressed = value; }
    }

    public bool ActionButtonPressed {
        get { return _actionButtonPressed; }
        set { _actionButtonPressed = value; }
    }

    public bool ActionButtonHeld {
        get { return _actionButtonHeld; }
        set { _actionButtonHeld = value; }
    }

    public bool FireButtonPressed {
        get { return _fireButtonPressed; }
        set { _fireButtonPressed = value; }
    }

    public bool FireButtonHeld {
        get { return _fireButtonHeld; }
        set { _fireButtonHeld = value; }
    }

    public bool IsGrounded {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }

    public bool WasGrounded {
        get { return _wasGrounded; }
        set { _wasGrounded = value;}
    }
    public bool IsIdle {
        get { return _isIdle; }
        set { _isIdle = value; }
    }
    public bool IsWalking {
        get { return _isWalking; }
        set { _isWalking = value; }
    }
    public bool IsSprinting {
        get { return _isSprinting; }
        set { _isSprinting = value; }
    }
    public bool IsJumping {
        get { return _isJumping; }
        set { _isJumping = value; }
    }
    public bool IsFalling {
        get { return _isFalling; }
        set { _isFalling = value; }
    }
    public bool IsDrawingWeapon {
        get { return _isDrawingWeapon; }
        set { _isDrawingWeapon = value; }
    }
    public bool IsHolsteringWeapon {
        get { return _isHolsteringWeapon; }
        set { _isHolsteringWeapon = value; }
    }
    public bool IsShooting {
        get { return _isShooting; }
        set{ _isShooting = value; }
    }
    public bool IsReloading{
        get { return _isReloading; }
        set { _isReloading = value; }
    }
    public bool IsDoingAction{
        get { return _isDoingAction; }
        set { _isDoingAction = value; }
    }
    public bool IsDoingHoldAction{
        get { return _isDoingHoldAction; }
        set { _isDoingHoldAction = value; }
    }

    // Update is called once per frame
    void Update() {
        CaptureInput();
        ProcessInput();
    }
    /// <summary>
    /// 
    /// </summary>
    private void CaptureInput() {
        //Check if a Weapon Slot Button was pressed.
        for (int button = 1; button < _weaponSelectButtonPressed.Count; button++) {
            if (Input.GetButtonDown("Weapon Slot " + button)) {
                
            }
        }
        DirectionMovementX  = Input.GetAxis("Directional Movement Horizontal");
        DirectionMovementY  = Input.GetAxis("Directional Movement Vertical");
        CameraMovementX     = Input.GetAxis("Camera Movement Horizontal");
        CameraMovementY     = Input.GetAxis("Camera Movement Vertical");
        SprintButtonPressed = Input.GetButton("Sprint");
        JumpButtonPressed   = Input.GetButtonDown("Jump");
        ActionButtonPressed = Input.GetButtonDown("Action");
        ActionButtonHeld    = Input.GetButton("Action");
        FireButtonPressed   = Input.GetButtonDown("Primary Attack");
        FireButtonHeld      = Input.GetButton("Primary Attack");
    }
    private void ProcessInput() {
        IsIdle              = DirectionMovementX == 0 && DirectionMovementY == 0 && !IsJumping && !IsFalling;
        IsSprinting         = SprintButtonPressed && !IsIdle && !IsJumping && !IsFalling;
        IsWalking           = !IsIdle && !IsSprinting && !IsJumping && !IsFalling;
    }
}