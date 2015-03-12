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

	public void ThuisClicked() {
		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();
		if (mm!= null) {
			mm.changeScene = true;

			mm.NewLocation("HOME");
		}
	}

	public void KantoorClicked() {
		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();
		if (mm!= null) {
			mm.changeScene = true;

			mm.NewLocation("OFFICE");
		}
	}

	public void MarktClicked() {
		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();
		if (mm!= null) {
			mm.changeScene = true;

			mm.NewLocation("MARKET");
		}
	}

	public void PleinClicked() {
		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();
		if (mm!= null) {
			mm.changeScene = true;

			mm.NewLocation("SQUARE");
		}
	}

	public void StationClicked() {
		MuseumManager mm = GameObject.Find("Main").GetComponent<MuseumManager>();
		if (mm!= null) {
			mm.changeScene = true;

			mm.NewLocation("STATION");
		}
	}
}
