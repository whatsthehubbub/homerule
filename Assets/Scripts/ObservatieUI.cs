using UnityEngine;
using System.Collections;

public class ObservatieUI : MonoBehaviour {

	private bool geopend = false;
	private bool bekeken = false;
	public GameObject hotspot;
	public GameObject camera;
	public GameObject oog;
	
	public void OnClickHotspot()
	{
		if (geopend == false)
		{
			geopend = true;
			oog.gameObject.GetComponent<Animator>().Play("open");
			camera.gameObject.GetComponent<Animator>().Play("open");
		}
		else if (geopend == true)
		{
			geopend = false;
			oog.gameObject.GetComponent<Animator>().Play("sluit");
			camera.gameObject.GetComponent<Animator>().Play("sluit");
		}
	}

	public void OnClickCamera()
	{
		Debug.Log("Klik!");
		//maak foto (verhaal +1)
		bekeken = true;
		camera.SetActive(false);

		GameObject.Find ("Main").SendMessage("FoundObservation");
	}

	public void OnClickOog()
	{
		Debug.Log ("Interessant...");
		//display beschrijving
	}

}
