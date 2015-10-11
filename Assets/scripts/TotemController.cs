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

	// start
	void Start() {
		isUsed = false;
	}

	// update
	void Update() {
		// make this totem invisible
		if(isUsed) {
			MeshRenderer[] renderers = this.transform.parent.gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer o in renderers) o.enabled = false;
		}
	}

	// trigger enter
	void OnTriggerEnter(Collider c) {
		//if the player triggers the totem
		if (c.gameObject.tag == "Player" && !isUsed) {
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
						
						// enemies
						if(oldChildren[j].gameObject.layer == 10) {
							Transform				n = newChildren[j],
													o = oldChildren[j];
							EnemyAgentController	nController = n.gameObject.GetComponent<EnemyAgentController>(),
													oController = o.gameObject.GetComponent<EnemyAgentController>();
							n.localPosition = o.localPosition;
							nController.health = oController.health;
						}
						
						// target
						if(oldChildren[j].gameObject.layer == 9) {
							Transform				n = newChildren[j],
													o = oldChildren[j];
							n.localPosition = o.localPosition;
						}
						
						// player						
						if(oldChildren[j].gameObject.layer == 8) {
							Transform				n = newChildren[j],
													o = oldChildren[j];
							AgentController			nController = n.gameObject.GetComponent<AgentController>(),
													oController = o.gameObject.GetComponent<AgentController>();
							n.localPosition = o.localPosition;
							nController.health = oController.health;
							nController.GetComponent<NavMeshAgent>().enabled = true;
						}
						
						// totems
						if(oldChildren[j].gameObject.name == "totemCollider") {
							Transform				n = newChildren[j],
													o = oldChildren[j];
							TotemController			nController = n.gameObject.GetComponent<TotemController>(),
													oController = o.gameObject.GetComponent<TotemController>();
							nController.isUsed = oController.isUsed;
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
