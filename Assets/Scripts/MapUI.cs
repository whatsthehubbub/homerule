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
	}

	public void CloseMap()
	{
		this.gameObject.SetActive(false);
	}
}
