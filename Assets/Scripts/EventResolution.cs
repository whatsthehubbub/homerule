using UnityEngine;
using System.Collections;

public class EventResolution : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoneButton() {
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("ShowIdle");
		}
	}
}
