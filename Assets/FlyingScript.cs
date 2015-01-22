using UnityEngine;
using System.Collections;

public class FlyingScript : MonoBehaviour {
	public Vector2 position;
	private float fall;
	private float fly;
	// Use this for initialization
	void Start () {
		fall = 0.025f;
		fly = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {

		this.position = this.transform.position;
		this.position.x = this.position.x + 0.06f;
		//Debug.Log (this.position.x);
		if (Input.GetKey (KeyCode.Space)) {
			fall = 0.025f; 
			this.position.y = this.position.y+fly;
			this.transform.position = this.position;
			fly += 0.002f;
		}
		else {
			fly = 0.05f;
			this.position.y = this.position.y-fall;
			this.transform.position = this.position;
			fall += 0.001f;
		}
	}
}
