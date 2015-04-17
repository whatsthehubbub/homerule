using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	[HideInInspector]public GameObject cam;

	void Start()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			cam.transform.Translate(new Vector3(cam.transform.position.x-0.1f, cam.transform.position.y, cam.transform.position.z));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			cam.transform.Translate(new Vector3(cam.transform.position.x+0.1f, cam.transform.position.y, cam.transform.position.z));
		}
	}
}
