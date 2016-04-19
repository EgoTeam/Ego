using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character {
    //Data Members
                        protected AudioSource[]     _audioSources;    //A reference to the audio source components used by the enemy.
                        protected Animator          _animator;
                        protected CapsuleCollider   _capsuleCollider;
                        protected Rigidbody         _rigidbody;
    [SerializeField]    protected AnimationClip     _deathClip;

    override protected void Awake () {
        _state                  = GetComponent<EnemyState>();
        _animator               = GetComponent<Animator>();
        _audioSources           = GetComponentsInChildren<AudioSource>();
        _dieTimeWait            = new WaitForSeconds(_deathClip.length + 5f);
        _capsuleCollider        = GetComponent<CapsuleCollider>();
        _rigidbody              = GetComponent<Rigidbody>();
    }

    override protected void Damage(int objectID, int damage) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        //Invoke parent method.
        base.Damage(objectID, damage);
        //Play damage sound effect.
        _audioSources[3].Play();
    }

    override protected IEnumerator DieCoroutine() {
        State.IsDying = true;
        _animator.SetTrigger("Die");
        _audioSources[2].Play();
        _rigidbody.isKinematic = true;
        _capsuleCollider.enabled = false;
        yield return _dieTimeWait;
        State.IsDead = true;
        Destroy(this.gameObject);
    }
}