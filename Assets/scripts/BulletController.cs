using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float		speed,
						timeOut,
						damage;
	public bool			isFriendlyBullet;

	SoundManager		sound;

	[HideInInspector]
	public GameObject	target,
						player;

	void Start() {

		// aimed at target
		transform.LookAt(target.transform.position);

		// sound manager
		sound = GameObject.Find("soundManager").GetComponent<SoundManager>();
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

			sound.bulletHit();

			c.gameObject.GetComponent<EnemyAgentController>().health -= damage;
			if(c.GetComponent<EnemyAgentController>().health <= 0) {
				sound.enemyDeath();
			}
			DestroyObject(transform.gameObject);
		} else if (!isFriendlyBullet && c.gameObject.layer == 8) {

			sound.bulletHit();

			c.gameObject.GetComponent<AgentController>().health -= damage;
			if(c.GetComponent<AgentController>().health <= 0) {
				sound.playerDeath();
			}
			DestroyObject(transform.gameObject);
		}
	}
}
