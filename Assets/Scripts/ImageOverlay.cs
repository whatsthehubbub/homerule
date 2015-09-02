using UnityEngine;
using System.Collections;

public class ImageOverlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void CloseOverlay() {
		Destroy(this.gameObject);
	}
}
