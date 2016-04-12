using UnityEngine;
using System.Collections;

public class EnemyCharacterController : Enemy {
    private Inventory _inventory;

    virtual protected void Start() {
        _inventory = GetComponent<Inventory>();
    }
    virtual protected void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.CompareTag("Player")) {
            _animator.SetTrigger("Attack");
            EventManager.AttackEvent(_inventory.Weapon.gameObject.GetInstanceID(), false);
           
        }
    }
    virtual protected void OnTriggerStay(Collider collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _animator.SetTrigger("Attack");
            EventManager.AttackEvent(_inventory.Weapon.gameObject.GetInstanceID(), false);
        }
    }
}