using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MuseumkidsLogoutOverlay : MonoBehaviour {

	private MuseumKids m;
	private MuseumManager mm;

	// Use this for initialization
	void Start () {
		m = GameObject.Find ("MuseumkidsHolder").GetComponent<MuseumKids>();

		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		mm.callBusy = true;

		GameObject.Find("LogoutExplanation").GetComponentInChildren<Text>().text = "Weet je zeker dat je " + m.email + " wilt uitloggen?";
	}

//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void CloseOverlay() {
		mm.callBusy = false;

		Destroy (this.gameObject);
	}

	public void LogoutButtonPressed() {
		m.Logout();

		this.CloseOverlay();
	}
}
