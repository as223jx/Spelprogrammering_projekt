using UnityEngine;
using System.Collections;

public class PressSpaceScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName == "level1pause" && Input.GetKey (KeyCode.Space)) {
			CollisionScript.health = 3;
			Application.LoadLevel ("level1");
		}
		if (Application.loadedLevelName == "level2pause" && Input.GetKey (KeyCode.Space)) {
			CollisionScript.health = 3;
			Application.LoadLevel ("level2");
		}
		if (Application.loadedLevelName == "lvl3pause" && Input.GetKey (KeyCode.Space)) {
			CollisionScript.health = 3;
			Application.LoadLevel ("level3");
		}
	}
}
