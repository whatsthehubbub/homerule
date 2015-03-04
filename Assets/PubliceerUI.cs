using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PubliceerUI : MonoBehaviour {

	public GameObject hand;
	public GameObject progress;
	public AudioClip typesound;

	private GameObject main;

	void Awake()
	{
		progress.SetActive(false);
	}

	public void OnClickHand()
	{
		StartCoroutine("DoType");
	}

	IEnumerator DoType()
	{
		audio.PlayOneShot(typesound);
		progress.SetActive(true);
		progress.GetComponent<Image>().fillAmount = 100;
		for(float f = 1; f > 0; f-=0.02f)
		{
			progress.GetComponent<Image>().fillAmount = f;
			yield return new WaitForSeconds(0.04f);
		}
		progress.SetActive(false);

		GameObject.Find ("Main").SendMessage("ConvertObservationIntoPublication");
	}
}