﻿using UnityEngine;
using System.Collections;

public class IntroReporter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IntroDoneButton() {
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("IntroReporterDone");
		}
	}
}