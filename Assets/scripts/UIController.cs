using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject player;

	Text t;

	void Start() {
		t = GetComponentInChildren<Text>();
	}

	void Update() {
		if(player.activeSelf) {
			t.text = player.GetComponent<AgentController>().health.ToString();
		} else {
			t.text = "0";
		}
	}
}
