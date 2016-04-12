using UnityEngine;
using System.Collections;
using System;

public abstract class Pickup : MonoBehaviour {

    protected AudioSource       _audioSource;
    protected Collider          _collider;
    protected SpriteRenderer    _spriteRenderer;

    virtual protected void Start () {
        _audioSource    = GetComponentInChildren<AudioSource>();
        _collider       = GetComponent<Collider>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
	}

    virtual protected void Collect() {
        _collider.enabled       = false;
        _spriteRenderer.enabled = false;
        _audioSource.Play();
    }

    virtual protected void DestroyPickup() {
        StartCoroutine(DestroyCoroutine());
    }

    virtual protected IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(_audioSource.clip.length);
        Destroy(this.gameObject);
    }

    virtual protected void OnCollisionEnter(Collision collision) {
        Collect();
        DestroyPickup();
    }
}