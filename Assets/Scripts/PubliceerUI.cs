using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PubliceerUI : MonoBehaviour {

	public GameObject handButton;
	public GameObject progressBar;
	public float progressSpeed = 0.04f;
	public AudioClip typeSound;

	void Awake()
	{
		progressBar.SetActive(false);
	}

	public void OnClickHand()
	{
//		MuseumManager mm = GameObject.Find ("Main").GetComponent<MuseumManager>();
//		if (mm != null && mm.observationsFound > 0) {
//			StartCoroutine("DoType");
//		}
	}

	IEnumerator DoType()
	{
		GetComponent<AudioSource>().PlayOneShot(typeSound);
		progressBar.SetActive(true);
		progressBar.GetComponent<Image>().fillAmount = 100;
		for(float f = 1; f > 0; f-=0.02f)
		{
			progressBar.GetComponent<Image>().fillAmount = f;
			yield return new WaitForSeconds(progressSpeed);
		}
		progressBar.SetActive(false);


		GameObject main = GameObject.Find ("Main");
		if (main != null) {
			main.SendMessage("ConvertObservationIntoPublication");
		}
	}
}