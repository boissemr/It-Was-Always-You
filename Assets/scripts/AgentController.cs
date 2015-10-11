using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour {

	// public parameters
	public GameObject	target;
	public float		health,
						attack,		// damage dealt per second
						reach;

	// private variables
	NavMeshAgent		agent;
	LayerMask			targets,
						enemies;
	RaycastHit[]		hit;
	GameObject			targetEnemy;

	// start
	void Start () {

		// layer masks
		enemies = 1 << 10;
		targets = 1 << 9;

		// claim a target
		hit = Physics.SphereCastAll(transform.position, 100, transform.forward, 0, targets);
		target = hit[0].collider.gameObject;
		
		// define nav mesh agent
		agent = GetComponent<NavMeshAgent>();
	}

	// update
	void Update () {
		// TODO optimize by only doing this stuff when necessary

		// set destination
		agent.SetDestination(target.transform.position);

		// find nearest enemy
		Vector3 targetPoint = transform.position + transform.forward*(reach/2);
		Debug.DrawLine(transform.position, targetPoint);
		hit = Physics.SphereCastAll(targetPoint, reach/2, transform.forward, 0, enemies);
		if (hit.Length > 0) {
			System.Array.Sort(hit, SortByDistance);
			targetEnemy = hit [0].collider.gameObject;
			Debug.DrawLine(transform.position, targetEnemy.transform.position);
		} else {
			targetEnemy = null;
		}

		// attack targeted enemy
		if (targetEnemy != null) {
			EnemyAgentController o = targetEnemy.GetComponent<EnemyAgentController>();

			// attack is in points per second
			// times speed/topspeed to scale with play start and stop
			o.health -= attack * Time.deltaTime * o.targetSpeed / agent.speed;
		}
	}

	void OnDisable() {
		GetComponent<NavMeshAgent>().enabled = false;
	}
	
	// sort two objects by distance from the player
	int SortByDistance(RaycastHit a, RaycastHit b) {
		float	aMagnitude = (transform.position - a.collider.transform.position).sqrMagnitude,
				bMagnitude = (transform.position - b.collider.transform.position).sqrMagnitude;
		return	aMagnitude.CompareTo(bMagnitude);
	}
}
