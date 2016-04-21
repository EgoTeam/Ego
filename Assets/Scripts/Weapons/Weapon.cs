using UnityEngine;
using System.Collections;

public class Weapon : InteractableObject {

    protected Animator      _animator;
    protected AudioSource[] _audioSources;
    protected SpriteRenderer _spriteRenderer;

    [SerializeField] protected WeaponClass    _weaponClass;
    [SerializeField] protected FireMechanism  _fireMechanism;
    [SerializeField] protected int            _damage;
    [SerializeField] protected int            _range;
    [SerializeField] protected float          _fireRate;
    [SerializeField] protected bool           _isCollected;
    [SerializeField] protected bool           _isEquipped;
    [SerializeField] protected bool           _canAttack;

    [SerializeField] protected AnimationClip _drawClip;
    [SerializeField] protected AnimationClip _holsterClip;
    [SerializeField] protected AnimationClip _attackClip;

    public WeaponClass Classification {
        get { return _weaponClass; }
    }
    public FireMechanism FireMode {
        get { return _fireMechanism; }
    }

    public int Damage {
        get { return _damage; }
    }
    public int Range {
        get { return _range; }
    }
    public float FireRate {
        get { return _fireRate; }
        set { _fireRate = value; }
    }
    public bool IsCollected {
        get { return _isCollected; }
        set { _isCollected = value; }
    }
    public bool IsEquipped {
        get { return _isEquipped; }
        set { _isEquipped = value;
            EventManager.EquipEvent(gameObject.GetInstanceID());
        }
    }
    public bool CanAttack {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    override protected void OnEnable() {
        base.OnEnable();
        EventManager.EquipEventHandler += Equip;
        EventManager.DrawEventHandler += Draw;
        EventManager.HolsterEventHandler += Holster;
        EventManager.AttackEventHandler += Attack;
    }
    override protected void OnDisable() {
        base.OnDisable();
        EventManager.EquipEventHandler -= Equip;
        EventManager.DrawEventHandler -= Draw;
        EventManager.HolsterEventHandler -= Holster;
        EventManager.AttackEventHandler -= Attack;
    }
    virtual protected void Awake() {
        _animator       = GetComponent<Animator>();
        _audioSources   = GetComponentsInChildren<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        FireRate = _attackClip.length;
    }
    virtual protected void Equip(int objectID) {
        if(!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        _spriteRenderer.enabled = IsEquipped;
    }
    virtual protected void Draw(int objectID) {
        if(!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        StartCoroutine(DrawCoroutine());
    }
    virtual protected IEnumerator DrawCoroutine() {
        IsEquipped = true;
        _animator.SetTrigger("Draw");
        yield return new WaitForSeconds(_drawClip.length);
        CanAttack = true;
    }
    virtual protected void Holster(int objectID) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        StartCoroutine(HolsterCoroutine());
    }
    virtual protected IEnumerator HolsterCoroutine() {
        CanAttack = false;
        _animator.SetTrigger("Holster");
        yield return new WaitForSeconds(_holsterClip.length);
        IsEquipped = false;
    }
    virtual protected void Attack(int objectID, bool autoFire) {
        if (!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        if (CanAttack) {
            if ((autoFire && FireMode.Equals(FireMechanism.Fullautomatic)) || (!autoFire)) {
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    virtual protected IEnumerator AttackCoroutine() {
        CanAttack = false;
        _animator.SetTrigger("Attack");
            _audioSources[0].Play();
        HitScan();
        yield return new WaitForSeconds(_fireRate);
    }
    virtual protected void HitScan() {
        Ray _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        //Draw ray for debug purposes.
        Debug.DrawRay(_ray.origin, _ray.direction, Color.green, 1000f);
        //If the Raycast hit a collider...
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, Range)) {
            if (hit.collider.gameObject.CompareTag("Enemy")) {
                EventManager.DamageEvent(hit.collider.gameObject.GetInstanceID(), Damage);
                //Debug.Log("Target Hit!");
            }
        }
    }
}