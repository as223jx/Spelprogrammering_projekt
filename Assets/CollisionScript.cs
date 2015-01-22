using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CollisionScript : MonoBehaviour {
	public static int health = 3;
	static GameObject[] player;
	GameObject newSpriteObj;
	GameObject redSpriteObj;
	GameObject blueSpriteObj;
	GameObject goal;
	GameObject particle;
	GameObject ball;
	GameObject camera;
	GameObject playerS;
	GameObject pause;
	Sprite invulSprite;
	Sprite redSprite;
	Sprite blueSprite;
	Sprite newSprite;
	Sprite ballSprite;
	Sprite regularBallSprite;
	Vector3 newPos;
	AudioClip sound;
	private static bool invul;
	private bool pausedGame = false;
	MonoBehaviour[] ballScripts;
	MonoBehaviour[] cameraScripts;
	Text pauseText;

	// Initialization
	void Start () {
		pause = GameObject.Find ("pauseone");
		pause.renderer.enabled = false;
		camera = GameObject.Find ("Main Camera");
		playerS = GameObject.Find ("ball2");
		goal = GameObject.Find ("goal");
		invul = false;
		particle = GameObject.Find ("spark");
		player = GameObject.FindGameObjectsWithTag ("Player");
		ball = player [0];
		ballSprite = player [0].GetComponent<SpriteRenderer> ().sprite;
		newSpriteObj = GameObject.FindGameObjectWithTag ("invul");
		newSprite = newSpriteObj.GetComponent<SpriteRenderer> ().sprite;
		redSpriteObj = GameObject.FindGameObjectWithTag ("ballred");
		redSprite = redSpriteObj.GetComponent<SpriteRenderer> ().sprite;
		blueSpriteObj = GameObject.FindGameObjectWithTag ("ballblue");
		blueSprite = blueSpriteObj.GetComponent<SpriteRenderer> ().sprite;
		ballScripts = playerS.GetComponents<MonoBehaviour> ();
		cameraScripts = camera.GetComponents<MonoBehaviour> ();
	}


	// Update is called once per frame
	void Update () {
		if (camera == null) {
			camera = GameObject.FindGameObjectWithTag("MainCamera");		
		}
		if(pause != null){
			pause = GameObject.Find ("pauseone");
			Vector3 textPos = new Vector3 (camera.transform.position.x, camera.transform.position.y, 0);
			pause.transform.position = textPos;
		}
		int i = 0;

		if (Input.GetKeyDown ("m") && pausedGame) {
			Application.LoadLevel("menu");
		}

		if (Input.GetKeyDown ("p") && pausedGame) {
			pause.renderer.enabled = false;
			pausedGame = false;
			foreach(MonoBehaviour script in ballScripts){
				i++;
				Debug.Log ("Ball 1: " + i);
				script.enabled = true;
			}

			foreach(MonoBehaviour script in cameraScripts){
				i++;
				Debug.Log ("Cam 1: " + i);
				Debug.Log (script);
				script.enabled = true;
				
			}
			Time.timeScale = 1;
		}
		else if(Input.GetKeyDown ("p")){
			pause.renderer.enabled = true;
			pausedGame = true;
			foreach(MonoBehaviour script in ballScripts){
				i++;
				Debug.Log ("Ball 2: " + i);
				script.enabled = false;
			}


			foreach(MonoBehaviour script in cameraScripts){
				i++;
				Debug.Log ("Cam 2: " + i);
				Debug.Log (script);
				script.enabled = false;

			}
			Time.timeScale = 0;
		}

		player = GameObject.FindGameObjectsWithTag ("Player");
		goal = GameObject.Find ("goal");
		// Makes sure player doesn't leave camera on z-axis
		if (player [0].transform.position.y > 3.36f) {
			newPos = new Vector3(player[0].transform.position.x, 3.35f);
			player[0].transform.position = newPos;
		}
		if (player [0].transform.position.y < -0.63f) {
			newPos = new Vector3(player[0].transform.position.x, -0.62f);
			player[0].transform.position = newPos;
		}

		// Checks if player gets to goal
		if (Application.loadedLevelName == "level1" && player[0].transform.position.x > goal.transform.position.x - 0.8) {
			Application.LoadLevel("level2pause");
		}
		if (Application.loadedLevelName == "level2" && player[0].transform.position.x > goal.transform.position.x - 0.8) {
			Application.LoadLevel("lvl3pause");
		}
		if (Application.loadedLevelName == "level3" && player[0].transform.position.x > 90.31) {
			Application.LoadLevel("menu");
		}

		// Sets player sprite depending on status; regular/invulnerable/attacking
		if (Input.GetKey (KeyCode.RightArrow) && !invul) {
			player[0].GetComponent<SpriteRenderer> ().sprite = redSprite;
		}
		else if (Input.GetKey (KeyCode.LeftArrow) && !invul) {
			player[0].GetComponent<SpriteRenderer> ().sprite = blueSprite;
		}
		else if (invul) {
			player[0].GetComponent<SpriteRenderer> ().sprite = newSprite;
		}
		else {
			player[0].GetComponent<SpriteRenderer> ().sprite = ballSprite;
		}
	}

	// Called when player collides with an object
	void OnCollisionEnter2D(Collision2D coll)
	{
		player = GameObject.FindGameObjectsWithTag ("Player");
		string contact = coll.transform.tag;
		Debug.Log ("Contact: " + contact);
		Debug.Log ("Tag: " + coll.gameObject.tag);
		Debug.Log ("Name: " + coll.gameObject.name);

		// Checks if colliding with an enemy while in attack mode
		if (coll.gameObject.name == "kryss" && Input.GetKey (KeyCode.RightArrow)) {
			Vector3 position = coll.gameObject.transform.position;
			Quaternion rotato = coll.gameObject.transform.rotation;
			Debug.Log ("Fienden död");
			Instantiate (particle, position, rotato);
			audio.Play ();
			Destroy (coll.gameObject);

		}
		// Checks if colliding with a blue wall
		else if (coll.gameObject.name == "bluewall" && Input.GetKey (KeyCode.LeftArrow)) {
			GameObject bluewall = GameObject.FindGameObjectWithTag("bluewall");
			bluewall.collider2D.enabled = false;
			Physics2D.IgnoreCollision(coll.gameObject.collider2D, ball.collider2D, true);
			Debug.Log ("Bluewall");
		}

		// Checks if colliding with a box or enemy
		else if (coll.gameObject.name == "Box" || coll.gameObject.name == "kryss" || coll.gameObject.name == "bluewall") {
			Debug.Log ("Name: " + coll.gameObject.name);
			Debug.Log("fiende?");
			health --;
			if (health <= 0) {
					health = 3;
					Application.LoadLevel (Application.loadedLevelName);
			}

			//Changing sprite and making player invulerable for a short while
			if (player != null) {
					player [0].collider2D.enabled = false;
					player [0].GetComponent<SpriteRenderer> ().sprite = newSprite;

					StartCoroutine (Invulnerable ());

			}
		} 
	}

	// Makes sure player can't collide with anything for 1.5 seconds
	IEnumerator Invulnerable(){
		invul = true;
		yield return new WaitForSeconds(1.5f);
		player[0].collider2D.enabled = true;
		player[0].GetComponent<SpriteRenderer>().sprite = ballSprite;
		invul = false;
	}
}
