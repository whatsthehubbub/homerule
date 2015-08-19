using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory0 : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;
	
	public MuseumManager mm;
	
	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		// Show the reporter history button since we talked to her
		mm.reporterButton.SetActive(true);
		
		AudioClip ringtone = Resources.Load<AudioClip>("Audio/ringtone");
		this.audioSource = main.GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = ringtone;
		audioSource.Play ();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/ReporterCalling"));
		call.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		call.name = "Reporter Calling";
		
		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
			audioSource.Stop ();
			
			GameObject.Destroy(call);
			
			StartStory ();
		});
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void StartStory() {
		// Pause the change scene
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";
		
		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/portrait katja wide");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddNPCBubble("Hoi, mijn naam is Katja. Ik ben verslaggever.");
		
		GameObject hello = cw.AddButton("Hoi");
		hello.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Hoi Katja!");

			Invoke ("ShowHello", 0.5f);
		});
	}

	public void ShowHello() {
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		cw.AddNPCBubble("Je bent in het Airborne Museum! Da's cool.");

		cw.AddNPCBubble("Ik wil schrijven over vrijheid. In het museum is daarover van alles te zien. Kan je me helpen?");

		GameObject how = cw.AddButton ("Hoe?");
		how.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Hoe dan?");
			
			Invoke ("ShowHow", 0.5f);
		});
	}

	public void ShowHow() {
		cw.AddNPCBubble("Loop door het museum en kijk wat je kan leren over vrijheid. Ik bel als ik een vraag heb.");

		GameObject ok = cw.AddButton ("Goed");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Is goed!");
			
			Invoke ("ShowInstruction", 0.5f);
		});
	}

	public void ShowInstruction() {
		// Show image of where the location is
		GameObject imageBubble = cw.AddNPCImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Resources.Load<Sprite>("Sprites/Locaties/behang");

		cw.AddNPCBubble("Fijn. Ga je naar deze gang? Daar hangt een oud stuk behang.");

		GameObject ok = cw.AddButton ("Oké");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Oké, ik ga het behang zoeken.");
			
			Invoke ("ShowClose", 0.5f);
		});
	}

	public void ShowClose() {
		cw.AddNPCBubble("Super! Tot zo.");

		GameObject bye = cw.AddButton("Tot zo");
		bye.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot zo!");
			
			mm.callBusy = false;
			
			mm.story0Done = true;

			Goal g = default(Goal);
			g.goalText = "Zoek het behang";
			g.overlayText = "Ga op zoek naar het behang. Dat hangt op de eerste verdieping.";
			g.locationSprite = "behang";
			mm.goal = g;


			cw.EnableBack();
			chat.SetActive(false);
		});
	}
}
