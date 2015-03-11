using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour
{	
	private GameObject map;

	void Start()
	{
		map = this.gameObject;
	}

	public void OpenMap()
	{
		this.gameObject.SetActive(true);
	}

	public void CloseMap()
	{
		this.gameObject.SetActive(false);
	}
}
