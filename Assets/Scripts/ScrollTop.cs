using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollTop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ScrollRect rect = GetComponent<ScrollRect>();

		rect.verticalNormalizedPosition = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
