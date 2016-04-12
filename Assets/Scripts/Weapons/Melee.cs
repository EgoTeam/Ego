using UnityEngine;
using System.Collections;

public class Melee : Weapon {
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override IEnumerator AttackCoroutine() {
        yield return StartCoroutine(base.AttackCoroutine());
        CanAttack = true;
    }
}