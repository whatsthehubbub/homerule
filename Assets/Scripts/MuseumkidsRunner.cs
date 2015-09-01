using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MuseumkidsRunner : MonoBehaviour {

	private MuseumKids m;

	// Use this for initialization
	void Start () {
		m = GameObject.Find ("Main Camera").GetComponent<MuseumKids>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetEmail() {
		var emailField = GameObject.Find ("EmailText");
		
		return emailField.GetComponentInChildren<Text>().text;
	}

	public void LoginButtonPressed() {
		m.email = GetEmail();

		StartCoroutine(m.DoLogin());
	}

	public void SessionButtonPressed() {
		Debug.Log ("Session button pressed");
		
		StartCoroutine(m.GetSessionToken());
	}

	public void PostButtonPressed() {
		Debug.Log ("Post button pressed");

		m.textToShare = "Test tekst";

		Sprite sprite = Resources.Load<Sprite>("Sprites/Locaties/behang");
		Texture2D tex = sprite.texture;

		m.imageToShare = tex;
		
		StartCoroutine(m.DoPost());
	}
}
