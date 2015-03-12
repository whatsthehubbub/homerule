using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour
{	


	void Start()
	{

	}

	public void OpenMap()
	{
		this.gameObject.SetActive(true);

		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();
		if (mm != null) {
			mm.changeScene = false;
		}
	}

	public void CloseMap()
	{
		this.gameObject.SetActive(false);

		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();
		if (mm!= null) {
			mm.changeScene = true;
		}
	}
}
