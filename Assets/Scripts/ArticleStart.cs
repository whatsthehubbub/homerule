using UnityEngine;
using System.Collections;

public class ArticleStart : MonoBehaviour {


	public GameObject cameraDisplay;

	private WebCamTexture mCamera = null;

	public bool pictureTaken = false;

	// Use this for initialization
	void Start () {
		mCamera = new WebCamTexture ();
		mCamera.Play();

		cameraDisplay.GetComponent<Renderer>().material.mainTexture = mCamera;

//		cameraDisplay.GetComponent<SpriteRenderer>().material.mainTexture = mCamera;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakePhotoButton() {
		mCamera.Pause ();

		pictureTaken = true;

		// Turn button into retake button

		// Show the rest of the publish UI
	}
}
