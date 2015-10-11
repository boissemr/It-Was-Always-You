using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float		speed,
						timeOut;
	public GameObject	target,
						player;
	public string		type;

	void Start() {
		// aimed at target
		transform.LookAt(target.transform.position);
	}

	void Update() {
		// move
		transform.position += transform.forward * speed * Time.deltaTime * player.GetComponent<AgentController>().gameSpeed;

		// time out
		timeOut -= player.GetComponent<AgentController>().gameSpeed * Time.deltaTime;
		if(timeOut < 0) {
			DestroyObject(transform.gameObject);
		}
	}
}
