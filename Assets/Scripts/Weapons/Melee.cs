using UnityEngine;
using System.Collections;
using System;

public class Melee : Weapon {

	/// <summary>
    /// 
    /// </summary>
	override protected void Start () {
        base.Start();
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Shoot, this);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator OnMelee() {
        yield return StartCoroutine(base.OnPrimaryOperation());
        CanUsePrimary = true;
    }
    public override void OnEvent(Enum type, Component sender, object param = null) {
        if (!this.GetInstanceID().Equals(sender.GetInstanceID())) {
            return;
        }
        base.OnEvent(type, sender, param);
        switch((EventCategory.WeaponEvent) type) {
            case EventCategory.WeaponEvent.Shoot:
                StartCoroutine(OnMelee());
                break;
        }
    }
}