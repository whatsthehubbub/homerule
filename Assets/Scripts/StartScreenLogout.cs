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

			GameObject.Find ("LoggedInText").GetComponent<Text>().text = "Ingelogd als: " + m.email;
		} else {
			logoutPanel.SetActive(false);
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LogoutButton() {
		MuseumKids.onMuseumkidsLoggedOut += LogoutCompleted;

		GameObject login = (GameObject)Instantiate(Resources.Load ("Prefabs/MuseumkidsLogoutOverlay"));
		login.name = "MuseumkidsLogoutOverlay";
		
		login.transform.SetParent(GameObject.Find ("Start UI").transform, false);
	}

	public void LogoutCompleted() {
		MuseumKids.onMuseumkidsLoggedOut -= LogoutCompleted;

		GameObject.Find ("LogoutPanel").SetActive(false);
	}
}
