using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class Underway : MonoBehaviour {

	private MuseumManager mm;
	private MuseumKids m;

	public GameObject postHistoryContainer;
	public GameObject chatHistoryContainer;

	public GameObject post1;
	public GameObject post2;
	public GameObject post3;

	public GameObject logoutPanel;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		m = GameObject.Find("MuseumkidsHolder").GetComponent<MuseumKids>();

		// Added custom event to test if code was getting stripped out
		Analytics.CustomEvent("Start", new Dictionary<string, object> { {"Starting", 0 } });
	}
	
	// Update is called once per frame
	void Update () {
		try {
			GameObject.Find ("GoalText").GetComponentInChildren<Text>().text = mm.goal.GetGoalText();
			GameObject.Find ("GoalTextExtra").GetComponentInChildren<Text>().text = mm.goal.GetOverlayText();
		} catch {
		}
	}

	public void ShowJournalistChats() {
		mm.reporterChatHistory.SetActive(true);
	}

	public void ShowOfficerChats() {
		mm.officerChatHistory.SetActive(true);
	}

	public void ShowArtistChats() {
		mm.artistChatHistory.SetActive(true);
	}

	public void ShowGoal() {
		GameObject goal = (GameObject)Instantiate(Resources.Load ("Prefabs/GoalOverlay"));
		goal.name = "GoalOverlay";

		goal.transform.SetParent(GameObject.Find ("Canvas").transform, false);

		if ("".Equals(mm.goal.locationSprite)) {
			GameObject.Find ("GoalImage").GetComponentInChildren<Image>().sprite = null;
			GameObject.Find ("GoalImage").GetComponentInChildren<Image>().color = Color.clear;
		} else {
			GameObject.Find ("GoalImage").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Locaties/" + mm.goal.locationSprite);
//			GameObject.Find ("GoalImage").GetComponentInChildren<Image>().color = Color.white;
		}

		if (mm.story4Done) {
			GameObject.Find ("ProgressTimeline").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/timelinedot4");
		} else if (mm.story3Done) {
			GameObject.Find ("ProgressTimeline").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/timelinedot3");
		} else if (mm.story2Done) {
			GameObject.Find ("ProgressTimeline").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/timelinedot2");
		} else if (mm.story1Done) {
			GameObject.Find ("ProgressTimeline").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/timelinedot1");
		}
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
			
			this.post1.transform.Find("PostText").GetComponent<Text>().text = mm.story1Text;
			this.post1.transform.Find("PostImage").GetComponent<Image>().sprite = Sprite.Create (mm.story1Image, new Rect(0, 0, mm.story1Image.width, mm.story1Image.height), new Vector2(0.5f, 0.5f));

			if (m.story1Shared) {
				GameObject.Find ("Share1Button").GetComponent<Button>().interactable = false;
			} else {
				GameObject.Find ("Share1Button").GetComponent<Button>().interactable = true;
			}
		}

		if (mm.story2Done) {
			this.post2.SetActive(true);

			this.post2.transform.Find("PostText").GetComponent<Text>().text = mm.story2Text;
			this.post2.transform.Find("PostImage").GetComponent<Image>().sprite = Sprite.Create (mm.story2Image, new Rect(0, 0, mm.story2Image.width, mm.story2Image.height), new Vector2(0.5f, 0.5f));
		
			if (m.story2Shared) {
				GameObject.Find ("Share2Button").GetComponent<Button>().interactable = false;
			} else {
				GameObject.Find ("Share2Button").GetComponent<Button>().interactable = true;
			}
		}

		if (mm.story3Done) {
			this.post3.SetActive(true);

			this.post3.transform.Find ("PostText").GetComponent<Text>().text = mm.story3Text;
			this.post3.transform.Find("PostImage").GetComponent<Image>().sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, mm.story3Image.width, mm.story3Image.height), new Vector2(0.5f, 0.5f));

			if (m.story3Shared) {
				GameObject.Find ("Share3Button").GetComponent<Button>().interactable = false;
			} else {
				GameObject.Find ("Share3Button").GetComponent<Button>().interactable = true;
			}
		}

		// Check whether we need to display the museumkids logged in thing
		MuseumKids.onMuseumkidsLoggedIn += ShowLogoutPanel;
		
		if (m.LoggedIn()) {
			ShowLogoutPanel();
		} else {
			logoutPanel.SetActive(false);
		}
	}

	public void ShowLogoutPanel() {
		this.logoutPanel.SetActive(true);

		GameObject.Find ("LoggedInText").GetComponent<Text>().text = "Ingelogd als: " + m.email;
	}

	public void LogoutButton() {
		MuseumKids.onMuseumkidsLoggedOut += LogoutCompleted;
		
		GameObject login = (GameObject)Instantiate(Resources.Load ("Prefabs/MuseumkidsLogoutOverlay"));
		login.name = "MuseumkidsLogoutOverlay";
		
		login.transform.SetParent(GameObject.Find ("Canvas").transform, false);
	}

	public void LogoutCompleted() {
		MuseumKids.onMuseumkidsLoggedOut -= LogoutCompleted;

		logoutPanel.SetActive(false);

		ShowPostHistory();
	}
}
