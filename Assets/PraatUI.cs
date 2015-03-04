using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PraatUI : MonoBehaviour {

	public GameObject label;
	public Text text;

	public void TalkClicked() {
		label.SetActive(true);

		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();

		if (mm != null) {
			if (mm.observationLocations.Count > 0) {
				text.text = "Ik zou eens gaan kijken in " + mm.observationLocations[0];
			} else {
				text.text = "Ik heb geen informatie.";
			}
		}
	}
}
