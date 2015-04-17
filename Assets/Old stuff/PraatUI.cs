using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PraatUI : MonoBehaviour {

	public GameObject label;
	public Text text;

	public void TalkClicked() {
		label.SetActive(true);

		GameObject main = GameObject.Find("Main");

		if (main != null) {
			MuseumManager mm = main.GetComponent<MuseumManager>();

//			if (mm.observationLocations.Count > 0) {
//				text.text = "Ik zou eens gaan kijken bij " + mm.getLocationInterfaceString(mm.observationLocations[0]) + ".";
//			} else {
//				text.text = "Ik heb geen informatie.";
//			}
		}
	}
}
