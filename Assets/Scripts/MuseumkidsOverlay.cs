using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MuseumkidsOverlay : MonoBehaviour {

	public GameObject login;
	public GameObject share;
	public GameObject loggedin;
	public GameObject shared;
	
	private MuseumKids m;
	private MuseumManager mm;

	// Use this for initialization
	void Start () {
		m = GameObject.Find ("MuseumkidsHolder").GetComponent<MuseumKids>();

		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		mm.callBusy = true;

		if (m.LoggedIn()) {
			ShowSharePanel();
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
		this.shared.SetActive(false);

		// Enable button
		GameObject.Find ("LoginButton").GetComponent<Button>().interactable = true;
	}

	public void ShowLoggedinPanel() {
		this.login.SetActive(false);
		this.share.SetActive(false);
		this.loggedin.SetActive(true);
		this.shared.SetActive(false);

		var text = "";
		if (m.accountstatus.Equals("ACCOUNT_NEW")) {
			text = "We hebben een account aangemaakt met " + m.email + " en je bent ingelogd. Je kan je aanmelding later afmaken op de Museumkids website.";
		} else if (m.accountstatus.Equals("ACCOUNT_REGISTERED")) {
			text = "Welkom terug! Je bent ingelogd als " + m.email + ".";
		}

		GameObject.Find("LoggedInExplanation").GetComponentInChildren<Text>().text = text;
	}

	public void ShowSharePanel() {
		this.login.SetActive(false);
		this.loggedin.SetActive(false);
		this.share.SetActive(true);
		this.shared.SetActive(false);

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

		// Disable current share button
		GameObject.Find ("ShareButton").GetComponent<Button>().interactable = true;
	}

	public void ShowSharedPanel() {
		this.login.SetActive(false);
		this.loggedin.SetActive(false);
		this.share.SetActive(false);
		this.shared.SetActive(true);
	}

	public void CloseOverlay() {
		mm.callBusy = false;

		Destroy (this.gameObject);
	}

	public void GoOnShareButtonPressed() {
		this.ShowSharePanel();
	}

	public void LoginButtonPressed() {
		// Get e-mail
		var emailField = GameObject.Find ("EmailFieldText");
		string text = emailField.GetComponentInChildren<Text>().text;

		if (!string.IsNullOrEmpty(text)) {
			m.email = text;
			
			StartCoroutine(LoginSequence());
		}
	}

	public IEnumerator LoginSequence() {
		// Hide button, show spinner
		GameObject.Find ("LoginButton").transform.localScale = new Vector3(0, 0, 0);
		GameObject.Find ("Spinner").transform.localScale = new Vector3(1, 1, 1);

		bool success = true;

		yield return StartCoroutine(m.DoLogin());

		if (string.IsNullOrEmpty(m.authtoken)) {
			success = false;
		} else {
			yield return StartCoroutine(m.GetSessionToken());

			if (string.IsNullOrEmpty(m.sessiontoken)) {
				success = false;
			}
		}

		// Show button, Hide spinner
		// We do this anyway, because we want to leave this view in a state that it can be used again.
		GameObject.Find ("LoginButton").transform.localScale = new Vector3(1, 1, 1);
		GameObject.Find ("Spinner").transform.localScale = new Vector3(0, 0, 0);

		if (success) {
			// Close login screen
			ShowLoggedinPanel();
		} else {
			// Show a message that logging in failed
			GameObject.Find ("MuseumkidsExplanation").GetComponentInChildren<Text>().text = "Sorry, het aanmelden is niet gelukt. Probeer het nog eens?";
		}
	}

	public void ShareButtonPressed() {
		StartCoroutine(ShareSequence());
	}

	public IEnumerator ShareSequence() {
		// Hide button, show spinner
		GameObject.Find ("ShareButton").transform.localScale = new Vector3(0, 0, 0);
		GameObject.Find ("Spinner").transform.localScale = new Vector3(1, 1, 1);

		yield return StartCoroutine(m.DoPost());

		bool success = true;

		if (m.storyToShare == 1 && !m.story1Shared) {
			success = false;
		} else if (m.storyToShare == 2 && !m.story2Shared) {
			success = false;
		} else if (m.storyToShare == 3 && !m.story3Shared) {
			success = false;
		}

		// Show button, Hide spinner
		// We do this anyway, because we want to leave this view in a state that it can be used again.
		GameObject.Find ("ShareButton").transform.localScale = new Vector3(1, 1, 1);
		GameObject.Find ("Spinner").transform.localScale = new Vector3(0, 0, 0);

		if (success) {
			GameObject.Find ("Share" + m.storyToShare + "Button").GetComponent<Button>().interactable = false;
			
			ShowSharedPanel();
		} else {
			GameObject.Find ("ConfirmationText").GetComponentInChildren<Text>().text = "Sorry, het delen is niet gelukt. Probeer het nog eens?";
		}
	}
}
