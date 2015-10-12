using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float		speed,
						timeOut,
						damage;
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

	void OnTriggerEnter(Collider c) {
		// hurt enemies
		if (type == "friendly" && c.gameObject.layer == 10) {
			c.gameObject.GetComponent<EnemyAgentController>().health -= damage;
			DestroyObject(transform.gameObject);
		} else if (type == "enemy" && c.gameObject.layer == 8) {
			c.gameObject.GetComponent<AgentController>().health -= damage;
			DestroyObject(transform.gameObject);
		}
	}
}
