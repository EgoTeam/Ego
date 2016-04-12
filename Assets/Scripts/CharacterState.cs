using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour {
    //Define State Settings.
    [SerializeField] protected bool _wasGrounded;                 
    [SerializeField] protected bool _isGrounded = false;                   
    [SerializeField] protected bool _isJumping;                  
    [SerializeField] protected bool _isFalling;
    [SerializeField] protected bool _airCollison;        
    [SerializeField] protected bool _isIdle;                      
    [SerializeField] protected bool _isWalking;                    
    [SerializeField] protected bool _isSprinting;
    [SerializeField] protected bool _isDying;
    [SerializeField] protected bool _isDead;

    public bool WasGrounded {
        get { return _wasGrounded; }
        set { _wasGrounded = value; }
    }
    public bool IsGrounded {
        get { return _isGrounded; }
        set { _isGrounded = value; }
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
    public bool AirCollision {
        get { return _airCollison; }
        set { _airCollison = value; }
    }
    public bool IsDying {
        get { return _isDying; }
        set { _isDying = value; }
    }
    public bool IsDead {
        get { return _isDead; }
        set { _isDead = value; }
    }
}