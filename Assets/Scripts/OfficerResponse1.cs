using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OfficerResponse1 : MonoBehaviour {

	public MuseumManager mm;
	
	public GameObject chat;
	public ChatWindow cw;

	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		// Enable the officer history button after our first chat
		mm.officerButton.SetActive(true);

		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/OfficerCalling"));
		call.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		call.name = "Officer Calling";

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
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/NewVideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";
		
		// Remove the static image in the video call
//		Destroy (GameObject.Find ("DisplayImage"));
		
		// Add the animated officer as a child of the chat
//		GameObject animatedOfficer = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent Animated"));
//		animatedOfficer.transform.parent = chat.transform;

		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/agent video 2");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Wolfsen. Aangenaam. Kunnen wij u spreken?");
		
		GameObject what = cw.AddButton ("Wat is er?");
		what.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Wat is er aan de hand?");
			
			Invoke ("ShowChatButton", 0.5f);
		});
	}

	public void ShowChatButton() {
		GameObject.Destroy(chat);
		
		chat = mm.officerChatHistory;
		mm.officerChatHistory.SetActive(true);

		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
		
		cw.AddNPCBubble("Weinig, gelukkig. Die kladderaar is opgefladderd.");
		cw.AddNPCBubble("Wij willen alleen nog weten of u deze verslaggever kent.");
		
		GameObject imageBubble = cw.AddNPCImageBubble();
		Sprite katjaPhotoSprite = Resources.Load<Sprite>("Sprites/katja foto");
		imageBubble.transform.Find("BubbleImage").GetComponentInChildren<Image>().sprite = katjaPhotoSprite;
		
		GameObject nooit = cw.AddButton("Nooit gezien");
		nooit.GetComponentInChildren<Button>().onClick.AddListener(() => {

			mm.officer1Answer = OfficerResponse1Answer.NEVER_SEEN;

			ShowPlayerResponse();	
		});
		
		GameObject waarom = cw.AddButton("Waarom?");
		waarom.GetComponentInChildren<Button>().onClick.AddListener(() => {
			mm.officer1Answer = OfficerResponse1Answer.WHY;
			ShowPlayerResponse();
		});
		
		GameObject ja = cw.AddButton("Ja");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			mm.officer1Answer = OfficerResponse1Answer.YES;

			ShowPlayerResponse();
		});
		
	}

	public void ShowPlayerResponse() {
		cw.ClearButtons();
		
		string playerResponseText = "";
		
		if (mm.officer1Answer == OfficerResponse1Answer.NEVER_SEEN) {
			playerResponseText = "Sorry, ik ken haar niet.";
		} else if (mm.officer1Answer == OfficerResponse1Answer.YES) {
			playerResponseText = "Ja, ik sprak haar net nog.";
		} else if (mm.officer1Answer == OfficerResponse1Answer.WHY) {
			playerResponseText = "Waarom vraagt u dit aan mij?";
		}
		
		cw.AddPlayerBubble(playerResponseText);
		
		Invoke("ShowOfficerResponse", 0.8f);
	}
	

	public void ShowOfficerResponse() {
		string officerResponseText = "";
		
		if (mm.officer1Answer == OfficerResponse1Answer.NEVER_SEEN) {
			officerResponseText = "Jammer. Wij willen haar spreken over wat ze schrijft.";
		} else if (mm.officer1Answer == OfficerResponse1Answer.YES) {
			officerResponseText = "Is dat zo? Interessant!";
		} else if (mm.officer1Answer == OfficerResponse1Answer.WHY) {
			officerResponseText = "Wij willen haar spreken over wat ze schrijft.";
		}
		
		cw.AddNPCBubble(officerResponseText);
		
		Invoke ("ShowOfficerCommand", 0.5f);
	}

	public void ShowOfficerCommand() {
		cw.AddNPCBubble("Komt u haar tegen, laat haar dan contact opnemen met ons.");
		
		GameObject ok = cw.AddButton ("Oké");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Oké…");
			cw.ClearButtons();
			
			Invoke ("ShowOfficerCloseOff", 0.5f);
		});
	}

	public void ShowOfficerCloseOff() {
		cw.AddNPCBubble("Bedankt voor uw tijd. Nog een fijne dag.");
		
		GameObject ok = cw.AddButton ("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Tot ziens.");
			
			cw.ClearButtons();
			
			cw.EnableBack();
			chat.SetActive(false);

			mm.callBusy = false;

			GameObject.Destroy(this);
		});
	}
}
