using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	Vector3 offset;

	void Start() {
		offset = target.localPosition - transform.localPosition;
	}

	void Update() {
		transform.localPosition = target.localPosition - offset;
	}

	void OnDisable() {
		offset = target.localPosition - transform.localPosition;
	}
}
