using UnityEngine;
using System.Collections;

public class StoryCommunication : MonoBehaviour {

	private GameObject reply;
	private GameObject submitButton;

	// Use this for initialization
	void Start () {
	
		reply = GameObject.Find ("Reply");
		reply.SetActive(false);

		submitButton = GameObject.Find ("SubmitButton");
		submitButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoodButton() {
		reply.SetActive(true);
		submitButton.SetActive(true);
	}

	public void BadButton() {
		reply.SetActive(true);
		submitButton.SetActive(true);
	}

	public void SubmitButton() {

	}
}
