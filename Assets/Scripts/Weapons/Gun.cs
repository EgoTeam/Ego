using UnityEngine;
using System.Collections;

public class Gun : Weapon {

    [SerializeField] protected AnimationClip _reloadClip;
    [SerializeField] protected Ammunition _ammo;
    [SerializeField] protected Ammunition _ammoSource;

    public Ammunition Ammo {
        get { return _ammo; }
        set { _ammo = value; }
    }
    public Ammunition AmmoSource {
        get { return _ammoSource; }
        set { _ammoSource = value; }
    }
    override protected void OnEnable() {
        base.OnEnable();
        EventManager.AmmoQueryEventHandler += AmmunitionQuery;
    }
    override protected void OnDisable() {
        base.OnDisable();
        EventManager.AmmoQueryEventHandler -= AmmunitionQuery;
    }
    protected override IEnumerator DrawCoroutine() {
        _animator.SetFloat("Ammo", Ammo.Count);
        yield return StartCoroutine(base.DrawCoroutine());
    }
    protected override IEnumerator AttackCoroutine() {
        yield return StartCoroutine(base.AttackCoroutine());
        Ammo.Count -= 1;
        _animator.SetFloat("Ammo", Ammo.Count);
        EventManager.AmmoQueryEvent(gameObject.GetInstanceID());
    }
    protected void AmmunitionQuery(int objectID) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }

        if(Ammo.Count == 0 && AmmoSource.Count != 0) {
            Reload();
        }
        else if(Ammo.Count == 0 && AmmoSource.Count == 0) {
            CanAttack = false;
        }
        else {
            CanAttack = true;
        }
    }
    virtual protected void Reload() {
        StartCoroutine(ReloadCoroutine());
    }
    virtual protected IEnumerator ReloadCoroutine() {
        _animator.SetTrigger("Reload");
        //_audioSources[1].Play();
        yield return new WaitForSeconds(_reloadClip.length);
        int _ammountToReload = Ammo.Capacity - Ammo.Count;
        if(_ammountToReload > AmmoSource.Count) {
            _ammountToReload = AmmoSource.Count;
        }
        AmmoSource.Count -= _ammountToReload;
        Ammo.Count = _ammountToReload;
        _animator.SetFloat("Ammo", Ammo.Count);
        CanAttack = true;
    }
}