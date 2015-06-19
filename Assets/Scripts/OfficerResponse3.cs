using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OfficerResponse3 : MonoBehaviour {

	public MuseumManager mm;
	
	public GameObject chat;
	public ChatWindow cw;

	public AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent belt"));
		call.name = "Agent belt";

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
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/agent video 2");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Ahum. Ik wil toch nog even met u spreken.");
		
		GameObject button = cw.AddButton ("Oké.");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Vooruit dan.");
			
			Invoke ("ShowArticle", 0.5f);
		});
	}

	public void ShowArticle() {
		// Change to Chat UI
		GameObject.Destroy (chat);

		chat = mm.officerChatHistory;
		mm.officerChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		cw.AddNPCBubble("Fijn. Zeg, dit bericht hebt u vast eerder gezien.");

		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

		GameObject imageBubble = cw.AddNPCImageBubble();
		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		GameObject imageObject = imageBubble.transform.Find ("BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, 200, 300), new Vector2(0.5f, 0.5f));

		GameObject storyBubble = cw.AddNPCBubble(mm.story3Text);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyBubble.GetComponentInChildren<Text>().color = Color.black;

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Die vogel zorgde voor veel onrust. Dat kunnen we niet hebben. Daarom moesten we hem arresteren.");
		
			Invoke ("ShowStatement", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Dit soort berichten zorgen voor veel onrust. We konden Katja niet meer laten lopen. De orde staat op het spel!");
		
			Invoke ("ShowStatement", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Volgens mij weet u donders goed wie hierachter zit! We houden u in de gaten.");
		
			Invoke ("ShowStatement", 0.5f);
		}
	}

	public void ShowStatement() {
		cw.AddNPCBubble("Door zo'n bericht gaan mensen anders denken. We moeten dit in de hand houden, anders gaat het fout.");

		GameObject button = cw.AddButton ("Oneens");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Daar ben ik het niet mee eens.");
			
			Invoke ("ShowQuestion", 0.5f);
		});
	}

	public void ShowQuestion() {
		cw.AddNPCBubble("O nee, waarom dan niet?");

		GameObject button1 = cw.AddButton ("Vrijheid");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Orde is belangrijk, maar niet zo belangrijk dat mensen hun vrijheid moeten opgeven.");
			
			Invoke ("ShowResponse", 0.5f);
		});

		GameObject button2 = cw.AddButton ("Oorlog");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Hier in het museum zie je wat er gebeurt als mensen hun vrijheid moeten opgeven. Dan maar iets minder orde!");
			
			Invoke ("ShowResponse", 0.5f);
		});

		GameObject button3 = cw.AddButton ("Bang");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("U bent gewoon bang dat het een rommeltje wordt. Maar zo'n vaart zal het echt niet lopen.");
			
			Invoke ("ShowResponse", 0.5f);
		});
	}

	public void ShowResponse() {
		cw.AddNPCBubble("Hm, ik weet het niet. Ik volg gewoon de regels.");

		GameObject button = cw.AddButton ("Dan niet");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Nou, dan niet.");
			
			Invoke ("ShowConclusion", 0.5f);
		});
	}

	public void ShowConclusion() {
		cw.EnableBack();
		chat.SetActive(false);

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/agent video 2");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;

		cw.AddNPCBubble("Wat brutaal. U gaat met de verkeerde mensen om. Pas daarmee op.");

		cw.AddNPCBubble("Hebt u alles al gezien in het museum? Ga terug naar de ruimte met de medailles als u klaar bent.");

		GameObject button = cw.AddButton ("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Oké, tot ziens.");
			
			Invoke ("ShowClose", 0.5f);
		});
	}
	
	public void ShowClose() {
		mm.callBusy = false;

		mm.targetText = "Ga terug naar het begin";
		mm.UpdateTargetText();
		
		GameObject.Destroy(chat);
		GameObject.Destroy(this);
	}
}
