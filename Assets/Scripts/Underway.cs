using UnityEngine;
using System.Collections;

public class Underway : MonoBehaviour {

	private MuseumManager mm;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowJournalistChats() {
		Debug.Log ("Show journalist chats");

		mm.reporterChatHistory.SetActive(true);
	}

	public void ShowOfficerChats() {
		mm.officerChatHistory.SetActive(true);
	}
}
