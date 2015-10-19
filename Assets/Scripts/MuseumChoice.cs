using UnityEngine;
using System.Collections;

public class MuseumChoice : MonoBehaviour {

	private static MuseumChoice instance;

	public string museumPicked;
	public bool pickMuseum;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
			return;
		}
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);

		pickMuseum = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Museum GetMuseum() {
		Museum m;

		if (museumPicked.Equals("Airborne")) {
			m = new AirborneMuseum();
		} else if (museumPicked.Equals("Dummy")) {
			m = new DummyMuseum();
		} else {
			m = null;
		}

		return m;
	}

}
