using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterResponse : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	// Use this for initialization
	void Start () {
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
		changeScene = false;
		showKatjaIntroSurveillanceResponse = false;

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";
		
		cw = chat.GetComponent<ChatWindow>();
		
		cw.AddNPCBubble("Die agenten willen me spreken. Maar ik doe toch niks verkeerd?");

		GameObject action = cw.AddButton("Je moet naar ze toe");
		action.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ze zeggen dat je naar ze toe moet gaan.");

			Invoke ("ShowResponse", 0.5f);
		});
		

	}

	public void ShowResponse() {
		cw.AddNPCBubble("Dat ga ik mooi niet doen!");

		cw.AddNPCBubble("Ik vind het belangrijk om te vertellen wat er gebeurt.");

		GameObject ok = cw.AddButton("Ok");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ok");
			
			GameObject main = GameObject.Find("Main");
			MuseumManager mm = main.GetComponentInChildren<MuseumManager>();
			
			mm.changeScene = true;
			mm.NewLocation ("UNDERWAY");
		});
	}
}
