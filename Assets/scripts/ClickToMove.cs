using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {

	public Camera cam;

	Ray ray;
	RaycastHit hit;

	void Update () {
		if (cam.pixelRect.Contains(Input.mousePosition) && Input.GetButtonDown("Fire1")) {
			ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				transform.position = hit.point;
			}
		}
	}
}
