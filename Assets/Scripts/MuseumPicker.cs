using UnityEngine;
using System.Collections;

public class MuseumPicker : MonoBehaviour {

	public GameObject titleScreen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MuseumPicked() {
		this.titleScreen.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
