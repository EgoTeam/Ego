using UnityEngine;
using System.Collections;
using System;

public abstract class Pickup : MonoBehaviour, IListener {

    protected AudioSource       _audioSource;
    protected Collider          _collider;
    protected SpriteRenderer    _spriteRenderer;
    /// <summary>
    /// 
    /// </summary>
    virtual protected void Start () {
        _audioSource    = GetComponentInChildren<AudioSource>();
        _collider       = GetComponent<Collider>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.Instance.AddListener(EventCategory.PickupEvent.Collect, this);
        EventManager.Instance.AddListener(EventCategory.PickupEvent.Destroy, this);
	}
    /// <summary>
    /// 
    /// </summary>
    virtual protected void OnCollect(GameObject character) {
        _collider.enabled       = false;
        _spriteRenderer.enabled = false;
        _audioSource.Play();
    }
    /// <summary>
    /// 
    /// </summary>
    virtual protected void OnDestroyPickup() {
        StartCoroutine(DestroyCoroutine());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(_audioSource.clip.length);
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    virtual protected void OnCollisionEnter(Collision collision) {
        if(!collision.gameObject.CompareTag("Player")) {
            return;
        }
        EventManager.Instance.PostNotification(EventCategory.PickupEvent.Collect, this, collision.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    virtual protected void OnDestroy() {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="sender"></param>
    /// <param name="param"></param>
    virtual public void OnEvent(Enum type, Component sender, object param = null) {
        if(!this.GetInstanceID().Equals(sender.GetInstanceID())) {
            return;
        }
        switch((EventCategory.PickupEvent) type) {
            case EventCategory.PickupEvent.Collect:
                OnCollect((GameObject) param);
                break;
            case EventCategory.PickupEvent.Destroy:
                OnDestroyPickup();
                break;
        }
    }
}