using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObserveerUI : MonoBehaviour {

	public GameObject cameraButton;
	public GameObject progressBar;
	public float progressSpeed = 0.1f;
	public AudioClip shutterSound;

	private GameObject main;

	void Awake()
	{
		progressBar.SetActive(false);
	}

	public void OnClickCamera()
	{
		StartCoroutine("MakePhoto");
	}

	IEnumerator MakePhoto()
	{	//foto maken
		audio.PlayOneShot(shutterSound);
		progressBar.SetActive(true);
		progressBar.GetComponent<Image>().fillAmount = 100;
		for(float f = 1; f > 0; f-=0.02f)
		{
			progressBar.GetComponent<Image>().fillAmount = f;
			yield return new WaitForSeconds(progressSpeed);
		}
		progressBar.SetActive(false);

		//observatie +1
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("FoundObservation");
		}

		//haal observatie knop weg
		this.gameObject.SetActive(false);
	}
}