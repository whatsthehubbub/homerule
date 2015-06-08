using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterResponse2 : MonoBehaviour {

	public MuseumManager mm;
	
	public GameObject chat;
	public ChatWindow cw;

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
			ShowVideoCall();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowVideoCall() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Hoi, ik verveel me! Niets om over te schrijven.");
		cw.AddNPCBubble("Heb jij toevallig nog een nieuwtje opgepikt?");

		GameObject button = cw.AddButton ("Vogels");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Ik hoorde dat die mensen niet uit hun huis zijn gezet vanwege de veiligheid.");
			
			Invoke ("ShowMotivation", 0.5f);
		});
	}

	public void ShowMotivation() {
		cw.AddNPCBubble("Waarom dan wel?");

		GameObject button = cw.AddButton ("Vrijheid");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Omdat ze lastig zijn, omdat ze van vrijheid houden.");
			
			Invoke ("ShowChat", 0.5f);
		});
	}
	
	public void ShowChat() {
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
		
		cw.AddNPCBubble("Als dat klopt, is het een slechte zaak. Dan moet iedereen het te weten komen. Maar hoe heb jij er eigenlijk over gehoord? ");
		
		GameObject button1 = cw.AddButton("Frank");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Ik hoorde het van Frank, de man die “Vogelvrij” op de muur had geschreven.");

			mm.reporter2Source = Reporter2Source.FRANK;

			cw.ClearButtons();

			Invoke ("ShowAnswer", 0.5f);
		});

		GameObject button2 = cw.AddButton("Zelf ontdekt");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Ik heb het zelf ontdekt. Weet nog niet of ik mijn naam onder het bericht wil, de politie houdt ons in de gaten.");

			mm.reporter2Source = Reporter2Source.SELF;

			cw.ClearButtons();

			Invoke ("ShowAnswer", 0.5f);
		});

		GameObject button3 = cw.AddButton("Anonieme bron");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Ik heb het van een kwetsbaar iemand. We moeten geheimhouden wie hij is, anders wordt hij opgepakt.");

			mm.reporter2Source = Reporter2Source.ANONYMOUS;

			cw.ClearButtons();

			Invoke ("ShowAnswer", 0.5f);
		});
	}

	public void ShowAnswer() {
		cw.AddNPCBubble("Oké, ik begrijp het.");

		GameObject button = cw.AddButton ("Wat nu?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Wat moeten we nu doen?");
			
			Invoke ("ShowGoal", 0.5f);
		});
	}

	public void ShowGoal() {
		cw.AddNPCBubble("Ik heb een idee. Kun je in het Airborne Museum op zoek gaan naar de lepels en sieraden met koningin Wilhelmina erop?");

		cw.AddNPCBubble("Misschien kunnen we daar een foto van maken. Ik vertel straks wel waarom.");

		GameObject button = cw.AddButton ("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Oké");
			
			mm.callBusy = false;

			mm.targetText = "Ga naar de lepels en sieraden";
			mm.UpdateTargetText();

			cw.EnableBack();
			chat.SetActive(false);

			GameObject.Destroy(this);
		});
	}
}
