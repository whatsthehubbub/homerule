using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterResponse1 : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;
	
	public MuseumManager mm;

	public AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
		call.name = "Katja belt";

		AudioClip ringtone = Resources.Load<AudioClip>("Audio/ringtone");
		this.audioSource = main.GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = ringtone;
		audioSource.Play ();
		
		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
			audioSource.Stop ();

			GameObject.Destroy(call);
			ShowChatButton();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ShowChatButton() {
		
		// Load the chat stuff
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
		
		cw.AddNPCBubble("De politie wil me spreken. Maar ik doe toch niks verkeerd?");
		
		GameObject action = cw.AddButton("Bel ze");
		action.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ze zeggen dat je ze moet bellen.");
			
			Invoke ("ShowResponse", 0.5f);
		});
	}
	
	public void ShowResponse() {
		cw.AddNPCBubble("Dat ga ik mooi niet doen.");
		
		cw.AddNPCBubble("Ik vind het belangrijk om te vertellen wat er gebeurt. Daar ga ik gewoon mee door!");
		
		GameObject ok = cw.AddButton("Oké");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké");

			cw.EnableBack();
			chat.SetActive(false);

			mm.callBusy = false;

			mm.targetText = "Ga naar het bord 'verboden Arnhem te betreden'";
			mm.UpdateTargetText();
			
			chat.SetActive(false);
			GameObject.Destroy(this);
		});
	}
}
