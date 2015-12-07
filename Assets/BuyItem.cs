using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour {

	public Item	item;

	Image	displaySprite;
	Text	displayText;
	bool	soldOut;

	void Start() {

		// select random item
		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		item = items[(int)Random.Range(0, items.Length)].GetComponent<Item>();

		soldOut = false;

		displaySprite = GetComponentInChildren<Image>();
		displayText = GetComponentInChildren<Text>();

		displaySprite.sprite = item.image;
		displayText.text = item.price.ToString();
	}

	void OnTriggerEnter(Collider c) {

		if(!soldOut && c.gameObject.tag == "Player") {

			AgentController player = c.gameObject.GetComponent<AgentController>();

			if(player.money >= item.price) {

				// subtract money and add item
				player.money -= item.price;
				player.items.Add(item);

				// apply effects
				if(item.setRatKing) {
					player.ratKing = true;
				}
				player.health += item.healthIncrease;
				player.reach += item.rangeIncrease;
				if(item.fireRateIncrease != 0) player.fireRate /= item.fireRateIncrease;
				player.agent.speed += item.speedIncrease;
				player.moneyMultiplier += item.moneyMultiplierIncrease;

				// refresh items
				foreach(GameObject o in GameObject.FindGameObjectsWithTag("UIitem")) {
					Destroy(o);
				}
				foreach(GameObject o in GameObject.FindGameObjectsWithTag("UIcamera")) {
					o.GetComponent<UIController>().updateItems();
				}

				// remove item from shop
				soldOut = true;
				displayText.text = "SOLD OUT";
				displaySprite.sprite = null;
			}
		}
	}
}
