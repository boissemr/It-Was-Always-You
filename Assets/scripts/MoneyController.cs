using UnityEngine;
using System.Collections;

public class MoneyController : MonoBehaviour {
	
	public GameObject	player;
	public GameObject[] players;

	SoundManager sound;
	
	void Start() {

		players = GameObject.FindGameObjectsWithTag("Player");
		sound = GameObject.Find("soundManager").GetComponent<SoundManager>();
	}

	void Update() {

		if(player == null) {

			// find a player
			foreach(GameObject o in players) {
				if(Vector3.Distance(transform.position, o.transform.position) < 100) {
					player = o;
				}
			}
		} else {

			// move towards player
			if(Vector3.Distance(transform.position, player.transform.position) < 4) {
				transform.position = Vector3.Lerp(transform.position, player.transform.position, .1f);

				// collect
				if(Vector3.Distance(transform.position, player.transform.position) < 1.5) {
					player.GetComponent<AgentController>().money += 1;
					DestroyObject(gameObject);
					sound.playSound(sound.moneySFX);
				}
			}
		}
	}
}
