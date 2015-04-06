using UnityEngine;
using System.Collections;

public class ArticleStart : MonoBehaviour {


	public GameObject cameraDisplay;

	private WebCamTexture mCamera = null;

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
	}
}
