using UnityEngine;
using System.Collections;

public class StoryCommunication : MonoBehaviour {

	public GameObject reply;
	public GameObject submitButton;

	// Use this for initialization
	void Start () {
	
		reply.SetActive(false);
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
