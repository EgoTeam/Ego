using UnityEngine;
using UnityEngine.UI;
using System;

public class HUD : MonoBehaviour {
    private Text _health;
    private Text _ammo;

   /* void OnEnable() {
        EventManager.HealEventHandler += HealthQuery;
        EventManager.AmmoUpdateEventHandler += AmmoQuery;
    }
    void OnDisable() {
        EventManager.HealEventHandler -= HealthQuery;
        EventManager.AmmoUpdateEventHandler -= AmmoQuery;
    }
    // Use this for initialization
    void Start () {
        //_health = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Text>();
       // _ammo   = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<Text>();
    }
	
    private void AmmoQuery(int objectID, Ammunition ammo, Ammunition source) {
        if (!objectID.Equals(gameObject.GetInstanceID()))
        {
            return;
        }
        _ammo.text = "A: " + ammo.Count + "/" + source.Count;
    }
   private void HealthQuery(int objectID, int health) {
        if (!objectID.Equals(gameObject.GetInstanceID()))
        {
            return;
        }
        _health.text = "H: " + health;
    }*/
}
