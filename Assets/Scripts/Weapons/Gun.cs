using UnityEngine;
using System.Collections;
using System;

public class Gun : Weapon {

    [SerializeField] protected AnimationClip    _reloadClip;

    /// <summary>
    /// 
    /// </summary>
    override protected void Start() {
		base.Start();
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Shoot, this);
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Reload, this);
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.OutOfAmmo, this);
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Ammo, this);
        _reloadTime = _reloadClip.length;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator OnShoot(bool autoFire) {
        if (CanUsePrimary) {
            if ((autoFire && Mechanism == FireType.Automatic) || !autoFire) {
                yield return StartCoroutine(base.OnPrimaryOperation());
                //Decrement loaded ammo.
                LoadedAmmo -= 1;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    override protected void OnAmmo() {
        Debug.Log(LoadedAmmo);
        if(LoadedAmmo == 0 && ReserveAmmo != 0) {
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Reload, this);
        }
        else if(LoadedAmmo == 0 && ReserveAmmo == 0) {
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.OutOfAmmo, this);
        }
        else {
            CanUsePrimary = true;
        }
        base.OnAmmo();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnReload() {
        Debug.Log("Reload");
        //Play the Reload Animation.
        _animator.Play("Reload");
        //PLay the Reload Sound.
        _audioSources[1].Play();
        //Yield until the Reload animation is done playing.
        Debug.Log("Reload Animation Length: " + _reloadTime);
        yield return new WaitForSeconds(_reloadTime);
        //Determine the amount of ammo to reload.
		int _amountToReload =  LoadedAmmoCapacity - LoadedAmmo;
        //Set the amount of reserve ammo remaining after reloading.
		if (_amountToReload > ReserveAmmo) {
			_amountToReload = ReserveAmmo;
		}
        ReserveAmmo -= _amountToReload;
        //Set the amount of loaded ammo being reloaded.
        LoadedAmmo = _amountToReload;
        //The weapon can now shoot again.
        CanUsePrimary = true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="sender"></param>
    /// <param name="param"></param>
    public override void OnEvent(Enum type, Component sender, object param = null) {
        if (!this.GetInstanceID().Equals(sender.GetInstanceID())) {
            return;
        }
        base.OnEvent(type, sender, param);
        switch ((EventCategory.WeaponEvent)type) {
            case EventCategory.WeaponEvent.Reload:
                StartCoroutine(OnReload());
                break;
            case EventCategory.WeaponEvent.Shoot:
                StartCoroutine(OnShoot((bool)param));
                break;
		case EventCategory.WeaponEvent.OutOfAmmo:
			    CanUsePrimary = false;
                break;
           case EventCategory.WeaponEvent.Ammo:
                OnAmmo();
                break;
        }
    }
}