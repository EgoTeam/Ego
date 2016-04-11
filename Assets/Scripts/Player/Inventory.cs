using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour{

    [SerializeField] private List<Weapon>   _weapons;
    [SerializeField] private int _bulletCount;
    [SerializeField] private int _bulletCapacity;
    [SerializeField] private int _shellCount;
    [SerializeField] private int _shellCapacity;
    [SerializeField] private int _grenadeCount;
    [SerializeField] private int _grenadeCapacity;

    public List<Weapon> Weapons {
        get { return _weapons; }
    }
    public int BulletCount {
        get { return _bulletCount; }
        set { _bulletCount = value; }
    }
    public int BulletCapacity {
        get { return _bulletCapacity; }
    }
    public int ShellCount {
        get { return _shellCount; }
        set { _shellCount = value; }
    }
    public int ShellCapacity {
        get { return _shellCapacity; }
    }
    public int GrenadeCount {
        get { return _grenadeCount; }
        set { _grenadeCount = value; }
    }
    public int GrenadeCapacity {
        get { return _grenadeCapacity; }
    }
    // Use this for initialization
    void Awake () {
        DontDestroyOnLoad(gameObject);
	}
}