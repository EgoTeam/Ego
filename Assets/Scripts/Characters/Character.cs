using UnityEngine;
using System.Collections;
using System;

public abstract class Character : MonoBehaviour, IListener {
    //Data Members
                                        protected WaitForSeconds    _dieTimeWait        = new WaitForSeconds(0f);   //The amount of time before the character completes it's dying behavior.
    [SerializeField][Range(000, 150)]   protected int               _health;                                        //The health of the character.
    [SerializeField][Range(000, 150)]   protected int               _healthMaximum;                                 //The health limit of the character.
                                        protected bool              _isDying;                                       //True if the character is dying but not yet dead, false otherwise.
                                        protected bool              _isDead;                                        //True if the character is dead, false otherwise.

    /// <summary>
    /// Method enables the health of the character to be retreived and set.
    /// </summary>
    virtual public int Health {
        get { return _health; }
        set {
            //Clamp the health value between 0 and the maximum amount of health.
            _health = Mathf.Clamp(value, 0, _healthMaximum);
            //If the health of the character is zero and the character is not dying nor dead...
            if (_health == 0 && !(IsDying || IsDead)) {
                //...Post a notification to the character to die.
                EventManager.Instance.PostNotification(EventCategory.CharacterEvent.Die, this);
            }
        }
    }
    /// <summary>
    /// Method enables the maximum health of the character to be retreived.
    /// </summary>
    public int HealthMaximum {
        get { return _healthMaximum; }
    }
    public bool IsDying {
        get { return _isDying; }
        set { _isDying = value; }
    }
    /// <summary>
    /// Method enables the boolean value of whether the character is dead to be retreived and set.
    /// </summary>
    public bool IsDead {
        get { return _isDead; }
        set { _isDead = value; }
    }

    /// <summary>
    /// Method executes once on Object activation.
    /// </summary>
    virtual protected void Start () {
        //Register event listener for spawn.
        EventManager.Instance.AddListener(EventCategory.CharacterEvent.Spawn, this);
        //Register event listener for die.
        EventManager.Instance.AddListener(EventCategory.CharacterEvent.Die, this);
        //Register event listener for heal.
        EventManager.Instance.AddListener(EventCategory.CharacterEvent.Heal, this);
        //Register event listener for damage.
        EventManager.Instance.AddListener(EventCategory.CharacterEvent.Damage, this);
    }
    /// <summary>
    /// Method determines character behavior based on an event occuring.
    /// </summary>
    /// <param name="type">The type of event.</param>
    /// <param name="sender">The object invoking the event.</param>
    /// <param name="param">The optional paramater value.</param>
    virtual public void OnEvent(Enum type, Component sender, object param = null) {
        if(!this.GetInstanceID().Equals(sender.GetInstanceID())) {
            return;
        }
        switch ((EventCategory.CharacterEvent)type) {
            case EventCategory.CharacterEvent.Heal:
                OnHeal((int)param);
                break;
            case EventCategory.CharacterEvent.Damage:
                OnDamage((int)param);
                break;
            case EventCategory.CharacterEvent.Die:
                StartCoroutine(OnDie());
                break;
            case EventCategory.CharacterEvent.Spawn:
                break;
        }
    }
    /// <summary>
    /// Method handles character behavior on healing.
    /// </summary>
    /// <param name="health">The amount of health the character is receiving.</param>
    virtual protected void OnHeal(int health) {
        //Increment the health of the character by the health received.
        Health += health;
    }
    /// <summary>
    /// Method handles character behavior on receiving damage.
    /// </summary>
    /// <param name="damage">The amount of damage the character is receiving.</param>
    virtual protected void OnDamage(int damage) {
        //Decrement the health of the character by the damage received.
        Health -= damage;
    }
    /// <summary>
    /// Method handles character behavior on character spawn.
    /// </summary>
    virtual protected void OnSpawn(Component sender) {

    }
    /// <summary>
    /// Method handles character behavior when dying.
    /// </summary>
    virtual protected IEnumerator OnDie() {
        IsDying = true;
        yield return _dieTimeWait;
        IsDead = true;
        Destroy(this.gameObject);
    }
}