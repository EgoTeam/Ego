﻿using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	private new Transform camera;

	[SerializeField]
	private bool _isVerticalBillboard = false;

	public bool IsVerticalBillboard {
		get {return _isVerticalBillboard;}
		set {_isVerticalBillboard = value;}
	}

	void Start() {
		camera = Camera.main.transform;
	}

	void LateUpdate() {
		Vector3 LookAtDirection = new Vector3 (camera.position.x - this.transform.position.x, 0f, camera.position.z - this.transform.position.z);
		if (_isVerticalBillboard) {
			LookAtDirection.y = camera.position.y - this.transform.position.y;
		} 
		this.transform.rotation = Quaternion.LookRotation(-LookAtDirection.normalized, Vector3.up);
	}
}