using UnityEngine;

public class PickupHealth : Pickup {
    [SerializeField] private int _health;

    public int Health {
        get { return _health; }
        set { _health = value; }
    }
	/// <summary>
    /// 
    /// </summary>
	override protected void Start () {
        base.Start();
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    protected override void OnCollect(GameObject character) {
        base.OnCollect(character);
        EventManager.Instance.PostNotification(EventCategory.CharacterEvent.Heal, character.GetComponent<Controller>(), Health);
        EventManager.Instance.PostNotification(EventCategory.PickupEvent.Destroy, this);
    }
}