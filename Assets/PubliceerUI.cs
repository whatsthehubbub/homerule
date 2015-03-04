using UnityEngine;
using System.Collections;

public class PubliceerUI : MonoBehaviour {

	public GameObject hand;
	public GameObject progress;

	public void OnClickHand() {
		Debug.Log ("Hand Clicked");
	}

//	public void OnClickHotspot()
//	{
//		if (geopend == false)
//		{
//			geopend = true;
//			oog.gameObject.GetComponent<Animator>().Play("open");
//			camera.gameObject.GetComponent<Animator>().Play("open");
//		}
//		else if (geopend == true)
//		{
//			geopend = false;
//			oog.gameObject.GetComponent<Animator>().Play("sluit");
//			camera.gameObject.GetComponent<Animator>().Play("sluit");
//		}
//	}
//	
//	public void OnClickCamera()
//	{
//		Debug.Log("Klik!");
//		//maak foto (verhaal +1)
//		bekeken = true;
//		camera.SetActive(false);
//		
//		GameObject.Find ("Main").SendMessage("FoundObservation");
//	}
//	
//	public void OnClickOog()
//	{
//		Debug.Log ("Interessant...");
//		//display beschrijving
//	}

}
