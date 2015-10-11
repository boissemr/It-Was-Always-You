using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour {

	// public parameters
	public GameObject	target,
						bullet;
	public float		health,
						attack,		// damage dealt per second
						reach;

	// private variables
	NavMeshAgent		agent;
	LayerMask			targets,
						enemies;
	RaycastHit[]		hit;
	GameObject			targetEnemy;

	[HideInInspector]
	public float	gameSpeed;
	public Vector3	previousPosition;

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
		// determining game speed
		gameSpeed = (transform.position - previousPosition).magnitude / Time.deltaTime;
		previousPosition = transform.position;

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
			Vector3 p1 = transform.position, p2 = targetEnemy.transform.position;
			GameObject o = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
			o.GetComponent<BulletController>().target = targetEnemy;
			o.GetComponent<BulletController>().type = "friendly";
			o.GetComponent<BulletController>().player = transform.gameObject;
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
