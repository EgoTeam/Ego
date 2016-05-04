using UnityEngine;
using System.Collections;

public class EnemyMeleeWeapon : Weapon {
    override protected void OnEnable() {
        EventManager.AttackEventHandler += Attack;
    }
    override protected void OnDisable() {
        EventManager.AttackEventHandler -= Attack;
    }
    override protected void Attack(int objectID, bool autoFire) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        if (CanAttack) {
            Debug.Log("Attacking!");
             StartCoroutine(AttackCoroutine());
        }
    }
    override protected IEnumerator AttackCoroutine() {
        CanAttack = false;
        HitScan();
        yield return new WaitForSeconds(_fireRate);
        CanAttack = true;
    }
    override protected void HitScan() {
        Transform enemyTransform = transform.parent.parent.parent;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Ray _ray = new Ray(enemyTransform.position, (-enemyTransform.position + playerTransform.position));
        RaycastHit hit;
        //Draw ray for debug purposes.
        Debug.DrawRay(_ray.origin, _ray.direction, Color.red, 10000f);
        //If the Raycast hit a collider...
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, Range)) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                EventManager.DamageEvent(hit.collider.gameObject.GetInstanceID(), Damage);
                Debug.Log("Player Hit!");
            }
        }
    }
}