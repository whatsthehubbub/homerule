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
		
		cw.AddNPCBubble(" Ahum. Ik wil toch nog even met u spreken.");
		
		GameObject button = cw.AddButton ("Oké.");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Vooruit dan.");
			
			Invoke ("ShowChatButton", 0.5f);
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
		GameObject storyBubble = cw.AddNPCBubble(mm.story3Text);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Die vrije vogels zorgen voor veel onrust. Dat kunnen we ons niet veroorloven. Daarom moesten we Frank oppakken.");
		
			Invoke ("ShowStatement", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Dit soort berichten zorgen voor veel onrust. We konden Katja niet langer haar gang laten gaan. De orde staat op het spel!");
		
			Invoke ("ShowStatement", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Ik heb het idee dat u meer weet over de bron van dit bericht, maar ik kan er niets mee. We houden Katja en Frank, en u, in de gaten.");
		
			Invoke ("ShowStatement", 0.5f);
		}
	}

	public void ShowStatement() {
		cw.AddNPCBubble("Door zo'n bericht gaan mensen anders denken. We moeten dit in de hand houden, anders loopt het helemaal fout.");

		GameObject button = cw.AddButton ("Oneens");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Daar ben ik het niet mee eens.");
			
			Invoke ("ShowQuestion", 0.5f);
		});
	}

	public void ShowQuestion() {
		cw.AddNPCBubble("O nee, waarom dan niet?");

		GameObject button1 = cw.AddButton ("Vrijheid");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Orde is belangrijk, maar niet zo belangrijk dat mensen hun vrijheid moeten opgeven.");
			
			Invoke ("ShowResponse", 0.5f);
		});

		GameObject button2 = cw.AddButton ("Oorlog");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Hier in het Airborne Museum zie je wat er gebeurt als mensen hun vrijheid moeten opgeven. Dan maar iets minder orde!");
			
			Invoke ("ShowResponse", 0.5f);
		});

		GameObject button3 = cw.AddButton ("Bang");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Je bent gewoon bang dat het een rommeltje wordt. Maar doe rustig aan, zo'n vaart zal het niet lopen.");
			
			Invoke ("ShowResponse", 0.5f);
		});
	}

	public void ShowResponse() {
		cw.AddNPCBubble("Hm, ik weet het niet. Ik volg ook maar de regels. Ik moet hier even over nadenken.");

		GameObject button = cw.AddButton ("Doe dat");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Oké, doe dat maar.");
			
			Invoke ("ShowResponse", 0.5f);
		});
	}

	public void ShowConclusion() {
		cw.EnableBack();
		chat.SetActive(false);

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/agent video 2");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;

		cw.AddNPCBubble("Hebt u alles al gezien in het museum? Als u klaar bent, kunt u teruggaan naar waar u bent begonnen.");

		GameObject button = cw.AddButton ("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Oké, tot ziens.");
			
			Invoke ("ShowClose", 0.5f);
		});
	}
	
	public void ShowClose() {
		mm.callBusy = false;

		mm.targetText = "Ga terug naar het beginpunt";
		mm.UpdateTargetText();
		
		GameObject.Destroy(chat);
		GameObject.Destroy(this);
	}
}
