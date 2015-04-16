using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroOfficer : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IntroDoneButton() {
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("IntroOfficerDone");
		}
	}

	public void ShowChatButton() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";

		cw = chat.GetComponent<ChatWindow>();

		GameObject bubble = cw.AddNPCBubble("Niks ernstigs. Heeft u deze persoon gezien?");

		cw.AddButton("Hello");
		cw.AddButton("optie");
		cw.AddButton("Nog een optie");

		Invoke("ShowPlayerResponses", 0.5f);
	}

	public void ShowPlayerResponses() {
		GameObject response = cw.AddPlayerBubble("Nooit gezien.");
		response.GetComponentInChildren<Button>().onClick.AddListener(() => {ShowOfficerResponse();});
	}

	public void ShowOfficerResponse() {
		GameObject response = cw.AddNPCBubble("Spijtig, we zouden haar graag eens spreken.");
		Invoke ("ShowOfficerCloseOff", 0.5f);
	}

	public void ShowOfficerCloseOff() {
		GameObject response = cw.AddNPCBubble("Als u de reporter spreekt zeg dan maar dat ze ons moet opzoeken.");
		Invoke ("ShowPlayerCloseOff", 0.5f);
	}

	public void ShowPlayerCloseOff() {
		GameObject response = cw.AddPlayerBubble("Ok…");

		response.GetComponentInChildren<Button>().onClick.AddListener(() => { IntroDoneButton(); });
	}
}
