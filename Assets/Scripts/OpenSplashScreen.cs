using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenSplashScreen : MonoBehaviour {

	public MuseumManager mm;

	public GameObject museumPicker;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		UpdateChangeMuseumButton();
	}

	void OnEnable() {
		UpdateChangeMuseumButton();
	}

	public void UpdateChangeMuseumButton() {
		GameObject button = GameObject.Find ("ChangeMuseumButton");

		if (button != null && mm != null) {
			button.GetComponentInChildren<Text>().text = "je speelt nu in <b>" + mm.museum.museumName + "</b>";
		}
	}

	public void ChangeMuseumButtonPressed() {
		GameObject.Find("MuseumChoiceHolder").GetComponentInChildren<MuseumChoice>().pickMuseum = true;

		this.gameObject.SetActive(false);
		this.museumPicker.SetActive(true);
	}

	public void ColofonButtonPressed() {
		GameObject colofon = (GameObject)Instantiate(Resources.Load ("Prefabs/ColofonOverlay"));
		colofon.transform.SetParent(GameObject.Find ("Start UI").transform, false);
		colofon.name = "ColofonOverlay";
	}
}
