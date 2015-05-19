using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterResponse : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	public MuseumManager mm;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
		call.name = "Katja belt";
		
		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
			GameObject.Destroy(call);
			ShowChatButton();
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowChatButton() {
		// Only lock stuff if the player actually picks up the phone. Otherwise you can just keep walking around and doing stuff.
		GameObject main = GameObject.Find("Main");
		MuseumManager mm = main.GetComponentInChildren<MuseumManager>();

		// Load the chat stuff
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();

		
		cw.AddNPCBubble("Die agenten willen me spreken. Maar ik doe toch niks verkeerd?");

		GameObject action = cw.AddButton("Ga naar ze toe");
		action.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ze zeggen dat je naar ze toe moet gaan.");

			Invoke ("ShowResponse", 0.5f);
		});
	}

	public void ShowResponse() {
		cw.AddNPCBubble("Dat ga ik mooi niet doen!");

		cw.AddNPCBubble("Ik vind het belangrijk om te vertellen wat er gebeurt.");

		GameObject ok = cw.AddButton("OK");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("OK.");

			mm.callBusy = false;

			chat.SetActive(false);
			GameObject.Destroy(this);
		});
	}
}
