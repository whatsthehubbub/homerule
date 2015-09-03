using UnityEngine;
using System.Collections;

public class ImageOverlay : MonoBehaviour {

	public delegate void ImageOverlayHandler ();
	public static event ImageOverlayHandler onImageOverlayClose;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void CloseOverlay() {
		if (onImageOverlayClose != null) {
			onImageOverlayClose();
		}

		Destroy(this.gameObject);
	}
}
