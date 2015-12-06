using UnityEngine;
using System.Collections;

public class EnemyAgentController : MonoBehaviour {

	// public parameters
	public GameObject	target,
						bullet;
	public float		health,
						speed,
						engagementRange,
						retreatRange,
						attackRange,
						fireRate;
	public bool			returnToPost,
						alwaysFacePlayer,
						wander,
						isDead;

	// private variables
	NavMeshAgent		agent;
	RaycastHit[]		hit;
	int					layerMask;
	float				gameSpeed,
						fireTimer,
						wanderTimer;
	Vector3				startPosition;

	[HideInInspector]
	public float		targetDistance;

	// start
	void Start() {
		// assign start position for returnToPost enemies
		startPosition = transform.position;

		// claim a target
		layerMask = 1 << 8;
		hit = Physics.SphereCastAll(transform.position, 200, transform.forward, 0, layerMask);
		target = hit[0].collider.gameObject;

		// define nav mesh agent
		agent = GetComponent<NavMeshAgent>();
	}

	// update
	void Update() {
		gameSpeed = target.GetComponent<AgentController>().gameSpeed;

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

		// moving around
		// !TODO add better commenting
		targetDistance = (target.transform.position - transform.position).magnitude;
		if(targetDistance < engagementRange) {
			agent.SetDestination(target.transform.position);
			agent.speed = gameSpeed * speed;
			if(!returnToPost) {
				startPosition = transform.position;
			}
			if(targetDistance < retreatRange) {
				agent.speed = gameSpeed * -speed;
			}
		} else if(returnToPost) {
			agent.SetDestination(startPosition);
			agent.speed = gameSpeed * speed;
		} else {
			if(wander) {
				if(wanderTimer <= 0) {
					wanderTimer = Random.value * 100;
					Vector3 wanderDestination;
					wanderDestination = transform.position + new Vector3(Random.value-.5f, 0, Random.value-.5f) * 5;
					if((wanderDestination - startPosition).magnitude < 10) {
						agent.SetDestination(wanderDestination);
					} else {
						agent.SetDestination(startPosition);
					}
				} else {
					wanderTimer -= gameSpeed * Time.deltaTime;
				}
				agent.speed = gameSpeed * speed;
			}
		}

		// attack the player
		if(fireTimer <= 0) {
			if(targetDistance < attackRange) {
				fireTimer += fireRate;
				BulletController o = ((GameObject)Instantiate(bullet, transform.position, transform.rotation)).GetComponent<BulletController> ();
				o.target = target;
				o.player = target;
			}
		} else {
			fireTimer -= Time.deltaTime * gameSpeed;
		}

		// face player
		if(alwaysFacePlayer) {
			transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
		}

		// die if health is below 0
		if(health <= 0) setAlive(false);
	}

	// stuff to do while dead
	void doWhileDead() {

		// resurrection if health is above 0
		if(health > 0) setAlive(true);
	}

	// set renderer and collider
	void setAlive(bool state) {
		isDead = !state;
		GetComponent<MeshRenderer>().enabled = state;
		GetComponent<NavMeshAgent>().enabled = state;
		GetComponent<CapsuleCollider>().enabled = state;
	}

	// start and stop agent
	public void startAgent() {
		agent.Stop();
	}
	public void stopAgent() {
		agent.SetDestination(target.transform.position);
	}
}
