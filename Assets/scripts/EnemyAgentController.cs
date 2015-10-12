using UnityEngine;
using System.Collections;

public class EnemyAgentController : MonoBehaviour {

	// public parameters
	public GameObject	target,
						bullet;
	public float		health,
						speed,
						attack,
						engagementRange,
						retreatRange,
						attackRange,
						fireRate;
	public bool			isDead;

	// private variables
	NavMeshAgent		agent;
	RaycastHit[]		hit;
	int					layerMask;
	float				gameSpeed,
						fireTimer;

	[HideInInspector]
	public float		targetDistance;

	// start
	void Start() {
		// claim a target
		layerMask = 1 << 8;
		hit = Physics.SphereCastAll(transform.position, 100, transform.forward, 0, layerMask);
		target = hit[0].collider.gameObject;

		// define nav mesh agent
		agent = GetComponent<NavMeshAgent>();
	}

	// update
	void Update() {
		gameSpeed = target.GetComponent<AgentController> ().gameSpeed;

		if (isDead) {
			doWhileDead();
		} else {
			doWhileAlive();
		}
	}
	
	void OnDisable() {
		GetComponent<NavMeshAgent>().enabled = false;
	}

	// stuff to do while alive
	void doWhileAlive() {
		targetDistance = (target.transform.position - transform.position).magnitude;

		// engage, retreat, or give up pursuit
		if (targetDistance < engagementRange) {
			agent.SetDestination(target.transform.position);
			agent.speed = gameSpeed * speed;
			if (targetDistance < retreatRange) {
				agent.speed = gameSpeed * -speed;
			}
		} else {
			agent.speed = 0;
		}

		// attack the player
		if (targetDistance < attackRange) {
			fireTimer -= Time.deltaTime * gameSpeed;
			if(fireTimer <= 0) {
				fireTimer += fireRate;
				BulletController o = ((GameObject)Instantiate(bullet, transform.position, transform.rotation)).GetComponent<BulletController>();
				o.target = target;
				o.type = "enemy";
				o.player = target;
				o.damage = attack;
			}
		} else {
			fireTimer = 0;
		}

		// die if health is below 0
		if (health <= 0) setAlive(false);
	}

	// stuff to do while dead
	void doWhileDead() {
		// resurrection if health is above 0
		if (health > 0) setAlive(true);
	}

	// set renderer and collider
	void setAlive(bool state) {
		isDead = !state;
		GetComponent<MeshRenderer>().enabled = state;
		GetComponent<NavMeshAgent>().enabled = state;
		GetComponent<CapsuleCollider>().enabled = state;
	}
}
