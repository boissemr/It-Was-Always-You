using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TotemController : MonoBehaviour {

	// public parameters
	public bool		isUsed;

	// private variables
	SoundManager	sound;
	GameObject		oldGroup,
					newGroup,
					o,
					n;
	Transform[]		oldChildren,
					newChildren;
	GameObject[]	groups;
	float			DEBUG_TIMER;
	ClickToMove[]	oldTargets,
					newTargets;
	AgentController[]	oldPlayers,
						newPlayers;
	EnemyAgentController[]	oldEnemies,
							newEnemies;
	TotemController[]	oldTotems,
						newTotems;

	// start
	void Start() {
		isUsed = false;
		DEBUG_TIMER = 10;
		
		// sound manager
		sound = GameObject.Find("soundManager").GetComponent<SoundManager>();
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

			// sound
			sound.playSound(sound.totemCopySFX);

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

					// identify old and new arrays
					oldEnemies = oldGroup.GetComponentsInChildren<EnemyAgentController>();
					newEnemies = newGroup.GetComponentsInChildren<EnemyAgentController>();
					oldTargets = oldGroup.GetComponentsInChildren<ClickToMove>();
					newTargets = newGroup.GetComponentsInChildren<ClickToMove>();
					oldPlayers = oldGroup.GetComponentsInChildren<AgentController>();
					newPlayers = newGroup.GetComponentsInChildren<AgentController>();
					oldTotems = oldGroup.GetComponentsInChildren<TotemController>();
					newTotems = newGroup.GetComponentsInChildren<TotemController>();

					// totems
					for(int j = 0; j < oldTotems.Length; j++) {
						
						newTotems[j].isUsed = oldTotems[j].isUsed;
					}

					// enemies 1
					for(int j = 0; j < oldEnemies.Length; j++) {

						n = newEnemies[j].gameObject;
						o = oldEnemies[j].gameObject;
						
						newEnemies[j].stopAgent();
						n.transform.localPosition = o.transform.localPosition;
					}

					// target
					for(int j = 0; j < oldTargets.Length; j++) {

						n = newTargets[j].gameObject;
						o = oldTargets[j].gameObject;

						n.transform.localPosition = o.transform.localPosition;
					}

					// player
					for(int j = 0; j < oldPlayers.Length; j++) {

						n = newPlayers[j].gameObject;
						o = oldPlayers[j].gameObject;

						n.transform.localPosition = o.transform.localPosition;
						newPlayers[j].health = oldPlayers[j].health;
						newPlayers[j].money = oldPlayers[j].money;
						newPlayers[j].GetComponent<NavMeshAgent>().enabled = true;
						newPlayers[j].target.transform.localPosition = oldPlayers[j].target.transform.localPosition;

						// item management
						newPlayers[j].items.Clear();
						foreach(Item item in oldPlayers[j].items) newPlayers[j].items.Add(item);
					}

					// enemies 2
					for(int j = 0; j < oldEnemies.Length; j++) {

						newEnemies[j].health = oldEnemies[j].health;
						newEnemies[j].startAgent();
					}
					
					// refresh items
					foreach(GameObject o in GameObject.FindGameObjectsWithTag("UIitem")) {
						Destroy(o);
					}
					foreach(GameObject o in GameObject.FindGameObjectsWithTag("UIcamera")) {
						o.GetComponent<UIController>().updateItems();
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
