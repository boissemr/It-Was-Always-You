using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour {

	// public parameters
	public GameObject	target,
						bullet;
	public float		health,
						reach,
						fireRate;

	// private variables
	NavMeshAgent		agent;
	LayerMask			targets,
						enemies;
	RaycastHit[]		hit;
	GameObject			targetEnemy;
	float				fireTimer;
	Animator			animator;


	public float	gameSpeed;
	[HideInInspector]
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
		
		// define animator
		animator = GetComponentInChildren<Animator>();
	}

	// update
	void Update () {		
		// determining game speed
		gameSpeed = (transform.position - previousPosition).magnitude / Time.deltaTime;
		previousPosition = transform.position;

		// animation
		if(gameSpeed == 0) {
			animator.enabled = false;
		} else {
			animator.enabled = true;
		}

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
		if(fireTimer <= 0) {
			if (targetEnemy != null) {
				fireTimer += fireRate;
				BulletController o = ((GameObject)Instantiate(bullet, transform.position, transform.rotation)).GetComponent<BulletController>();
				o.target = targetEnemy;
				o.player = transform.gameObject;
			}
		} else {
			fireTimer -= Time.deltaTime * gameSpeed;
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
