using UnityEngine;
using System.Collections;

public class CharacterMovementController : MonoBehaviour {

    private     CharacterState              State;
    private new Rigidbody                   rigidbody;
    private     CapsuleCollider             capsuleCollider;
    private     Transform                   cameraTransform;

    //Define The Camera Settings.
    [SerializeField] private Vector2 _cameraSensitivity = new Vector2(1.0f, 1.0f);
    [SerializeField] private float   _lookLimit = 0.40f;

    //Define Ground Settings.
    [SerializeField]                    private Transform   _ground;                    //The position of the ground.
    [SerializeField]                    private LayerMask   _whatIsGround;              //The layer(s) that are considered a ground layer.
    [SerializeField][Range(0.0f, 5.0f)] private float       _groundRadius;              //The radius of the circle used to check if the player is on the ground.

    //Define Movement Settings.
    [SerializeField][Range(0.0f, 030.0f)] private float _gravity = 09.8f;               //The gravity applied to the player.
    [SerializeField][Range(0.0f, 020.0f)] private float _forwardSpeed = 10.0f;          //The speed of the character when moving forward.
    [SerializeField][Range(0.0f, 020.0f)] private float _backwardSpeed = 05.0f;         //The speed of the character when moving backward.
    [SerializeField][Range(0.0f, 020.0f)] private float _strafeSpeed = 08.0f;           //The speed of the character when strafing (moving left or right).
    [SerializeField][Range(1.0f, 005.0f)] private float _sprintMultiplyer = 02.0f;      //The speed multiplyer when the character is sprinting.
    [SerializeField][Range(0.0f, 100.0f)] private float _jumpForce = 05.0f;             //The force applied to the character when jumping.
    [SerializeField][Range(0.0f, 020.0f)] private float _maxDifference = 10.0f;         //The maximum difference in velocity between the target velocity and the acutal velocity of the player.

    // Use this for initialization
    void Awake() {
        State                       = GetComponent<CharacterState>();
        rigidbody                   = GetComponent<Rigidbody>();
        capsuleCollider             = GetComponent<CapsuleCollider>();
        cameraTransform             = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        rigidbody.freezeRotation    = true;
        rigidbody.useGravity        = false;
    }
	/// <summary>
    /// 
    /// </summary>
    void FixedUpdate() {
        //Check if the player is on the ground.
        GroundCheck();
        //If the player is on the ground...
        if (State.IsGrounded) {
            //...The player is not jumping.
            State.IsJumping = false;
            //...The player is not falling.
            State.IsFalling = false;
            //If the player is jumping...
            if (State.JumpButtonPressed) {
                //The player is jumping.
                State.IsJumping = true;
                //Apply a vertical force.
                rigidbody.AddForce(new Vector3(0f, (_jumpForce * _gravity) / 2, 0f), ForceMode.Impulse);
            }
        }
        //If the player was grounded, and is not jumping, and is not grounded...
        if (State.WasGrounded && !State.IsJumping && !State.IsGrounded) {
            //...Apply a downward force to keep the player on the ground.
            rigidbody.AddForce(new Vector3(0f, -_gravity, 0f), ForceMode.Impulse);
        }
        //...Else...
        else {
            //...Apply the force of gravity.
            rigidbody.AddForce(new Vector3(0f, -_gravity, 0f), ForceMode.Force);
        }
        //The player is falling if the vertical velocity is less than zero, and the player is not grounded.
        State.IsFalling = rigidbody.velocity.y < 0 && !State.IsGrounded;
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
	void LateUpdate () {
        RotateCamera();
	}
    /// <summary>
    /// 
    /// </summary>
    private void RotateCamera() {
        if ((this.cameraTransform.localRotation.x < _lookLimit && State.CameraMovementY < 0) || (this.cameraTransform.localRotation.x > -_lookLimit && State.CameraMovementY > 0)) {
            this.cameraTransform.Rotate(new Vector3(-State.CameraMovementY * _cameraSensitivity.y, 0f, 0f));
        }
        this.transform.Rotate(new Vector3(0f, State.CameraMovementX * _cameraSensitivity.x, 0f), Space.World);
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
        Vector3 targetVelocity = new Vector3(State.DirectionMovementX, 0f, State.DirectionMovementY);
        //Apply forward and backward movement speed modifiers to the target velocity.
        targetVelocity.z *= (targetVelocity.z > 0 ? _forwardSpeed : _backwardSpeed);
        //Apply the strafe movement modifier to the target velocity.
        targetVelocity.x *= _strafeSpeed;
        //Apply sprint speed modifier to the target velocity.
        targetVelocity *= (State.IsSprinting ? _sprintMultiplyer : 1f);
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
        Vector3 force = new Vector3();
        //Calculate the velocity difference.
        Vector3 velocityDifference = (targetVelocity - rigidbody.velocity);
        force.x = Mathf.Clamp(velocityDifference.x, -_maxDifference, _maxDifference);
        force.z = Mathf.Clamp(velocityDifference.z, -_maxDifference, _maxDifference);
        force.y = 0f;
        //Return the force.
        return force;
    }
    /// <summary>
    /// 
    /// </summary>
    private void GroundCheck() {
        //The player was grounded if the last ground check indicated the player is grounded.
        State.WasGrounded = State.IsGrounded;
        //The player is grounded if the sphere collides with another collider.
        State.IsGrounded = (Physics.OverlapSphere(_ground.position, _groundRadius, _whatIsGround).Length) > 0 ? true : false;
    }
}