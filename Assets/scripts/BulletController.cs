using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float		speed,
						timeOut,
						damage;
	public bool			isFriendlyBullet;

	[HideInInspector]
	public GameObject	target,
	player;

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
		if (isFriendlyBullet && c.gameObject.layer == 10) {
			c.gameObject.GetComponent<EnemyAgentController>().health -= damage;
			DestroyObject(transform.gameObject);
		} else if (!isFriendlyBullet && c.gameObject.layer == 8) {
			c.gameObject.GetComponent<AgentController>().health -= damage;
			DestroyObject(transform.gameObject);
		}
	}
}
