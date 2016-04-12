using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

    [SerializeField] protected int _masterObjectID;
    //[SerializeField] protected int _objectID;

    public int MasterObjectID {
        get { return _masterObjectID; }
        set { _masterObjectID = value; }
    }
    virtual protected void OnEnable() {
        EventManager.RegisterMasterIDEventHandler += RegisterMasterID;
    }
    virtual protected void OnDisable() {
        EventManager.RegisterMasterIDEventHandler -= RegisterMasterID;
    }
    virtual protected void RegisterMasterID(int masterObjectID, int objectID) {
        if(!objectID.Equals(gameObject.GetInstanceID())) {
            return;
        }
        MasterObjectID = masterObjectID;
    }
}