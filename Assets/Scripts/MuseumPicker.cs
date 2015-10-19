using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MuseumPicker : MonoBehaviour {

	public GameObject titleScreen;
	public ToggleGroup toggleGroup;

	public MuseumManager mm;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MuseumPicked() {
		Toggle active = toggleGroup.ActiveToggles().FirstOrDefault();

		Museum m;
		if (active.gameObject.name.Equals("AirborneToggle")) {
			m = new AirborneMuseum();
		} else if (active.gameObject.name.Equals("DummyToggle")) {
			m = new DummyMuseum();
		} else {
			m = null;
		}
		mm.museum = m;

		this.titleScreen.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
