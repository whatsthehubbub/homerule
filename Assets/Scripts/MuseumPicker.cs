using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MuseumPicker : MonoBehaviour {

	public GameObject titleScreen;
	public ToggleGroup toggleGroup;

	public MuseumManager mm;
	public MuseumChoice mc;

	void Awake() {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		GameObject mch = GameObject.Find ("MuseumChoiceHolder");
		mc = mch.GetComponentInChildren<MuseumChoice>();
	}

	// Use this for initialization
	void Start () {
		CheckWhetherToDisplayOurselves();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable() {
		CheckWhetherToDisplayOurselves();
	}

	public void CheckWhetherToDisplayOurselves() {
		if (mc.pickMuseum) {
			// Show this screen to allow the player to pick a museum

			// Disable this control since the player will have to set a museum now
			mc.pickMuseum = false;

			// Deselect all toggles
			foreach (var toggle in toggleGroup.ActiveToggles()) {
				toggle.isOn = false;
			}

			// Select the toggle that the user had previously picked
			try {
				GameObject.Find (mc.museumPicked + "Toggle").GetComponentInChildren<Toggle>().isOn = true;
			} catch {
			}
		} else {
			GoToNextScreen();
		}
	}

	public void MuseumPicked() {
		Toggle active = toggleGroup.ActiveToggles().FirstOrDefault();

		Museum m;
		if (active.gameObject.name.Equals("AirborneToggle")) {
			m = new AirborneMuseum();

			mc.museumPicked = m.museumCode;
		} else if (active.gameObject.name.Equals("DummyToggle")) {
			m = new DummyMuseum();
			mc.museumPicked = m.museumCode;
		} else {
			m = null;
		}
		mm.museum = m;

		GoToNextScreen();
	}

	public void GoToNextScreen() {
		this.titleScreen.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
