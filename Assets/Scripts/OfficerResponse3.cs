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

		cw.AddDivider();
		
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
		storyImage.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, mm.story3Image.width, mm.story3Image.height), new Vector2(0.5f, 0.5f));

		GameObject storyBubble = cw.AddNPCBubble(mm.story3Text);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyBubble.GetComponentInChildren<Text>().color = Color.black;

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Die vogel zorgde voor veel onrust, daarom moesten we hem arresteren.");

			Invoke ("ShowStatement1", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Dit soort nieuws zorgt voor onrust, we konden die journalist niet laten lopen.");

			Invoke ("ShowStatement1", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Volgens mij weet u donders goed wie hierachter zit! We houden u in de gaten.");

			Invoke ("ShowStatement1", 0.5f);
		}
	}

	public void ShowStatement1() {
		cw.AddNPCBubble("We moeten hard optreden, dan kunnen we dit soort gedoe voortaan voorkomen.");

		GameObject button = cw.AddButton ("Oneens");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Daar ben ik het niet mee eens.");
			
			Invoke ("ShowStatement2", 0.5f);
		});
	}

	public void ShowStatement2() {
		cw.AddNPCBubble("O nee, waarom dan niet?");
		
		GameObject button = cw.AddButton ("Berichten");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Kijk nog eens naar de berichten van de afgelopen tijd. Die hebben veranderd hoe de mensen denken.");
			cw.AddPlayerBubble("Er zijn altijd manieren om te vertellen wat er gebeurt.");

			Invoke ("ShowStatement3", 0.5f);
		});
	}

	public void ShowStatement3() {
		cw.AddNPCBubble("Maar…");
		
		GameObject button = cw.AddButton ("Wat nu gebeurt");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			if (mm.story3Attribution == Story3Attribution.FRANK) {
				cw.AddPlayerBubble("Wat gebeurt er bijvoorbeeld als de arrestatie van Frank in het nieuws komt?");
			} else if (mm.story3Attribution == Story3Attribution.KATJA) {
				cw.AddPlayerBubble("Wat gebeurt er bijvoorbeeld als de arrestatie van Katja in het nieuws komt?");
			} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
				cw.AddPlayerBubble("Wat gebeurt er bijvoorbeeld als ik iedereen vertel hoe u nu tegen mij praat?");
			}

			Invoke ("ShowQuestion", 0.5f);
		});
	}

	public void ShowQuestion() {
		cw.AddNPCBubble("Maar… we kunnen iedereen toch niet zomaar zijn gang laten gaan?");

		GameObject button1 = cw.AddButton ("Vrijheid");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Orde is belangrijk, maar niet zo belangrijk dat mensen er hun vrijheid voor moeten opgeven.");
			
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

			cw.AddPlayerBubble("U bent gewoon bang dat het een rommeltje wordt. Maar zo'n vaart zal het heus niet lopen.");
			
			Invoke ("ShowResponse", 0.5f);
		});
	}

	public void ShowResponse() {
		cw.AddNPCBubble("Ik weet het niet… Ik volg ook maar gewoon de regels.");

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

		cw.AddNPCBubble("Wat brutaal. U gaat duidelijk met de verkeerde mensen om. Dit gesprek moest maar eens voorbij zijn.");

		cw.AddNPCBubble("Gaat u terug naar de ruimte met de medailles als u klaar bent?");

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
		mm.targetImage = Resources.Load<Sprite>("Sprites/locatie_start");
		mm.UpdateTargetText();
		
		GameObject.Destroy(chat);
		GameObject.Destroy(this);
	}
}
