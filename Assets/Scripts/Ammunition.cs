using UnityEngine;
using System.Collections;

public class Ammunition : MonoBehaviour {
    [SerializeField] private AmmunitionClassification _classification;
    [SerializeField] private int _count;
    [SerializeField] private int _capacity;

    public int Count {
        get { return _count; }
        set { _count = Mathf.Clamp(value, 0, Capacity); }
    }

    public int Capacity {
        get { return _capacity; }
    }

    public AmmunitionClassification Classification {
        get { return _classification; }
    }
}