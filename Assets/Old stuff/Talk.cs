using UnityEngine;
using System.Collections;

public class Talk : MonoBehaviour
{
	private Animator anim;

	void Start()
	{
		anim = this.gameObject.GetComponent<Animator>();
	}
	
	public void TalkTo()
	{
		anim.Play("talk");
		//hierna moet ie nog een keer terug naar idle, maar voor het idee.
	}
}
