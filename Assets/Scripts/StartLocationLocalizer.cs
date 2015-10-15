using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartLocationLocalizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		MuseumManager mm = main.GetComponentInChildren<MuseumManager>();

		GameObject.Find("LocationScopeText").GetComponent<Text>().text = mm.museum.museumIntroScope;

		GameObject.Find ("StartLocationImage").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Locaties/" + mm.museum.GetStartGoal().locationSprite);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
