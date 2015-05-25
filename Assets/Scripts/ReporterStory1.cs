using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory1 : MonoBehaviour {

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
			
			StartStory ();
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartStory() {
		// Pause the change scene
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";
		
		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Hela!");
		cw.AddNPCBubble("Er is iets aan de hand hier in het kattenstadje. Kijk eens op die muur! ");
		
		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			Invoke ("ShowStorySituation", 0.5f);
		});
	}

	public void ShowStorySituation() {
		// TODO input start image here
//		GameObject displayImage = GameObject.Find ("DisplayImage");
//		Sprite introSprite = Resources.Load<Sprite>("Sprites/situatie intro");
//		displayImage.GetComponentInChildren<Image>().sprite = introSprite;
		
		cw.AddNPCBubble("Die man is opgepakt omdat hij die muur heeft beklad.");
		cw.AddNPCBubble("Kun je mij helpen hier over te schrijven?");
		
		GameObject ja = cw.AddButton("Natuurlijk");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Natuurlijk help ik je.");

			GameObject.Destroy(chat);
			
			chat = mm.reporterChatHistory;
			mm.reporterChatHistory.SetActive(true);
			
			cw = chat.GetComponent<ChatWindow>();
			
			Invoke ("GiveOpinion0", 0.8f);
		});
	}

	public void GiveOpinion0() {
		cw.AddNPCBubble("Het is verboden muren te bekladden, maar die schildering is best mooi.");

		Invoke ("GiveOpinion1", 0.5f);
	}

	public void GiveOpinion1() {

		cw.AddNPCBubble("Wat vind jij dat ermee moet gebeuren?");
		
		GameObject clean = cw.AddButton("Schoonmaken");
		clean.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het mag niet, dus die man moet het schoonmaken.");

			mm.story1PlayerOpinion = Story1OpinionAnswer.CLEAN;

			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject leave = cw.AddButton("Laten staan");
		leave.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het mag niet, maar het is niet lelijk. Laat maar staan.");

			mm.story1PlayerOpinion = Story1OpinionAnswer.LEAVE;
		
			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject display = cw.AddButton("Tentoonstellen");
		display.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zo mooi, zet er maar een hek voor en een lamp op!");

			mm.story1PlayerOpinion = Story1OpinionAnswer.DISPLAY;
			
			Invoke ("OpinionResponse1", 0.5f);
		});
	}

	public void OpinionResponse1() {
		cw.AddNPCBubble("Helemaal mee eens. Laten we de mensen hier van overtuigen.");

		GameObject hoe = cw.AddButton("Hoe?");
		hoe.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké, maar hoe?");
			
//			Invoke ("GiveOpinion1", 0.5f);
		});
	}


	public void ShowResultClose() {
		GameObject ok = cw.AddButton("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");
			
			mm.callBusy = false;
			
			Destroy(chat);
		});
	}
}
