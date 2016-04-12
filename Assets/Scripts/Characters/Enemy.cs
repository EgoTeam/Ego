using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character {
    //Data Members
    private     AudioSource[] _audioSources;    //A reference to the audio source components used by the enemy.
    protected   Animator _animator;
    protected Collider _collider;
    protected Rigidbody _rigidbody;
    [SerializeField] protected AnimationClip _deathClip;

    override protected void Awake () {
        base.Awake();
        _animator       = GetComponent<Animator>();
        _audioSources   = GetComponentsInChildren<AudioSource>();
        _dieTimeWait    = new WaitForSeconds(_deathClip.length + 5f);
        _collider       = GetComponent<Collider>();
        _rigidbody      = GetComponent<Rigidbody>();
    }

    override protected void Damage(int objectID, int damage) {
        //Invoke parent method.
        base.Damage(objectID, damage);
        //Play damage sound effect.
        //_audioSources[0].Play();
    }

    override protected IEnumerator DieCoroutine() {
        State.IsDying = true;
        _animator.SetTrigger("Die");
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        yield return _dieTimeWait;
        State.IsDead = true;
        Destroy(this.gameObject);
    }
}