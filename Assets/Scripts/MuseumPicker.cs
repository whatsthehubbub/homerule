using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MuseumPicker : MonoBehaviour {

	public GameObject titleScreen;
	public ToggleGroup toggleGroup;

	public MuseumManager mm;
	public MuseumChoice mc;

	public bool checkOnce;

	void Awake() {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		GameObject mch = GameObject.Find ("MuseumChoiceHolder");
		mc = mch.GetComponentInChildren<MuseumChoice>();
	}

	// Update is called once per frame
	void Update () {
		// We need to do this check once but only when the entire GameObject is running
		// This seems to be the most consistent way to achieve that
		if (checkOnce) {
			checkOnce = false;

			CheckWhetherToDisplayOurselves();
		}
	}

	void OnEnable() {
		// Set the variable that says we need to do the check once
		checkOnce = true;
	}

	public void CheckWhetherToDisplayOurselves() {
		if (mc.pickMuseum) {
			// Show this screen to allow the player to pick a museum
			
			toggleGroup.SetAllTogglesOff();
			
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
		// Disable this control since the player will have to set a museum now
		mc.pickMuseum = false;

		Toggle active = toggleGroup.ActiveToggles().FirstOrDefault();

		if (active.gameObject.name.Equals("AirborneToggle")) {
			mc.museumPicked = "Airborne";
		} else if (active.gameObject.name.Equals("DummyToggle")) {
			mc.museumPicked = "Dummy";
		}
		GoToNextScreen();
	}

	public void GoToNextScreen() {
		mm.museum = mc.GetMuseum();

		mm.goal = mm.museum.GetStartGoal();

//		this.titleScreen.SetActive(true);
//		this.gameObject.SetActive(false);
	}
}
