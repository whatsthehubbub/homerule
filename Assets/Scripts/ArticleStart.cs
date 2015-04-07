using UnityEngine;
using UnityEngine.UI;
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

	public void SubmitButton() {
		// Process everything

		bool yesOn = GameObject.Find ("ToggleYes").GetComponent<Toggle>().isOn;

	
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("ArticleSubmitted", yesOn);
		}


		// Load the resolution scene
		Application.LoadLevel("EventResolution");
	}
}
