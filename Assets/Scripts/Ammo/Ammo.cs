using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    public enum AmmoType {Bullet, Shell, Grenade};

    [SerializeField] protected AmmoType   _type;
    [SerializeField] protected int        _count;
    [SerializeField] protected int        _capacity;

    public AmmoType Type {
        get { return _type; }
        set { _type = value; }
    }

    public int Count {
        get { return _count; }
        set { _count = Mathf.Clamp(value, 0, Capacity); }
    }

    public int Capacity {
        get { return _capacity; }
        set { _capacity = value; }
    }
}