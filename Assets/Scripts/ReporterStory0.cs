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
		
		AudioClip ringtone = Resources.Load<AudioClip>("Audio/ringtone");
		this.audioSource = main.GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = ringtone;
		audioSource.Play ();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
		call.name = "Katja belt";
		
		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
			audioSource.Stop ();
			
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
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddNPCBubble("Hoi! Ik ben Katja. Ik werk als journalist.");
		
		GameObject hello = cw.AddButton("Hoi");
		hello.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Hoi Katja!");
			cw.ClearButtons();

			Invoke ("ShowHello", 0.5f);
		});
	}

	public void ShowHello() {
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		cw.AddNPCBubble("Ik zie dat je in het Airborne Museum bent. Da's cool.");

		cw.AddNPCBubble("Ik wil graag schrijven over vrijheid, en in het museum is van alles te zien over vrijheid. Jij kan me helpen!");

		GameObject how = cw.AddButton ("Hoe?");
		how.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Hoe dan?");
			
			Invoke ("ShowHow", 0.5f);
		});
	}

	public void ShowHow() {
		cw.AddNPCBubble("Loop door het museum en let goed op. Kijk wat er te leren valt over vrijheid. Ik bel als ik een vraag voor je heb.");

		GameObject ok = cw.AddButton ("Oké");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Dat kan ik wel.");
			
			Invoke ("ShowInstruction", 0.5f);
		});
	}

	public void ShowInstruction() {
		cw.AddNPCBubble("Fijn! Ga eerst kijken in deze gang. Daar hangt een stuk behang.");

		GameObject ok = cw.AddButton ("Is goed");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.EnableBack();
			chat.SetActive(false);
			
			chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
			chat.name = "VideoCall";

			// Show the correct sprite (Journalist)
			GameObject displayImage = GameObject.Find ("DisplayImage");
			Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
			displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
			
			cw = chat.GetComponent<ChatWindow>();
			cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

			cw.AddPlayerBubble("Is goed, ik ga op zoek naar het behang.");
			
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

			mm.targetText = "Ga naar de gang met het behang";
			mm.UpdateTargetText();

			Destroy(chat);
			GameObject.Destroy(this);

		});
	}
}
