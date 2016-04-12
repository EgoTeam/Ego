using UnityEngine;
using System.Collections;

public class ControllableCharacterMovement : MonoBehaviour {

    private ControllableCharacterController _controller;
    private new Rigidbody rigidbody;
    //private CapsuleCollider capsuleCollider;
    private Transform cameraTransform;

    //Define The Camera Settings.
    [SerializeField] private Vector2 _cameraSensitivity = new Vector2(1.0f, 1.0f);
    [SerializeField] private float _lookLimit = 0.40f;

    //Define Ground Settings.
    [SerializeField]                    private Transform   _ground;                    //The position of the ground.
    [SerializeField]                    private LayerMask   _whatIsGround;              //The layer(s) that are considered a ground layer.
    [SerializeField][Range(0.0f, 5.0f)] private float       _groundRadius;              //The radius of the circle used to check if the player is on the ground.

    //Define Movement Settings.
    [SerializeField] [Range(0.0f, 030.0f)] private float _gravity           = 09.8f;    //The gravity applied to the player.
    [SerializeField] [Range(0.0f, 020.0f)] private float _forwardSpeed      = 10.0f;    //The speed of the character when moving forward.
    [SerializeField] [Range(0.0f, 020.0f)] private float _backwardSpeed     = 05.0f;    //The speed of the character when moving backward.
    [SerializeField] [Range(0.0f, 020.0f)] private float _strafeSpeed       = 08.0f;    //The speed of the character when strafing (moving left or right).
    [SerializeField] [Range(1.0f, 005.0f)] private float _sprintMultiplyer  = 02.0f;    //The speed multiplyer when the character is sprinting.
    [SerializeField] [Range(0.0f, 100.0f)] private float _jumpForce         = 05.0f;    //The force applied to the character when jumping.
    [SerializeField] [Range(0.0f, 020.0f)] private float _maxDifference     = 10.0f;    //The maximum difference in velocity between the target velocity and the acutal velocity of the player.

    protected ControllableCharacterController Controller {
        get { return _controller; }
    }

    // Use this for initialization
    void Awake() {
        _controller = GetComponent<ControllableCharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        ///capsuleCollider = GetComponent<CapsuleCollider>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
        //Controller.State.IsGrounded = false;
    }

    void FixedUpdate() {
        //Check if the player is on the ground.
        GroundCheck();
        //If the player is on the ground...
        if (Controller.State.IsGrounded) {
            //...The player is not jumping.
            Controller.State.IsJumping = false;
            //...The player is not falling.
            Controller.State.IsFalling = false;
            //If the player is jumping...
            if (Controller.JumpButtonPressed) {
                //The player is jumping.
                Controller.State.IsJumping = true;
                //Apply a vertical force.
                rigidbody.AddForce(new Vector3(0f, (_jumpForce * _gravity) / 2, 0f), ForceMode.Impulse);
            }
        }
        //If the player was grounded, and is not jumping, and is not grounded...
        if (Controller.State.WasGrounded && !Controller.State.IsJumping && !Controller.State.IsGrounded) {
            //...Apply a downward force to keep the player on the ground.
            rigidbody.AddForce(new Vector3(0f, -_gravity, 0f), ForceMode.Impulse);
        }
        //...Else...
        else {
            //...Apply the force of gravity.
            rigidbody.AddForce(new Vector3(0f, -_gravity, 0f), ForceMode.Force);
        }
        //The player is falling if the vertical velocity is less than zero, and the player is not grounded.
        Controller.State.IsFalling = rigidbody.velocity.y < 0 && !Controller.State.IsGrounded;
        //Calculate the target velocity.
        Vector3 targetVelocity = CalculateTargetVelocity();
        //Calculate the force to apply.
        Vector3 force = CalculateForce(targetVelocity);
        //Apply a force to the rigidbody.
        if(!Controller.State.AirCollision) {
            if(!Controller.State.IsGrounded) {
                force = force * 0.02f;
            }
            rigidbody.AddForceAtPosition(force, this.transform.forward, ForceMode.VelocityChange);
        }
       
    }
    void LateUpdate() {
        RotateCamera();
    }
    private void RotateCamera() {
        if ((this.cameraTransform.localRotation.x < _lookLimit && Controller.CameraMovementY < 0) || (this.cameraTransform.localRotation.x > -_lookLimit && Controller.CameraMovementY > 0)) {
            this.cameraTransform.Rotate(new Vector3(-Controller.CameraMovementY * _cameraSensitivity.y, 0f, 0f));
        }
        this.transform.Rotate(new Vector3(0f, Controller.CameraMovementX * _cameraSensitivity.x, 0f), Space.World);
    }
    private float CalculateJumpForce() {
        //Return the jump force.
        return Mathf.Sqrt(2 * _jumpForce * _gravity);
    }
    private Vector3 CalculateTargetVelocity() {
        Vector3 targetVelocity = new Vector3(Controller.DirectionMovementX, 0f, Controller.DirectionMovementY);
        //Apply forward and backward movement speed modifiers to the target velocity.
        targetVelocity.z *= (targetVelocity.z > 0 ? _forwardSpeed : _backwardSpeed);
        //Apply the strafe movement modifier to the target velocity.
        targetVelocity.x *= _strafeSpeed;
        //Apply sprint speed modifier to the target velocity.
        targetVelocity *= (Controller.State.IsSprinting && Controller.State.IsGrounded ? _sprintMultiplyer : 1f);
        //Account for the direction the player is traveling in.
        targetVelocity = transform.TransformDirection(targetVelocity);
        //Return the target velocity.
        return targetVelocity;
    }
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
        Controller.State.WasGrounded = Controller.State.IsGrounded;
        //The player is grounded if the sphere collides with another collider.
        Controller.State.IsGrounded = (Physics.OverlapSphere(_ground.position, _groundRadius, _whatIsGround).Length) > 0 ? true : false;
    }
    private void OnCollisionEnter(Collision collision)  {
        Controller.State.AirCollision = !Controller.State.IsGrounded;
    }
    private void OnCollisionStay(Collision collision) {
        Controller.State.AirCollision = !Controller.State.IsGrounded;
    }
    private void OnCollisionExit() {
        //Set air collision to false.
        Controller.State.AirCollision = false;
    }
}