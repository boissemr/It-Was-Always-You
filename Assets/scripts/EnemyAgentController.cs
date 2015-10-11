using UnityEngine;
using System.Collections;

public class EnemyAgentController : MonoBehaviour {

	// public parameters
	public GameObject	target;
	public float		health,
						speed,
						engagementRange,
						retreatRange;
	public bool			isDead;

	// private variables
	NavMeshAgent		agent;
	RaycastHit[]		hit;
	int					layerMask;

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
		if (isDead) {
			doWhileDead();
		} else {
			doWhileAlive();
		}
	}

	// stuff to do while alive
	void doWhileAlive() {
		// engage, retreat, or give up pursuit
		targetDistance = (target.transform.position - transform.position).magnitude;
		if (targetDistance < engagementRange) {
			agent.SetDestination(target.transform.position);
			agent.speed = target.GetComponent<AgentController>().gameSpeed * speed;
			if (targetDistance < retreatRange) {
				agent.speed = target.GetComponent<AgentController>().gameSpeed * -speed;
			}
		} else {
			agent.speed = 0;
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
