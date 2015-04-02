using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (/* Input.touchCount == 1 || */ Input.GetMouseButtonDown(0)) {
			// Touch or mouse click
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2(wp.x, wp.y);

			Debug.Log (touchPos);

			if (this.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
				Debug.Log("Camera button pressed");	
			} else {
				Debug.Log ("Somewhere else pressed");
			}
		}
	}
}
