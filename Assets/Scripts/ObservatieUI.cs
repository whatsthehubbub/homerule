using UnityEngine;
using System.Collections;

public class ObservatieUI : MonoBehaviour {

	private bool geopend = false;
	public GameObject hotspot;
	public GameObject cam;
	public GameObject oog;
	
	public void OnClickHotspot()
	{
		if (geopend == false)
		{
			geopend = true;
			oog.gameObject.GetComponent<Animator>().Play("open");
			cam.gameObject.GetComponent<Animator>().Play("open");
		}
		else if (geopend == true)
		{
			geopend = false;
			oog.gameObject.GetComponent<Animator>().Play("sluit");
			cam.gameObject.GetComponent<Animator>().Play("sluit");
		}
	}

	public void OnClickCamera()
	{	//maak foto (verhaal +1)
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("FoundObservation");
		}
		cam.SetActive(false);
	}

	public void OnClickOog()
	{
		Debug.Log ("Interessant...");
		//display beschrijving
	}

}
