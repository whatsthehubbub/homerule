using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MuseumPicker : MonoBehaviour {

	public GameObject titleScreen;
	public ToggleGroup toggleGroup;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MuseumPicked() {
		Toggle active = toggleGroup.ActiveToggles().FirstOrDefault();

		Debug.Log (active.gameObject.name);

		this.titleScreen.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
