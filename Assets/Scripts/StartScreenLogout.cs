using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreenLogout : MonoBehaviour {

	public GameObject logoutPanel;

	private MuseumKids m;

	// Use this for initialization
	void Start () {
		m = GameObject.Find("MuseumkidsHolder").GetComponent<MuseumKids>();

		if (m.LoggedIn()) {
			logoutPanel.SetActive(true);
		} else {
			logoutPanel.SetActive(false);
		}

		GameObject.Find ("LoggedInText").GetComponent<Text>().text = "Ingelogd als: " + m.email;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LogoutButton() {
		m.Logout();

		GameObject.Find ("LoggedInText").GetComponent<Text>().text = "Uitgelogd!";

		GameObject.Find ("LogoutButton").GetComponent<Button>().interactable = false;
	}
}
