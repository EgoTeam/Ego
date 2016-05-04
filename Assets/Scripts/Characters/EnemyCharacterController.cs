using UnityEngine;
using System.Collections;

public class EnemyCharacterController : Enemy {

    private Inventory _inventory;
    [SerializeField]    protected float         _direction;


    public float Direction {
        get { return _direction; }
        set { _direction = value; }
    }
    override protected void OnEnable()
    {
        base.OnEnable();
        EventManager.AttackEventHandler += Attack;
        EventManager.PatrolEventHandler += Patrol;
        EventManager.ChaseEventHandler += Chase;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.AttackEventHandler -= Attack;
        EventManager.PatrolEventHandler -= Patrol;
        EventManager.ChaseEventHandler -= Chase;
    }
    virtual protected void Start() {
        _inventory = GetComponent<Inventory>();
        //Set the default rotation of the Enemy.
        ChangeDirection(Direction);
    }
    public void Patrol(int objectID) {
        if (objectID.Equals(gameObject.GetInstanceID()))
        {
            _animator.SetBool("IsMoving", true);
        }
    }
    public void Chase(int objectID) {
        if (objectID.Equals(gameObject.GetInstanceID()))
        {
            Patrol(objectID);
        }
    }
    virtual protected void Attack(int objectID, bool autoFire) {
        if (objectID.Equals(gameObject.GetInstanceID()))
        {
            _animator.SetBool("IsMoving", false);
            if (_inventory.Weapon.CanAttack)
            {
                _animator.SetTrigger("Attack");
            }
           
        }
    }
    virtual protected void OnAttack() { EventManager.AttackEvent(_inventory.Weapon.gameObject.GetInstanceID(), false); }

    virtual protected void ChangeDirection(float direction) {
        Direction = direction;
        _animator.SetFloat("Direction", Direction);
    }
}