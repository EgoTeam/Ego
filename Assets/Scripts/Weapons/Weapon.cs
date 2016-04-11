using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour, IListener {

    public enum WeaponType  {Punch = 0, Pistol = 1, ShotgunPistol = 2, Shotgun = 3, AssuatRifle = 4};
    public enum FireType    { Semiautomatic, Automatic };

    
    
    protected Animator          _animator;
    protected AudioSource[]     _audioSources;
    protected SpriteRenderer    _spriteRenderer;
    protected HUD               _hud;

                        public WeaponType Type;
    [SerializeField] protected FireType _mechanism;
    [SerializeField] protected int	    _damage;
	[SerializeField] protected int 	    _range;
	[SerializeField] protected float 	_useDelay;
    [SerializeField] protected float    _reloadTime;
    [SerializeField] protected int      _loadedAmmo;
    [SerializeField] protected int      _loadedAmmoCapacity;
    [SerializeField] protected int      _reserveAmmo;
    [SerializeField] protected int      _reserveAmmoCapacity;
    [SerializeField] protected bool     _isHeadshotInstantKill  = false;
    [SerializeField] protected bool     _isCollected            = false;
    [SerializeField] protected bool     _isEquipped             = false;
    [SerializeField] protected bool     _canUsePrimary          = true;


    [SerializeField] protected AnimationClip _holsterClip;
    [SerializeField] protected AnimationClip _drawClip;
    [SerializeField] protected AnimationClip _primaryActionClip;

    public FireType Mechanism {
        get { return _mechanism; }
    }
    public int Damage {
		get{ return _damage; }
		set{ _damage = value; }
	}

	public int Range {
		get{ return _range; }
		set{ _range = value; }
	}

	public float UseDelay {
		get { return _useDelay; }
		set { _useDelay = value; }
	}
    public int LoadedAmmo {
        get { return _loadedAmmo; }
        set {
            _loadedAmmo = Mathf.Clamp(value, 0, LoadedAmmoCapacity);
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Ammo, this);
        }
    }
    public int LoadedAmmoCapacity {
        get { return _loadedAmmoCapacity; }
    }

    public int ReserveAmmo {
        get { return _reserveAmmo; }
        set { _reserveAmmo = Mathf.Clamp(value, 0, ReserveAmmoCapacity); }
    }
    public int ReserveAmmoCapacity  {
        get { return _reserveAmmoCapacity; }
        set { _reserveAmmoCapacity = value; }
    }
    public bool IsCollected {
        get { return _isCollected; }
        set { _isCollected = value; }
    }
    public bool IsEquipped {
        get { return _isEquipped; }
        set { _isEquipped = value;
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Equip, this);
            EventManager.Instance.PostNotification(EventCategory.HUDEvent.WeaponUpdate, _hud, (Weapon)this);
        }
    }
    public bool CanUsePrimary {
        get { return _canUsePrimary; }
        set { _canUsePrimary = value; }
    }

	/// <summary>
    /// 
    /// </summary>
	virtual protected void Start () {
		_animator       = GetComponent<Animator>();
        _audioSources   = GetComponentsInChildren<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hud            = GetComponentInParent<HUD>();
        UseDelay = _primaryActionClip.length;
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Draw, this);
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Holster, this);
        EventManager.Instance.AddListener(EventCategory.WeaponEvent.Equip, this);
        EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Equip, this);
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
	virtual protected IEnumerator OnDraw() {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        //The weapon is equipped.
        IsEquipped = true;
        //Play the weapon draw animation.
        _animator.Play("Draw");
        //Yield until the animation is done playing.
        yield return new WaitForSeconds(_drawClip.length);
        //Enable the weapon to shoot.
        CanUsePrimary = true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
	virtual protected IEnumerator OnHolster() {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        //Enable the weapon to shoot.
        CanUsePrimary = false;
        //Play the weapon draw animation.
        _animator.Play("Holster");
        //Yield until the animation is done playing.
        yield return new WaitForSeconds(_holsterClip.length);
        //The weapon is not equipped.
        IsEquipped = false;
    }
    /// <summary>
    /// 
    /// </summary>
    virtual protected void OnEquip() {
        _spriteRenderer.enabled = IsEquipped;
       /* if (IsEquipped)
        {
            EventManager.Instance.PostNotification(EventCategory.WeaponEvent.Draw, this);
        }*/
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	virtual protected IEnumerator OnPrimaryOperation() {
        //The weapon cannot be shot.
        CanUsePrimary = false;
        //Play the Shoot Animation.
        _animator.Play("PrimaryAction");
        //Play the Shoot Sound.
        _audioSources[0].Play();
        //Calculate Hit
        HitScan();
        //Wait for use delay.
        yield return new WaitForSeconds(UseDelay);
    }
    /// <summary>
    /// 
    /// </summary>
	virtual protected IEnumerator OnSecondaryOperation() {
        yield return 0;
	}
    virtual protected void OnAmmo() {
        EventManager.Instance.PostNotification(EventCategory.HUDEvent.WeaponUpdate, _hud, (Weapon)this);
    }
    /// <summary>
    /// 
    /// </summary>
    virtual protected void HitScan() {
        //Get ray from screen center to target.
        Ray _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        //Test for ray collision.
        RaycastHit hit;
        //Draw ray for debug purposes.
        //Debug.DrawRay(_ray.origin, _ray.direction, Color.green, 1000f);
        //If the Raycast hit a collider...
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, Range)) {
            //...Check if the target is an enemy...
            if (hit.collider.gameObject.CompareTag("Enemy")) {
                //...Send damage message to enemy...
                EventManager.Instance.PostNotification(EventCategory.CharacterEvent.Damage, hit.collider.gameObject.GetComponent<Enemy>(), Damage);
                Debug.Log("Target Hit!");
            }
        }
    }
    virtual public void OnEvent(Enum type, Component sender, object param = null) {
        if (!this.GetInstanceID().Equals(sender.GetInstanceID())) {
            return;
        }
        switch ((EventCategory.WeaponEvent)type) {
            case EventCategory.WeaponEvent.Draw:
                StartCoroutine(OnDraw());
                break;
            case EventCategory.WeaponEvent.Holster:
                StartCoroutine(OnHolster());
                break;
            case EventCategory.WeaponEvent.Equip:
                OnEquip();
                break;
        }
    }
}