using UnityEngine;
using System.Collections;

public class CharacterActionController : Character {
  /*  private Inventory _inventory;

    [SerializeField] private Weapon _equippedWeapon;

    public Inventory Inventory {
        get { return _inventory; }
        set { _inventory = value; }
    }
    public Weapon EquippedWeapon {
        get { return _equippedWeapon; }
        set { _equippedWeapon = value;}
    }

    // Use this for initialization
	void Awake () {
        State       = GetComponent<CharacterState>();
        Inventory   = GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(State.FireButtonPressed || State.FireButtonHeld) {
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Shoot, EquippedWeapon, State.FireButtonHeld);
        }
        if(State.ActionButtonHeld || State.ActionButtonPressed) {
            EventManager.Instance.PostNotification(EventCategory.CharacterEvent.Action, this, State.ActionButtonHeld);
        }
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator OnWeaponChange(int index) {
        if (_inventory.Weapons[index].IsEquipped && _inventory.Weapons[index].IsCollected) {
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Holster, EquippedWeapon);
            State.IsHolsteringWeapon = true;
            while (EquippedWeapon.IsEquipped) {
                yield return 0;
            }
            State.IsHolsteringWeapon = false;
            EquippedWeapon = _inventory.Weapons[index];
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Draw, EquippedWeapon);
            State.IsDrawingWeapon = true;
            while (!EquippedWeapon.CanUsePrimary) {
                yield return 0;
            }
            State.IsDrawingWeapon = false;
        }
        yield return 0;
    }*/
}