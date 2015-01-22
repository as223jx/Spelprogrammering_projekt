using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private GameObject[] player;
	int startHealth;
	int currentHealth;
	int lastHealth;
	Vector3 newPos;
	Vector3[] testPos;
	GameObject healthOne;
	GameObject healthTwo;
	GameObject healthThree;
	GameObject[] test;
	
	// Use this for initialization
	void Start () {

		startHealth = CollisionScript.health;
		lastHealth = startHealth;
		currentHealth = CollisionScript.health;

		testPos = new Vector3[currentHealth];
		test = new GameObject[currentHealth];

		// Creating health icons for each health
		for (int i = 0; i<currentHealth; i++) {
			GameObject health = GameObject.CreatePrimitive(PrimitiveType.Cube);
			health.renderer.material.color = Color.red;
			health.transform.position = new Vector3(-19f + i, 3.5f, 0f);
			health.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
			test[i] = health;
			testPos[i] = health.transform.position;
		}
		player = GameObject.FindGameObjectsWithTag ("Player");
		newPos = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Checking current health
		lastHealth = currentHealth;
		currentHealth = CollisionScript.health;

		//Debug.Log ("Last: " + lastHealth);
		if (currentHealth < lastHealth) {
			Destroy(test[currentHealth]);
		}

		for (int i = 0; i < currentHealth; i++) {
				testPos [i].x += 0.06f;
			if(test[i] != null){
				test [i].transform.position = testPos [i];
			}
		}

		newPos.x += 0.06f;
		Camera.main.transform.position = newPos;
	}
}
