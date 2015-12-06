using UnityEngine;
using System.Collections;

public class MoveToScene : MonoBehaviour {

	void Update () {
		if(Application.loadedLevelName == "main") {
			if(Input.GetKeyDown(KeyCode.R)) {
				Application.LoadLevel("title");
			}
		} else if(Input.anyKeyDown) {
			Application.LoadLevel("main");
		}
	}
}
