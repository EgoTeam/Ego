using UnityEngine;
using UnityEngine.UI;
using System;

public class HUD : MonoBehaviour {
    private Text _health;
    private Text _ammo;

    // Use this for initialization
    void Start () {
        _health = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Text>();
        _ammo   = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<Text>();
       // EventManager.Instance.AddListener(EventCategory.HUDEvent.WeaponUpdate, this);
        //EventManager.Instance.AddListener(EventCategory.HUDEvent.HealthUpdate, this);
    }
	
    private void OnWeaponUpdate(Weapon weapon) {
       // _ammo.text = "A: " + weapon.LoadedAmmo + "/" + weapon.ReserveAmmo;
    }
   /* private void OnHealthUpdate(Controller controller) {
        _health.text = "H: " + controller.Health + "/" + controller.HealthMaximum;
    }
    public void OnEvent(Enum type, Component sender, object param = null) {
        switch ((EventCategory.HUDEvent)type) {
            case EventCategory.HUDEvent.WeaponUpdate:
                OnWeaponUpdate((Weapon)param);
                break;
            case EventCategory.HUDEvent.HealthUpdate:
                OnHealthUpdate((Controller)param);
                break;
        }
    }*/
}
