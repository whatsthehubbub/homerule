using UnityEngine;
using System.Collections;

public class ReporterStory : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	// Use this for initialization
	void Start () {
		ShowChatButton();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void ShowChatButton() {
		// Only lock stuff if the player actually picks up the phone. Otherwise you can just keep walking around and doing stuff.

		// Load the chat stuff
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";
		
		cw = chat.GetComponent<ChatWindow>();
		
		cw.AddNPCBubble("Die agenten willen me spreken. Maar ik doe toch niks verkeerd?");
	}
}
