using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject	player,
						UIitem,
						UI;

	Text[] texts;
	Item[] items;

	void Start() {

		texts = GetComponentsInChildren<Text>();
		items = player.GetComponent<AgentController>().items;
		UI = GetComponentInChildren<LocatorDummy>().gameObject;

		// items
		for(int i = 0; i < items.Length; i++) {
			GameObject o = GameObject.Instantiate(UIitem);
			o.transform.SetParent(UI.transform);
			o.transform.localScale = new Vector3(.15f, .15f, .15f);
			o.transform.localPosition = new Vector3(67f, -125f - (20f * i), 0f);
			// why do we need this line? no idea why it's offset by exactly 50 units like this...
			o.transform.localPosition += new Vector3(-50f, 50f, 0f);
			o.GetComponent<Image>().sprite = items[i].image;
			o.GetComponentInChildren<Text>().text = items[i].description;
			o.GetComponentInChildren<Text>().gameObject.SetActive(false);
		}
	}

	void Update() {

		// health and money
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

		// items
		/*foreach(Item o in player.GetComponent<AgentController>().items) {
			Debug.Log(o.gameObject.name + "|" + o.image.name + "|" + o.description);
		}*/
	}
}
