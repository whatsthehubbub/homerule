using UnityEngine;
using System.Collections;

public class RocketManager : MonoBehaviour {

	public float speed = 3.0f;
	public GameObject target = null;

	public GameObject ball;
	public GameObject origin;

	// Use this for initialization
	void Start () {
		Debug.Log ("Target at start" + target);

		ball = GameObject.Find("BallSprite");
		origin = GameObject.Find ("Origin");

//		target = GameObject.Find ("BallSprite");
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
		}
	}

	public void Move() {
		if (this.target == null || this.target == origin) {
			target = ball;
		} else if (this.target == ball) {
			target = origin;
		}
	}

	public void NewBeacon(Location location) {
		if (location.name == "OFFICE") {
			target = ball;
		} else if (location.name == "HOME") {
			target = origin;
		}
	}
}
