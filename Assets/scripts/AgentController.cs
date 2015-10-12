using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour {

	// public parameters
	public GameObject	target,
						bullet;
	public float		health,
						attack,		// damage in each shot
						reach,
						fireRate;	// time between shots

	// private variables
	NavMeshAgent		agent;
	LayerMask			targets,
						enemies;
	RaycastHit[]		hit;
	GameObject			targetEnemy;
	float				fireTimer;

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
			fireTimer -= Time.deltaTime * gameSpeed;
			if(fireTimer <= 0) {
				fireTimer += fireRate;
				BulletController o = ((GameObject)Instantiate(bullet, transform.position, transform.rotation)).GetComponent<BulletController>();
				o.target = targetEnemy;
				o.type = "friendly";
				o.player = transform.gameObject;
				o.damage = attack;
			}
		} else {
			fireTimer = 0;
		}

		// die
		if (health <= 0) {
			transform.parent.gameObject.SetActive(false);
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
