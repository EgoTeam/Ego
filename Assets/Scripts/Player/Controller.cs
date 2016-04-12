using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class Controller : Character {
    /*
    private new Rigidbody   rigidbody;
    private CapsuleCollider capsuleCollider;
    private Transform       cameraTransform;
    
    //Weapon Inventory
    [SerializeField] private List<Weapon>  _weapons;
    [SerializeField] private Weapon _weapon = null;

    //Define The Camera Settings.
    [SerializeField] private Vector2    _cameraSensitivity  = new Vector2(1.0f, 1.0f);
    [SerializeField] private float      _lookLimit          = 0.40f;

    //Define Ground Settings.
    [SerializeField]                        private Transform   _ground;                        //The position of the ground.
    [SerializeField]                        private LayerMask   _whatIsGround;                  //The layer(s) that are considered a ground layer.
    [SerializeField][Range(0.0f, 5.0f)]     private float       _groundRadius;                  //The radius of the circle used to check if the player is on the ground.
    
    //Define Movement Settings.
    [SerializeField][Range(0.0f, 030.0f)]   private float _gravity          = 09.8f;    //The gravity applied to the player.
    [SerializeField][Range(0.0f, 020.0f)]   private float _forwardSpeed     = 10.0f;    //The speed of the character when moving forward.
    [SerializeField][Range(0.0f, 020.0f)]   private float _backwardSpeed    = 05.0f;    //The speed of the character when moving backward.
    [SerializeField][Range(0.0f, 020.0f)]   private float _strafeSpeed      = 08.0f;    //The speed of the character when strafing (moving left or right).
    [SerializeField][Range(1.0f, 005.0f)]   private float _sprintMultiplyer = 02.0f;    //The speed multiplyer when the character is sprinting.
    [SerializeField][Range(0.0f, 100.0f)]   private float _jumpForce        = 05.0f;    //The force applied to the character when jumping.
    [SerializeField][Range(0.0f, 020.0f)]   private float _maxDifference    = 10.0f;    //The maximum difference in velocity between the target velocity and the acutal velocity of the player.

    //Define State Settings.
    [SerializeField]                        private bool _wasGrounded;                  //True if the player was previously grounded.
    [SerializeField]                        private bool _isGrounded;                   //True if the player is on a surface, false otherwise.
    [SerializeField]                        private bool _isJumping;                    //True if the player is jumping, false otherwise.
    [SerializeField]                        private bool _isFalling;                    //True if the player is falling, false otherwise.
    [SerializeField]                        private bool _isIdle;                       //True if the player is idle, false otherwise.
    [SerializeField]                        private bool _isWalking;                    //True if the player is walking, false otherwise.
    [SerializeField]                        private bool _isSprinting;                  //True if the player is sprinting, false otherwise.
    [SerializeField]                        private bool _isDrawingWeapon;
    [SerializeField]                        private bool _isHolsteringWeapon;
    [SerializeField]                        private bool _isShooting;
    [SerializeField]                        private bool _isReloading;
    [SerializeField]                        private bool _isDoingAction;
    [SerializeField]                        private bool _isDoingHoldAction;

    //Define Inout Settings
    private Vector2 _directionalMovement = new Vector2();
    private Vector2 _cameraMovement = new Vector2();
    private bool _sprintButtonPressed;
    private bool _jumpButtonPressed;
    private bool _actionButtonPressed;
    private bool _actionButtonHeld;
    private bool _fireButtonPressed;
    private bool _fireButtonHeld;
    private List<bool> _weaponSlotButtonPressed = new List<bool>(9) {false, false, false, false, false, false, false, false, false};

    public bool IsGrounded {
        get { return _isGrounded; }
    }
    public bool IsIdle {
        get { return _isIdle; }
    }
    public bool IsWalking {
        get { return _isWalking; }
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
        set {
            _isShooting = value;
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Shoot, EquippedWeapon, _isShooting);    
        }
    }
    public bool IsReloading {
        get { return _isReloading; }
        set { _isReloading = value; }
    }
    public bool IsDoingAction {
        get { return _isDoingAction; }
    }
    public bool IsDoingHoldAction {
        get { return _isDoingHoldAction; }
    }
    public List<Weapon> Weapons {
        get { return _weapons; }
        set { _weapons = value; }
    }
    public Weapon EquippedWeapon {
        get { return _weapon; }
        set { _weapon = value;}
    }
    void Awake() {
        rigidbody                   = GetComponent<Rigidbody>();
        capsuleCollider             = GetComponent<CapsuleCollider>();
        cameraTransform             = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        rigidbody.freezeRotation    = true;
        rigidbody.useGravity        = false;
    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Start() {
        base.Start();
        
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Change, this);
        EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Change, this, 2);
    }
    /// <summary>
    /// 
    /// </summary>
    void Update() {
        //Get the user input.
        //GetInput();
        //Process the user input.
        ProcessInput();
        //Rotate Camera.
        RotateCamera();
    }
    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate() {
        _jumpButtonPressed = Input.GetButtonDown("Jump");
        //Check if the player is on the ground.
        GroundCheck();
        //If the player is on the ground...
        if (IsGrounded) {
            //...The player is not jumping.
            IsJumping = false;
            //...The player is not falling.
            IsFalling = false;
            //If the player is jumping...
            if (_jumpButtonPressed) {
                //The player is jumping.
                IsJumping = true;
                //Apply a vertical force.
                rigidbody.AddForce(new Vector3(0f, (_jumpForce * _gravity) / 2, 0f), ForceMode.Impulse);
            }
        }


        //If the player was grounded, and is not jumping, and is not grounded...
        if (_wasGrounded && !IsJumping && !IsGrounded) {
            //...Apply a downward force to keep the player on the ground.
            rigidbody.AddForce(new Vector3(0f, -_gravity, 0f), ForceMode.Impulse);
        }
        //...Else...
        else {
            //...Apply the force of gravity.
            rigidbody.AddForce(new Vector3(0f, -_gravity, 0f), ForceMode.Force);
        }
        //The player is falling if the vertical velocity is less than zero, and the player is not grounded.
        IsFalling = rigidbody.velocity.y < 0 && !IsGrounded;
        //Calculate the target velocity.
        Vector3 targetVelocity = CalculateTargetVelocity();
        //Calculate the force to apply.
        Vector3 force = CalculateForce(targetVelocity);
        //Apply a force to the rigidbody.
        rigidbody.AddForceAtPosition(force, this.transform.forward, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ProcessInput() {
        IsSprinting          = _sprintButtonPressed;
		if (_fireButtonHeld && EquippedWeapon.CanUsePrimary && EquippedWeapon.Mechanism.Equals(Weapon.FireType.Automatic)) {
			EventManager.Instance.PostNotification (EventCategory.WeaponEvent.Shoot, EquippedWeapon);
		}
        else if(_fireButtonPressed && EquippedWeapon.CanUsePrimary) {
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Shoot, EquippedWeapon);
        }
        //For each bool value in the _weaponSlotButtonPressed List...
        foreach(bool buttonPressed in _weaponSlotButtonPressed) {
            //...If the button was pressed...
            if(buttonPressed) {
                EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Change, this, _weaponSlotButtonPressed.IndexOf(buttonPressed));
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
	private void RotateCamera() {
		//Debug.Log (this.cameraTransform.rotation.x + " \t" + _cameraMovement.y);
		if ((this.cameraTransform.localRotation.x < _lookLimit && _cameraMovement.y < 0) || (this.cameraTransform.localRotation.x > -_lookLimit && _cameraMovement.y > 0)) { 
			this.cameraTransform.Rotate(new Vector3 (-_cameraMovement.y * _cameraSensitivity.y, 0f, 0f));
		}
		this.transform.Rotate (new Vector3 (0f, _cameraMovement.x * _cameraSensitivity.x, 0f), Space.World);
	}
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	private float CalculateJumpForce() {
        //Return the jump force.
		return Mathf.Sqrt(2 * _jumpForce * _gravity);
	}
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	private Vector3 CalculateTargetVelocity() {
		Vector3 targetVelocity = new Vector3(_directionalMovement.x, 0f, _directionalMovement.y);
		//Apply forward and backward movement speed modifiers to the target velocity.
		targetVelocity.z *= (targetVelocity.z > 0 ? _forwardSpeed : _backwardSpeed);
		//Apply the strafe movement modifier to the target velocity.
		targetVelocity.x *= _strafeSpeed;
		//Apply sprint speed modifier to the target velocity.
		targetVelocity *= (_isSprinting ? _sprintMultiplyer : 1f);
        //Account for the direction the player is traveling in.
        targetVelocity = transform.TransformDirection(targetVelocity);
		//Return the target velocity.
		return targetVelocity;
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetVelocity"></param>
    /// <returns></returns>
	private Vector3 CalculateForce(Vector3 targetVelocity) {
		//Instantiate a Force vector.
		Vector3 force 				= new Vector3();
		//Calculate the velocity difference.
		Vector3 velocityDifference 	= (targetVelocity - rigidbody.velocity);
		force.x = Mathf.Clamp (velocityDifference.x, -_maxDifference, _maxDifference);
		force.z = Mathf.Clamp (velocityDifference.z, -_maxDifference, _maxDifference);
		force.y = 0f;
		//Return the force.
		return force;
	}
    /// <summary>
    /// 
    /// </summary>
	private void GroundCheck() {
		//The player was grounded if the last ground check indicated the player is grounded.
		_wasGrounded = _isGrounded;
        //The player is grounded if the sphere collides with another collider.
		_isGrounded  = (Physics.OverlapSphere(_ground.position, _groundRadius, _whatIsGround).Length) > 0 ? true : false;
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator OnWeaponChange(int index) {
        if(!_weapons[index].IsEquipped && _weapons[index].IsCollected) {
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Holster, EquippedWeapon);
            while (IsHolsteringWeapon) {
                yield return 0;
            }
            EquippedWeapon = _weapons[index];
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Draw, EquippedWeapon);
        }
        yield return 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="sender"></param>
    /// <param name="param"></param>
    public override void OnEvent(Enum type, Component sender, object param = null) {
        if (!this.GetInstanceID().Equals(sender.GetInstanceID())) {
            return;
        }

        base.OnEvent(type, sender, param);

        switch ((EventCategory.WeaponEvent) type) {
            case EventCategory.WeaponEvent.Change:
                StartCoroutine(OnWeaponChange((int)param));
                break;
        }
    }*/
}