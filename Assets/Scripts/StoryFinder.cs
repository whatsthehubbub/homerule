using UnityEngine;
using System.Collections;

public class StoryFinder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			// Touch or mouse click
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			
			Debug.Log (touchPos);
			
			if (this.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
				// Hard increment the value on the MuseumManager

//				GameObject.Find("Main").GetComponent<MuseumManager>().observationsFound += 1;

				// TODO turn this into an event call
			}
		}
	}
}
