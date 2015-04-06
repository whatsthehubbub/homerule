using UnityEngine;
using System.Collections;

public class EventStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("in event start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonPressed() {
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("ShowEventSituation");
		}
	}
}
