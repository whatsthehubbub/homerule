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
}
