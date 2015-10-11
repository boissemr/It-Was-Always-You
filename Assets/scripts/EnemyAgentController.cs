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
	public float		targetSpeed,
						targetDistance;
	Vector3				targetPosition,
						targetPreviousPosition;

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
		// target attributes
		targetPreviousPosition = targetPosition;
		targetPosition = target.transform.position;
		targetSpeed = (targetPosition - targetPreviousPosition).magnitude / Time.deltaTime;
		targetDistance = (targetPosition - transform.position).magnitude;
		
		// engage, retreat, or give up pursuit
		if (targetDistance < engagementRange) {
			agent.SetDestination(targetPosition);
			agent.speed = targetSpeed * speed;
			if (targetDistance < retreatRange) {
				agent.speed = targetSpeed * -speed;
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
