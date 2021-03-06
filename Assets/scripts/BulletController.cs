﻿using UnityEngine;
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

			sound.playSound(sound.friendlyBulletHitSFX);

			c.gameObject.GetComponent<EnemyAgentController>().health -= damage;
			if(c.GetComponent<EnemyAgentController>().health <= 0) {
				sound.playSound(sound.enemyDeathSFX);
				for(int i = 0; i < player.GetComponent<AgentController>().moneyMultiplier; i++) {
					c.GetComponent<EnemyAgentController>().dropLoot();
				}
			}
			DestroyObject(transform.gameObject);
		} else if (!isFriendlyBullet && c.gameObject.layer == 8) {

			sound.playSound(sound.enemyBulletHitSFX);

			if(c.gameObject.GetComponent<AgentController>().invincibilityTimer <= 0) {
				c.gameObject.GetComponent<AgentController>().health -= damage;
			}
			c.gameObject.GetComponent<AgentController>().resetInvincibilityTimer();
			if(c.GetComponent<AgentController>().health <= 0) {
				sound.playSound(sound.playerDeathSFX);
			}
			DestroyObject(transform.gameObject);
		}
	}
}
