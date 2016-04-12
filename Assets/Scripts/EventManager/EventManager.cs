using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

    public delegate void RegisterMasterID(int masterObjectID, int objectID);
    public static event RegisterMasterID RegisterMasterIDEventHandler;

    public delegate void Attack(int objectID, bool autoFire);
    public static event Attack AttackEventHandler;

    //public delegate void Reload(int masterObjectID, int objectID);
    //public static event Reload ReloadEventHandler;

    public delegate void Equip(int objectID);
    public static Equip EquipEventHandler;

    public delegate void Draw(int objectID);
    public static Draw DrawEventHandler;

    public delegate void Holster(int objectID);
    public static event Holster HolsterEventHandler;

    public delegate void WeaponSwitch(int objectID, int index);
    public static event WeaponSwitch WeaponSwitchEventHandler;

    //public delegate int AmmunitionRequest(int masterObjectID, int objectID, int ammunitionRequested);
    //public static event AmmunitionRequest AmmunitionRequestEventHandler;


    public delegate void Heal(int objectID, int health);
    public static event Heal HealEventHandler;

    public delegate void Damage(int objectID, int damage);
    public static event Damage DamageEventHandler;

    public delegate void Dying(int objectID);
    public static event Dying DyingEventHandler;

    public delegate void AmmoQuery(int objectID);
    public static event AmmoQuery AmmoQueryEventHandler;

    public delegate void AmmoUpdate(int objectID, Ammunition clip, Ammunition reserve);
    public static event AmmoUpdate AmmoUpdateEventHandler;

    public delegate void AmmoCollect(int objectID, Ammunition ammo);
    public static event AmmoCollect AmmoCollectEventHandler;

    public static void RegisterMasterIDEvent(int masterObjectID, int objectID) {
        RegisterMasterIDEventHandler(masterObjectID, objectID);
    }

    public static void EquipEvent(int objectID) {
        EquipEventHandler(objectID);
    }
    public static void AttackEvent(int objectID, bool autoFire) {
        AttackEventHandler(objectID, autoFire);
    }
    public static void DrawEvent(int objectID) {
        DrawEventHandler(objectID);
    }
    public static void HolsterEvent(int objectID) {
        HolsterEventHandler(objectID);
    }
    public static void WeaponSwitchEvent(int objectID, int index) {
        WeaponSwitchEventHandler(objectID, index);
    }
    public static void HealEvent(int objectID, int damage) { 
        HealEventHandler(objectID, damage);
    }
    public static void DamageEvent(int objectID, int damage) {
        DamageEventHandler(objectID, damage);
    }
    public static void DyingEvent(int objectID) {
        DyingEventHandler(objectID);
    }
    public static void AmmoCollectEvent(int objectID, Ammunition ammo) {
        AmmoCollectEventHandler(objectID, ammo);
    }
    public static void AmmoQueryEvent(int objectID) {
        AmmoQueryEventHandler(objectID);
    }
    public static void AmmoUpdateEvent(int objectID, Ammunition clip, Ammunition reserve)
    {
        AmmoUpdateEventHandler(objectID, clip, reserve);
    }
}