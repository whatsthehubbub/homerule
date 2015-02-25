using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryUpdater : MonoBehaviour {

	private Text txt;
	private MuseumManager m;

	// Use this for initialization
	void Start () {
		try {
			m = GameObject.Find("Main").GetComponent<MuseumManager>();
		} catch {}

		txt = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (m) {
			txt.text = "Verhalen: " + m.observationsFound;
		}
	}
}
