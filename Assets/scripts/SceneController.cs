using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	
	GameObject[] groups;
	
	void Start() {
		// doesn't work because they are for some reason out of order:
		//groups = GameObject.FindGameObjectsWithTag("Group");
		// so we'll do it the stupid way for now, I guess
		groups = new GameObject[4];
		groups[0] = GameObject.Find("GROUP_1");
		groups[1] = GameObject.Find("GROUP_2");
		groups[2] = GameObject.Find("GROUP_3");
		groups[3] = GameObject.Find("GROUP_4");
		for(int i = 1; i < groups.Length; i++) {
			groups[i].SetActive(false);
		}
	}

	public GameObject[] GetGroups() {
		return groups;
	}
}
