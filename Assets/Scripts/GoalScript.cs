using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalScript : MonoBehaviour {

	private MuseumManager mm;

	public GameObject stopOverlay;
	public GameObject endGameMessage;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();


		if (mm.story4Done) {
			// This is the end of the game and we need to do some special things here
			GameObject.Find ("GoalImage").SetActive(false);

			this.endGameMessage.SetActive(true);
			this.endGameMessage.GetComponentInChildren<Text>().text = mm.museum.endGameText;
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