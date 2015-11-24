using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalScript : MonoBehaviour {

	private MuseumManager mm;

	public GameObject stopOverlay;
	public GameObject endGameMessage;
	public GameObject endGameMuseumkidsText;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		if (mm.story4Done) {
			// This is the end of the game and we need to do some special things here

			// Disable the goal image object to make space
			GameObject goalImage = GameObject.Find ("GoalImage");
			if (goalImage != null) {
				goalImage.SetActive(false);
			}

			// Enable the end game message and set it to a specific text
			if (this.endGameMessage != null) {
				this.endGameMessage.SetActive(true);
				this.endGameMessage.GetComponentInChildren<Text>().text = mm.museum.endGameText;
			}

			// Turn off area text
			GameObject areaText = GameObject.Find ("AreaText");
			if (areaText != null) {
				areaText.SetActive(false);
			}

			// Enable the museum kids notice
			if (this.endGameMuseumkidsText != null) {
				this.endGameMuseumkidsText.SetActive(true);
			}
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void ShowStopOverlay() {
		mm.callBusy = true;

		this.stopOverlay.SetActive(true);
	}

	public void HideStopOverlay() {
		mm.callBusy = false;

		this.stopOverlay.SetActive(false);
	}

	public void CloseGoal() {
		GameObject goal = GameObject.Find("GoalOverlay");
		
		Destroy (goal);
	}
}