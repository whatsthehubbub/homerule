using UnityEngine;
using System.Collections;

public class EventCallAction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonPressed() {
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("ShowArticleStart");
		}
	}
}
