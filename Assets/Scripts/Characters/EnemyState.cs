using UnityEngine;
using System.Collections;

public class EnemyState : CharacterState {
    [SerializeField] protected AIState _actionState;

    public AIState ActionState {
        get { return _actionState; }
        set { _actionState = value; }
    }
}
