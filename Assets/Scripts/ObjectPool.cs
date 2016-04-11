//Import Statements
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
	/*DATA MEMBERS*/
	public static 	ObjectPool instance; 			//The current instance of the ObjectPool object.
	public 			GameObject pooledObject;		//The GameObject being pooled.
	public 			int		   capacity		= 50;	//The maximum objects allowed in the pool.
	public 			bool	   isExpandable = true;	//True if the object pool will expand based on the objects needed.

	private List<GameObject> objectPool;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		InitializeObjectPool();
	}
	public GameObject GetObject() {
		foreach (GameObject obj in objectPool) {
			if (!obj.activeInHierarchy) {
				return obj;
			}
		}
		if (isExpandable) {
			GameObject obj = (GameObject)Instantiate(pooledObject);
			objectPool.Add(obj);
			return obj;
		}
		return null;
	}

	/**
	 * Method Name: InitializeObjectPool
	 * Description: Method initializes the ObjectPool List.
	 */ 
	private void InitializeObjectPool() {
		objectPool = new List<GameObject>();
		//For the capacity of the object pool...
		for (int i = 0; i < capacity; i++) {
			//...Create a temporary object of the pooled object.
			GameObject tempObject = (GameObject)Instantiate(pooledObject);
			//...Set the temporary object as inactive.
			tempObject.SetActive(false);
			//...Add the temporary object to the object pool.
			objectPool.Add(tempObject);
		}	
	}
}