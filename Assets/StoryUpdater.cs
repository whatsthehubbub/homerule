using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryUpdater : MonoBehaviour {

	private Text txt;
	private MuseumManager m;

	// Use this for initialization
	void Start () {
		m = GameObject.Find("Main").GetComponent<MuseumManager>();

		txt = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		txt.text = "Verhalen: " + m.storiesFound;
	}
}
