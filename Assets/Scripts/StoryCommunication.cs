using UnityEngine;
using System.Collections;

public class StoryCommunication : MonoBehaviour {

	private GameObject reply;

	// Use this for initialization
	void Start () {
	
		reply = GameObject.Find ("Reply");
		reply.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoodButton() {
		reply.SetActive(true);
	}

	public void BadButton() {
		reply.SetActive(true);
	}
}
