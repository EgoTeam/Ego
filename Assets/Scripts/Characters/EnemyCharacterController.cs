using UnityEngine;
using System.Collections;

public class EnemyCharacterController : Enemy {

    private Inventory _inventory;
                        protected NavMeshAgent  _navigation;
    [SerializeField]    protected float         _direction;
    [SerializeField]    protected float         _chaseDistance;


    protected NavMeshAgent Navigation {
        get { return _navigation; }
        set { _navigation = value; }
    }
    public float Direction {
        get { return _direction; }
        set { _direction = value; }
    }
    public float ChaseDistance {
        get { return _chaseDistance; }
        set { _chaseDistance = value; }
    }

    virtual protected void Start() {
        _inventory = GetComponent<Inventory>();
        Navigation = GetComponent<NavMeshAgent>();
        //Set the default rotation of the Enemy.
        ChangeDirection(Direction);
        InvokeRepeating("CheckDistance", 0.0f, 2.0f);
    }
    virtual protected void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.CompareTag("Player") && !(State.IsDying || State.IsDead)) {
            EnemyState _enemyState = (EnemyState)State;
            _enemyState.ActionState = AIState.Chase;
            ChangeDirection(0);
            Navigation.SetDestination(Camera.main.transform.position);
            _animator.SetBool("IsMoving", true);
        }
    }
    virtual protected void OnTriggerStay(Collider collision) {
        if (collision.gameObject.CompareTag("Player") && !(State.IsDying || State.IsDead)) {
            EnemyState _enemyState = (EnemyState)State;
            _enemyState.ActionState = AIState.Chase;
            ChangeDirection(0);
            Navigation.SetDestination(Camera.main.transform.position);
            _animator.SetBool("IsMoving", true);
        }
    }

    virtual protected void Attack() {
        _animator.SetTrigger("Attack");
        EventManager.AttackEvent(_inventory.Weapon.gameObject.GetInstanceID(), false);
    }

    virtual protected void CheckDistance() {
        float distance = Vector3.Distance(gameObject.transform.position, Camera.main.transform.position);
        if (distance <= ChaseDistance) {

        }
        else if(distance <= _inventory.Weapon.Range) {
            //Set state to attack.
            Navigation.Stop();
            Attack();
        }
        else {
            //Set state to patrol.
        }

    }

    virtual protected void ChangeDirection(float direction) {
        Direction = direction;
        _animator.SetFloat("Direction", Direction);
    }
}