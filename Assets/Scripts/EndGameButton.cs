using UnityEngine;
using System.Collections;

public class EndGameButton : MonoBehaviour {

	private MuseumManager mm;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void QuitPlayingButton() {
		mm.QuitGame();
	}
}
