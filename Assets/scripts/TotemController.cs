using UnityEngine;
using System.Collections;

public class TotemController : MonoBehaviour {

	// public parameters
	public bool		isUsed;

	// private variables
	GameObject		oldGroup,
					newGroup;
	Transform[]		oldChildren,
					newChildren;
	GameObject[]	groups;
	float			DEBUG_TIMER;

	// start
	void Start() {
		isUsed = false;
		DEBUG_TIMER = 10;
	}

	// activate
	void Awake() {
		DEBUG_TIMER = 10;
	}

	// update
	void Update() {

		// make this totem invisible
		if(isUsed) {
			foreach(MeshRenderer o in this.transform.parent.gameObject.GetComponentsInChildren<MeshRenderer>()) o.enabled = false;
			foreach(Light o in this.transform.parent.gameObject.GetComponentsInChildren<Light>()) o.enabled = false;
		}

		// DEBUG TIMER
		DEBUG_TIMER -= 1;
	}

	// trigger enter
	void OnTriggerEnter(Collider c) {

		// DEBUG TIMER
		if(c.gameObject.tag == "Player" && DEBUG_TIMER > 0) {
			isUsed = true;
		}

		//if the player triggers the totem
		if(c.gameObject.tag == "Player" && !isUsed && DEBUG_TIMER <= 0) {

			// activate first unused group
			groups = GameObject.Find("sceneController").GetComponent<SceneController>().GetGroups();
			for(int i = 0; i < groups.Length; i++) {
				if(!groups[i].activeSelf) {
					
					// set up deactivation of this totem
					isUsed = true;

					// find the old and new groups
					newGroup = groups[i];
					System.Array.Sort(groups, SortByDistance);
					oldGroup = groups[0];
					
					// activate the new group
					newGroup.SetActive(true);

					// find all the children in the old and new groups
					oldChildren = oldGroup.GetComponentsInChildren<Transform>();
					newChildren = newGroup.GetComponentsInChildren<Transform>();

					// match the attributes of children from old to new
					for(int j = 0; j < oldChildren.Length; j++) {

						/*
						 * !NOTE
						 * to fix issue with enemy jumping
						 * all enemies must have agents disabled when player's state is copied
						 * maybe disable enemy agents in Awake()?
						 */

						// get this child's old and new instances
						GameObject o = oldChildren[j].gameObject;
						GameObject n = newChildren[j].gameObject;
						
						// totems
						/*
						 * !NOTE
						 * something seriously janky happens here if we are copying from the original sreen
						 * why does it happen?
						 */
						/*
						if(o.name == "totemCollider") {

							// get controllers
							TotemController			nController = n.GetComponent<TotemController>(),
													oController = o.GetComponent<TotemController>();

							// copy state from old to new
							nController.isUsed = oController.isUsed;
						}
						*/
						
						// target
						if(o.layer == 9) {
							
							// copy state from old to new
							n.transform.localPosition = o.transform.localPosition;
						}
						
						// player						
						if(o.layer == 8) {
							
							// get controllers
							AgentController			nController = n.GetComponent<AgentController>(),
													oController = o.GetComponent<AgentController>();
							
							// copy state from old to new
							n.transform.localPosition = o.transform.localPosition;
							nController.health = oController.health;
							nController.GetComponent<NavMeshAgent>().enabled = true;
							nController.target.transform.localPosition = oController.target.transform.localPosition;
						}
						
						// enemies
						if(o.layer == 10) {
							
							// get controllers
							EnemyAgentController	nController = n.GetComponent<EnemyAgentController>(),
													oController = o.GetComponent<EnemyAgentController>();
							
							// copy state from old to new
							n.transform.localPosition = o.transform.localPosition;
							nController.health = oController.health;
							nController.GetComponent<NavMeshAgent>().enabled = true;
						}
					}

					// break
					i = 100;
				}
			}
		}
	}

	// sort two objects by distance from the totem
	int SortByDistance(GameObject a, GameObject b) {
		float	aMagnitude = (transform.position - a.transform.position).sqrMagnitude,
				bMagnitude = (transform.position - b.transform.position).sqrMagnitude;
		return	aMagnitude.CompareTo(bMagnitude);
	}
}
