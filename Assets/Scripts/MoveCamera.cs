using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	[HideInInspector]public GameObject camera;

	void Start()
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			camera.transform.Translate(new Vector3(camera.transform.position.x-0.1f, camera.transform.position.y, camera.transform.position.z));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			camera.transform.Translate(new Vector3(camera.transform.position.x+0.1f, camera.transform.position.y, camera.transform.position.z));
		}
	}
}
