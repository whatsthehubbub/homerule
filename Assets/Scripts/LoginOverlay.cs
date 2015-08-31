using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginOverlay : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	private MuseumKids m;

	public void CloseLoginOverlay() {
		Destroy (this.gameObject);
	}

	public void SendButtonPressed() {
		// Get e-mail
		var emailField = GameObject.Find ("EmailFieldText");
		string text = emailField.GetComponentInChildren<Text>().text;

		// Get Mumeuskids object
		m = GameObject.Find ("Main Camera").GetComponent<MuseumKids>();
		m.email = text;

		StartCoroutine(SendButtonSequence());
	}

	public IEnumerator SendButtonSequence() {
		// Show progress?

		yield return StartCoroutine(m.DoLogin());
		yield return StartCoroutine(m.GetSessionToken());

		// Close login screen

		CloseLoginOverlay();

		// Go on with Sharing
	}
}
