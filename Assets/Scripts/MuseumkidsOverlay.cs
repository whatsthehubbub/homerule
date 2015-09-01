using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MuseumkidsOverlay : MonoBehaviour {

	public GameObject login;
	public GameObject share;
	
	private MuseumKids m;
	private MuseumManager mm;

	// Use this for initialization
	void Start () {
		Debug.Log ("in overlay start");

		m = GameObject.Find ("Main").GetComponent<MuseumKids>();

		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		mm.callBusy = true;

		if (LoggedIn()) {
			ShowSharePanel();
		} else {
			ShowLoginPanel();
		}
	}

//	// Update is called once per frame
//	void Update () {
//	
//	}

	public bool LoggedIn() {
		return !string.IsNullOrEmpty(m.authtoken) && !string.IsNullOrEmpty(m.sessiontoken);
	}

	public void ShowLoginPanel() {
		this.login.SetActive(true);
		this.share.SetActive(false);
	}

	public void ShowSharePanel() {
		this.login.SetActive(false);
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

		CloseLoginOverlay();
	}
}
