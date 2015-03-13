using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour
{	
	private MuseumManager mm;

	void Start()
	{
		mm = GameObject.Find("Main").GetComponent<MuseumManager>();
	}

	public void OpenMap()
	{
		this.gameObject.SetActive(true);

		if (mm != null) {
			mm.changeScene = false;
		}
	}

	public void CloseMap()
	{
		this.gameObject.SetActive(false);
		
		if (mm!= null) {
			mm.changeScene = true;
		}
	}

	public void ThuisClicked() {
		if (!Debug.isDebugBuild) return;
		
		if (mm!= null) {
			mm.changeScene = true;
			mm.NewLocation("HOME");
		}
	}

	public void KantoorClicked() {
		if (!Debug.isDebugBuild) return;

		if (mm!= null) {
			mm.changeScene = true;
			mm.NewLocation("OFFICE");
		}
	}

	public void MarktClicked() {
		if (!Debug.isDebugBuild) return;

		if (mm!= null) {
			mm.changeScene = true;
			mm.NewLocation("MARKET");
		}
	}

	public void PleinClicked() {
		if (!Debug.isDebugBuild) return;

		if (mm!= null) {
			mm.changeScene = true;
			mm.NewLocation("SQUARE");
		}
	}

	public void StationClicked() {
		if (!Debug.isDebugBuild) return;

		if (mm!= null) {
			mm.changeScene = true;
			mm.NewLocation("STATION");
		}
	}
}
