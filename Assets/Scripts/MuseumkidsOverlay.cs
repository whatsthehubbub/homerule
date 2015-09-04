﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MuseumkidsOverlay : MonoBehaviour {

	public GameObject login;
	public GameObject share;
	public GameObject loggedin;
	
	private MuseumKids m;
	private MuseumManager mm;

	// Use this for initialization
	void Start () {
		Debug.Log ("in overlay start");

		m = GameObject.Find ("MuseumkidsHolder").GetComponent<MuseumKids>();

		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		mm.callBusy = true;

		if (m.LoggedIn()) {
			ShowLoggedinPanel();
		} else {
			ShowLoginPanel();
		}
	}

//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void ShowLoginPanel() {
		this.login.SetActive(true);
		this.share.SetActive(false);
		this.loggedin.SetActive(false);
	}

	public void ShowLoggedinPanel() {
		this.login.SetActive(false);
		this.share.SetActive(false);
		this.loggedin.SetActive(true);

		GameObject.Find("LoggedInExplanation").GetComponentInChildren<Text>().text = "Je bent ingelogd als: " + m.email;
	}

	public void ShowSharePanel() {
		this.login.SetActive(false);
		this.loggedin.SetActive(false);
		this.share.SetActive(true);

		// Set the right text and image of the story we want to share
		GameObject main = GameObject.Find("Main");
		MuseumManager mm = main.GetComponentInChildren<MuseumManager>();

		if (m.storyToShare == 1) {
			m.textToShare = mm.story1Text;
			m.imageToShare = mm.story1Image;
		} else if (m.storyToShare == 2) {
			m.textToShare = mm.story2Text;
			m.imageToShare = mm.story2Image;
		} else if (m.storyToShare == 3) {
			m.textToShare = mm.story3Text;
			m.imageToShare = mm.story3Image;
		}

		Sprite s = Sprite.Create (m.imageToShare, new Rect(0, 0, m.imageToShare.width, m.imageToShare.height), new Vector2(0.5f, 0.5f));
		GameObject.Find ("ShareImage").GetComponentInChildren<Image>().sprite = s;
		GameObject.Find ("ShareText").GetComponentInChildren<Text>().text = m.textToShare;
	}

	public void CloseLoginOverlay() {
		mm.callBusy = false;

		Destroy (this.gameObject);
	}

	public void GoOnShareButtonPressed() {
		this.ShowSharePanel();
	}

	public void LogoutButtonPressed() {
		m.Logout();

		this.ShowLoginPanel();
	}

	public void LoginButtonPressed() {
		// Get e-mail
		var emailField = GameObject.Find ("EmailFieldText");
		string text = emailField.GetComponentInChildren<Text>().text;

		m.email = text;

		StartCoroutine(LoginSequence());
	}

	public IEnumerator LoginSequence() {
		// Show progress?

		yield return StartCoroutine(m.DoLogin());
		yield return StartCoroutine(m.GetSessionToken());

		// Close login screen
		ShowSharePanel();

		// Go on with Sharing
	}

	public void ShareButtonPressed() {
		StartCoroutine(ShareSequence());
	}

	public IEnumerator ShareSequence() {
		yield return StartCoroutine(m.DoPost());

		// TODO return a message that we succeeded

		// Disable the share button that this is about
		GameObject.Find ("Share" + m.storyToShare + "Button").GetComponent<Button>().interactable = false;

		CloseLoginOverlay();
	}
}
