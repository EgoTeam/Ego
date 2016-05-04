using UnityEngine;
using System.Collections;

public class SountrackTrigger : MonoBehaviour {

    private SoundtrackManager _manager;
    private BoxCollider _collider;

	// Use this for initialization
	void Start () {
        _manager = GetComponentInParent<SoundtrackManager>();
        _collider = GetComponent<BoxCollider>();
	}


    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            _collider.enabled = false;
            _manager.AddNextToQueue();
        }
    }
}
