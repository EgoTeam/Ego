using UnityEngine;
using System.Collections;
using System;

public abstract class Character : MonoBehaviour{
    protected Animator _animator;
                                        protected CharacterState    _state;
                                        protected WaitForSeconds    _dieTimeWait;           //The amount of time before the character completes it's dying behavior.
    [SerializeField][Range(000, 150)]   protected int               _health;                                        //The health of the character.
    [SerializeField][Range(000, 150)]   protected int               _healthMaximum;                                 //The health limit of the character.

    virtual protected void OnEnable() {
        EventManager.HealEventHandler   += Heal;
        EventManager.DamageEventHandler += Damage;
        EventManager.DyingEventHandler += Die;
    }
    virtual protected void OnDisable() {
        EventManager.DamageEventHandler -= Damage;
        EventManager.HealEventHandler   -= Heal;
        EventManager.DyingEventHandler -= Die;
    }
    public CharacterState State {
        get { return _state; }
    }
    virtual public int Health {
        get { return _health; }
        set {
            _health = Mathf.Clamp(value, 0, _healthMaximum);
            if (_health == 0 && !(State.IsDying || State.IsDead)) {
                EventManager.DyingEvent(gameObject.GetInstanceID());
            }
        }
    }
    public int HealthMaximum {
        get { return _healthMaximum; }
    }
    virtual protected void Awake () {
        _state      = GetComponent<CharacterState>();
        _dieTimeWait = new WaitForSeconds(0f);
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    virtual protected void Heal(int objectID, int health) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        if (!(State.IsDying || State.IsDead)) {
            Health += health;
        }
    }
    virtual protected void Damage(int objectID, int damage) {
        if(!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        Health -= damage;
    }
    virtual protected void Spawn(int objectID) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        //Empty Method.
    }
    virtual protected void Die(int objectID) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        StartCoroutine(DieCoroutine());
    }
    virtual protected IEnumerator DieCoroutine() {
        State.IsDying = true;
        _animator.enabled = true;
        _animator.Play("Death");
        yield return _dieTimeWait;
        State.IsDead = true;
        Destroy(this.gameObject);
    }
}