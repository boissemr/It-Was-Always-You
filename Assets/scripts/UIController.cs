using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

	public GameObject	player,
						UIitem,
						UI;

	Text[] texts;
	List<Item> items;

	void Start() {

		texts = GetComponentsInChildren<Text>();
		UI = GetComponentInChildren<LocatorDummy>().gameObject;

		updateItems();
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
	}

	public void updateItems() {

		// add items
		items = player.GetComponent<AgentController>().items;
		for(int i = 0; i < items.Count; i++) {
			GameObject o = GameObject.Instantiate(UIitem);
			o.transform.SetParent(UI.transform);
			o.transform.localScale = Vector3.one * .1f;
			o.transform.localPosition = new Vector3(67f, -125f - (20f * i), 0f);
			o.transform.localPosition += new Vector3(-50f, 50f, 0f); // why do we need this line? no idea why it's offset by exactly 50 units like this...
			o.GetComponent<Image>().sprite = items[i].image;
			o.GetComponentInChildren<Text>().text = items[i].description;
			o.GetComponentInChildren<Text>().gameObject.SetActive(false);
		}
	}
}
