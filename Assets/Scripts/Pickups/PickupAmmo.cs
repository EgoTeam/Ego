using UnityEngine;
using System.Collections;

public class PickupAmmo : Pickup {

    [SerializeField] private Ammunition _ammo;

    public Ammunition Ammo {
        get { return _ammo; }
        set { _ammo = value; }
    }

    override protected void Start() {
        _ammo = GetComponentInChildren<Ammunition>();
        base.Start();
    }
    protected override void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            EventManager.AmmoCollectEvent(collision.gameObject.GetInstanceID(), Ammo);
            base.OnCollisionEnter(collision);
        }
    }
}
