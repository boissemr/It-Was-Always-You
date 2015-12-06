using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	GameObject			target;
	float				gameSpeed;
	ParticleSystem[]	system;
	LayerMask			layerMask;
	RaycastHit[]		hit;

	void Start() {

		// claim a target
		layerMask = 1 << 8;
		hit = Physics.SphereCastAll(transform.position, 100, transform.forward, 0, layerMask);
		target = hit[0].collider.gameObject;

		// define system
		system = GetComponentsInChildren<ParticleSystem>();
	}

	void Update() {

		// game speed
		gameSpeed = target.GetComponent<AgentController>().gameSpeed;

		// animation speed
		foreach(ParticleSystem o in system) {
			o.playbackSpeed = gameSpeed/10;
		}
	
	}
}
