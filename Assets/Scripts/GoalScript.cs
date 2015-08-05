using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void CloseGoal() {
		GameObject goal = GameObject.Find("GoalOverlay");
		
		Destroy (goal);
	}
}