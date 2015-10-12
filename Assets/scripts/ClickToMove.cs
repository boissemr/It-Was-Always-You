using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {

	public Camera cam;

	Ray ray;
	RaycastHit hit;
	LayerMask ground;

	void Start() {
		ground = 1<<11;
	}

	void Update () {
		if (cam.pixelRect.Contains(Input.mousePosition) && Input.GetButton("Fire1")) {
			ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100, ground)) {
				transform.position = hit.point;
			}
		}
	}
}
