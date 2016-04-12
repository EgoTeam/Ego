using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour{

    [SerializeField] private List<Weapon>   _weapons;
    [SerializeField] private Weapon         _weapon;

    [SerializeField] private Ammunition _bullets;
    [SerializeField] private Ammunition _shells;
    [SerializeField] private Ammunition _grenades;

    public List<Weapon> Weapons {
        get { return _weapons; }
    }
    public Weapon Weapon {
        get { return _weapon; }
        set { _weapon = value;}
    }
    public Ammunition Bullets {
        get { return _bullets; }
    }
    public Ammunition Shells {
        get { return _shells; }
    }
    public Ammunition Grenades {
        get { return _grenades; }
    }
    void OnEnable() {
        EventManager.WeaponSwitchEventHandler += SwitchWeapons;
        EventManager.AmmoCollectEventHandler += AmmoCollect;
    }
    void OnDisable() {
        EventManager.WeaponSwitchEventHandler -= SwitchWeapons;
        EventManager.AmmoCollectEventHandler -= AmmoCollect;
    }
    void Start() {
        foreach(Weapon weapon in Weapons) {
            weapon.IsEquipped = false;
        }
        Weapon.IsEquipped = true;
        EventManager.DrawEvent(Weapon.gameObject.GetInstanceID());
    }
    private void SwitchWeapons(int objectID, int index) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        if(index < Weapons.Count) {
            if (Weapons[index].IsCollected) {
                StartCoroutine(SwitchWeaponsCoroutine(index));
            }
        }
    }
    private IEnumerator SwitchWeaponsCoroutine(int index)
    {
        EventManager.HolsterEvent(Weapon.gameObject.GetInstanceID());
        while (Weapon.IsEquipped)
        {
            yield return null;
        }
        Weapon = Weapons[index];
        EventManager.DrawEvent(Weapon.gameObject.GetInstanceID());
    }
    private void AmmoCollect(int objectID, Ammunition ammo) {
        switch (ammo.Classification) {
            case AmmunitionClassification.Bullet:
                Bullets.Count += ammo.Count;
                break;
            case AmmunitionClassification.Shell:
                Shells.Count += ammo.Count;
                break;
            case AmmunitionClassification.Grenade:
                Grenades.Count += ammo.Count;
                break;
        }
    }
}