using UnityEngine;
using System.Collections;

using CameraShot;

public class Example : MonoBehaviour {

	string log = "";
	void OnEnable()
	{

		CameraShotEventListener.onImageSaved += OnImageSaved;
		CameraShotEventListener.onImageLoad += OnImageLoad;
		CameraShotEventListener.onVideoSaved += OnVideoSaved;
		CameraShotEventListener.onError += OnError;
		CameraShotEventListener.onCancel += OnCancel;
	}

	void OnDisable()
	{
		CameraShotEventListener.onImageSaved -= OnImageSaved;
		CameraShotEventListener.onImageLoad -= OnImageLoad;
		CameraShotEventListener.onVideoSaved -= OnVideoSaved;
		CameraShotEventListener.onError -= OnError;
		CameraShotEventListener.onCancel -= OnCancel;
	}

	void OnImageSaved(string path)
	{
		Debug.Log ("Image Saved to gallery");
		log += "\nImage Saved to gallery :" + path;
	}

	void OnImageLoad(string path,Texture2D tex)
	{
		Debug.Log ("Image Captured by camera saved at location : "+path);
		GameObject.Find("Cube").GetComponent<Renderer>().material.mainTexture = tex;
		log += "\nImage Saved to gallery, loaded :" + path;
	}

	void OnVideoSaved(string path)
	{
		Debug.Log ("Video Saved at path : "+path);
		log += "\nVideo Saved at path :" + path;
	}

	void OnError(string errorMsg)
	{
		Debug.Log ("Error : "+errorMsg);
		log += "\nError : "+errorMsg;
	}

	void OnCancel()
	{
		Debug.Log ("OnCancel");
		log += "\nOnCancel";
	}

	void OnGUI()
	{
		GUILayout.Label (log);
		float btnWidth = 150;
		float btnHeight = 50;
		float y = Screen.height/2-btnHeight/2 - 50;
		if(GUI.Button(new Rect(Screen.width/2-btnWidth/2,y,btnWidth,btnHeight),"Capture Image"))
		{
			#if UNITY_ANDROID
			AndroidCameraShot.LaunchCameraForImageCapture();
			#elif UNITY_IPHONE
			IOSCameraShot.LaunchCameraForImageCapture();
			#endif
		}

		y += 100;
		if(GUI.Button(new Rect(Screen.width/2-btnWidth/2,y,btnWidth,btnHeight),"Get Texture"))
		{
			#if UNITY_ANDROID
			AndroidCameraShot.GetTexture2DFromCamera();
			#elif UNITY_IPHONE
			IOSCameraShot.GetTexture2DFromCamera();
			#endif
		}

		y += 100;
		if(GUI.Button(new Rect(Screen.width/2-btnWidth/2,y,btnWidth,btnHeight),"Record Video"))
		{
			#if UNITY_ANDROID
			AndroidCameraShot.LaunchCameraForVideoCapture();
			//AndroidCameraShot.LaunchCameraForVideoCapture(10);// record for 10 seconds
			#elif UNITY_IPHONE
			IOSCameraShot.LaunchCameraForVideoCapture();
			#endif
		}


	}
}
