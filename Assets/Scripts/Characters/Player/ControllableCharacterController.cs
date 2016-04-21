using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllableCharacterController : Player {
    private Inventory _inventory;
    private Vector2 _directionalMovement    = new Vector2();
    private Vector2 _cameraMovement         = new Vector2();
    private bool    _sprintButtonPressed;
    private bool    _jumpButtonPressed;
    private bool    _actionButtonPressed;
    private bool    _actionButtonHeld;
    private bool    _fireButtonPressed;
    private bool    _fireButtonHeld;
    private List<bool> _weaponSelectButtonPressed = new List<bool>(9) { false, false, false, false, false, false, false, false, false };

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
    override protected void Awake() {
        base.Awake();
        _inventory = GetComponent<Inventory>();
    }
    // Update is called once per frame
    void Update() {
        CaptureInput();
        if(!(State.IsDying || State.IsDead))
        {
            ProcessInput();
        }
        
    }
    /// <summary>
    /// 
    /// </summary>
    private void CaptureInput() {
        //Check if a Weapon Slot Button was pressed.
        for (int button = 1; button < _weaponSelectButtonPressed.Count; button++) {
            if (Input.GetButtonDown("Weapon Slot " + button)) {
                EventManager.WeaponSwitchEvent(_inventory.gameObject.GetInstanceID(), button);
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
    private void ProcessInput()
    {
        State.IsSprinting = _sprintButtonPressed;
        if (FireButtonPressed)
        {
            EventManager.AttackEvent(_inventory.Weapon.gameObject.GetInstanceID(), false);
        }
        else if (FireButtonHeld)
        {
            EventManager.AttackEvent(_inventory.Weapon.gameObject.GetInstanceID(), true);
        }
    }
}