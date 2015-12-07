using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject player;

	Text[] texts;

	void Start() {
		texts = GetComponentsInChildren<Text>();
	}

	void Update() {
		foreach(Text o in texts) {
			switch(o.gameObject.name) {
				case "health":
					o.text = player.GetComponent<AgentController>().health.ToString();
					break;
				case "money":
					o.text = player.GetComponent<AgentController>().money.ToString();
					break;
			}
		}
	}
}
