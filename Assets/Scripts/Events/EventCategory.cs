using UnityEngine;
using System.Collections;

public class EventCategory : Singleton<EventCategory> {
    public enum WeaponEvent {Draw, Holster, Equip, Change, Shoot, Reload, Ammo, OutOfAmmo};
    public enum CharacterEvent {Spawn, Die, Heal, Damage, Action};
    public enum PickupEvent {Collect, Destroy};
    public enum HUDEvent { HealthUpdate, WeaponUpdate };
    public enum GameEvent {Start, Pause, GameOver, Quit};
    public enum LevelEvent {Load, Change};
}