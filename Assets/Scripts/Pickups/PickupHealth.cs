using UnityEngine;

public class PickupHealth : Pickup {
    [SerializeField] private int _health;

    public int Health {
        get { return _health; }
        set { _health = value; }
    }

	override protected void Start () {
        base.Start();
	}
    protected override void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            EventManager.HealEvent(collision.gameObject.GetInstanceID(), Health);
            base.OnCollisionEnter(collision);
        }
    }
}