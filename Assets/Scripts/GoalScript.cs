using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

	private MuseumManager mm;

	public GameObject stopOverlay;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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