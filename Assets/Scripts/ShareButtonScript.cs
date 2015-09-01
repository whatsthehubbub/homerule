using UnityEngine;
using System.Collections;

public class ShareButtonScript : MonoBehaviour {

	private MuseumKids m;

	// Use this for initialization
	void Start () {
		// Get Mumeuskids object
		m = GameObject.Find ("Main").GetComponent<MuseumKids>();
	}

//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void ShowOverlay() {
		GameObject login = (GameObject)Instantiate(Resources.Load ("Prefabs/MuseumkidsOverlay"));
		login.name = "MuseumkidsOverlay";
		
		login.transform.SetParent(GameObject.Find ("Canvas").transform, false);
	}
	
	public void Share1ButtonPressed() {
		Debug.Log ("Share button pressed");

		// If not logged in, do the login dance

		m.storyToShare = 1;

		ShowOverlay();
	}

	public void Share2ButtonPressed() {
		Debug.Log ("Share button pressed");
		
		// If not logged in, do the login dance
		
		m.storyToShare = 2;
		
		ShowOverlay();
	}

	public void Share3ButtonPressed() {
		Debug.Log ("Share button pressed");
		
		// If not logged in, do the login dance
		
		m.storyToShare = 3;
		
		ShowOverlay();
	}
}
