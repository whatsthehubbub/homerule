﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Underway : MonoBehaviour {

	private MuseumManager mm;

	public GameObject postHistoryContainer;
	public GameObject chatHistoryContainer;

	public GameObject post1;
	public GameObject post2;
	public GameObject post3;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowJournalistChats() {
		mm.reporterChatHistory.SetActive(true);
	}

	public void ShowOfficerChats() {
		mm.officerChatHistory.SetActive(true);
	}

	public void ShowGoal() {
		GameObject goal = (GameObject)Instantiate(Resources.Load ("Prefabs/Goal"));
		goal.name = "Goal";
	}

	public void ShowChatHistory() {
		GameObject.Find ("ChatsTabButton").GetComponent<Button>().interactable = false;
		GameObject.Find ("PostsTabButton").GetComponent<Button>().interactable = true;

		postHistoryContainer.SetActive(false);
		chatHistoryContainer.SetActive(true);
	}

	public void ShowPostHistory() {
		GameObject.Find ("ChatsTabButton").GetComponent<Button>().interactable = true;
		GameObject.Find ("PostsTabButton").GetComponent<Button>().interactable = false;

		postHistoryContainer.SetActive(true);
		chatHistoryContainer.SetActive(false);

		// Fill in the various posts
		if (mm.story1Done) {
			this.post1.SetActive(true);

			this.post1.transform.Find("PostContainer/PostText").GetComponent<Text>().text = mm.story1Text;
		}

		if (mm.story2Done) {
			this.post2.SetActive(true);

			this.post2.transform.Find("PostContainer/PostText").GetComponent<Text>().text = mm.story2Text;
		}
	}

	public void DebugButton() {
		mm.TakeImmediateCall(2);
	}
}
