using UnityEngine;
using UnityEngine.UI;
using System;

public class HUD : MonoBehaviour {
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ControllableCharacterController _player;
    [SerializeField] private Image _health;
    [SerializeField] private Image _loaded;
    [SerializeField ]private Image _remain;

    void Start()
    {
        _health = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<Image>();
        _loaded = GameObject.FindGameObjectWithTag("Loaded Ammo Bar").GetComponent<Image>();
        _remain = GameObject.FindGameObjectWithTag("Reserve Ammo Bar").GetComponent<Image>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableCharacterController>();
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void LateUpdate() {
        if(_player != null) {
            _health.fillAmount = ((float)_player.Health / (float)_player.HealthMaximum);
        }
        else {
           _health.fillAmount = 0f;
        }
        if(_player != null ||_inventory.Weapon != null || _inventory != null) {
            if(_inventory.Weapon.Classification.Equals(WeaponClass.Melee)) {
                _loaded.fillAmount = 1f;
                _remain.fillAmount = 1f;
            }
            else {
                Gun gunObject = _inventory.Weapon as Gun;
                _loaded.fillAmount = ((float)gunObject.Ammo.Count / (float)gunObject.Ammo.Capacity);
                _remain.fillAmount = ((float)gunObject.AmmoSource.Count / (float)gunObject.AmmoSource.Capacity);
            }
        }
        else {
            _loaded.fillAmount = 0f;
            _remain.fillAmount = 0f;
        }
    }
}