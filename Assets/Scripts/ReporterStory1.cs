using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory1 : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;
	
	public MuseumManager mm;

	public AudioSource audioSource;

	void OnEnable() {
		NativeToolkit.OnCameraShotComplete += CameraShotComplete;
	}
	
	void OnDisable ()
	{
		NativeToolkit.OnCameraShotComplete -= CameraShotComplete;
	}

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		mm.reporterChatHistory.GetComponent<ChatWindow>().FlushChildren();

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
	void Update () {
	
	}

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

//		cw.AddDivider();
		
		cw.AddNPCBubble("Hela!");
		cw.AddNPCBubble("Er is iets aan de hand, bij mij in de stad. Moet je zien!");
		
		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			Invoke ("ShowStorySituation", 0.5f);
		});
	}

	public void ShowStorySituation() {
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite introSprite = Resources.Load<Sprite>("Sprites/S1 intro wide");
		displayImage.GetComponentInChildren<Image>().sprite = introSprite;

		// Add the sprite we show in the video call to the archive
		GameObject bubble = cw.archivalChat.AddNPCImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = introSprite;
		
		cw.AddNPCBubble("Die man wordt opgepakt. Heeft hij die graffiti gemaakt?");
		cw.AddNPCBubble("Kun je me helpen hierover te schrijven?");
		
		GameObject ja = cw.AddButton("Natuurlijk");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Natuurlijk help ik je.");

			GameObject.Destroy(chat);
			
			chat = mm.reporterChatHistory;
			mm.reporterChatHistory.SetActive(true);
			
			cw = chat.GetComponent<ChatWindow>();
			cw.DisableBack();
			
			Invoke ("GiveOpinion0", 0.8f);
		});
	}

	public void GiveOpinion0() {
		cw.AddNPCBubble("Gebouwen bekladden is verboden. Maar dit is een belangrijke tekst.");

		Invoke ("GiveOpinion1", 0.5f);
	}
	
	public void GiveOpinion1() {
		cw.AddNPCBubble("Wat vind jij dat er moet gebeuren? Als je samen speelt, overleg!");
		
		GameObject clean = cw.AddButton("Schoonmaken");
		clean.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verboden. Die man moet het schoonmaken.");

			mm.story1Opinion = Story1OpinionAnswer.CLEAN;

			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject leave = cw.AddButton("Laten staan");
		leave.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verboden, maar een goede tekst. De regen spoelt het wel weg.");

			mm.story1Opinion = Story1OpinionAnswer.LEAVE;
		
			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject display = cw.AddButton("Inlijsten");
		display.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat een prachtige tekst! Het moet ingelijst worden.");

			mm.story1Opinion = Story1OpinionAnswer.DISPLAY;
			
			Invoke ("OpinionResponse1", 0.5f);
		});
	}

	public void OpinionResponse1() {
		cw.AddNPCBubble("Mee eens. Dit moeten de mensen weten.");

		GameObject hoe = cw.AddButton("Hoe?");
		hoe.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké. Maar hoe gaan we het vertellen?");
			
			Invoke ("TakePhotoOfObject", 0.5f);
		});
	}

	public void TakePhotoOfObject() {
		cw.AddNPCBubble("Ik weet iets. Maak jij een foto van het stuk behang in het museum?");

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				NativeToolkit.TakeCameraShot();
			} else {
				cw.ClearButtons();

				// Create a blank texture
				mm.story1Image = new Texture2D(2, 2, TextureFormat.ARGB32, false);

				Invoke ("FactQuestion", 0.5f);
			}
		});
	}
	
	// Method to handle taking the picture
	void CameraShotComplete(Texture2D img, string path) {
		if (path.Equals("Cancelled")) {
			// We don't actually have to do anything, the interface is still there
		} else {
			cw.ClearButtons();

			mm.story1Image = img;

			GameObject bubble = cw.AddPlayerImageBubble();
			bubble.name = "PlayerImage1";
			GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
			Image im = bubbleImage.GetComponentInChildren<Image>();
			
			im.sprite = Sprite.Create (mm.story1Image, new Rect(0, 0, mm.story1Image.width, mm.story1Image.height), new Vector2(0.5f, 0.5f));
			
			Invoke ("FactQuestion", 0.5f);
		}

	}

	public void FactQuestion() {
		cw.AddNPCBubble("Goeie foto!");

		cw.AddNPCBubble("Een Engelse soldaat schreef in de oorlog op dat behang.");
		cw.AddNPCBubble("Waarom deed hij dat, denk jij? Je mag overleggen!");

		cw.AddNPCBubble("Was dat (1) om te tellen hoeveel Duitsers hij had neergeschoten, (2) omdat hij bang was, of (3) omdat hij graag muren bekladde?");

		GameObject count = cw.AddButton("Tellen");
		count.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk om te tellen hoeveel Duitsers hij had neergeschoten.");

			mm.story1Fact = Story1FactAnswer.COUNT;
			
			Invoke ("FactAnswer", 0.5f);
		});

		GameObject fear = cw.AddButton("Bang");
		fear.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk omdat hij bang was.");

			mm.story1Fact = Story1FactAnswer.FEAR;
			
			Invoke ("FactAnswer", 0.5f);
		});

		GameObject vandalism = cw.AddButton("Kladden");
		vandalism.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk omdat hij graag muren bekladde.");

			mm.story1Fact = Story1FactAnswer.VANDALISM;
			
			Invoke ("FactAnswer", 0.5f);
		});
	}

	public void FactAnswer() {
		switch (mm.story1Fact) {
		case Story1FactAnswer.COUNT:
			cw.AddNPCBubble("Klopt! De soldaat telde hoeveel Duitsers hij neerschoot.");
			cw.AddNPCBubble("Bedenk wel: hij zat in een gevaarlijke situatie. Hij praatte zichzelf hiermee ook moed in.");
			break;
		case Story1FactAnswer.FEAR:
			cw.AddNPCBubble("Klopt! De soldaat zat in een gevaarlijke situatie. Hij praatte zichzelf hiermee moed in.");
			break;
		case Story1FactAnswer.VANDALISM:
			cw.AddNPCBubble("Dat was niet de reden. De soldaat schaamt zich er nu zelfs voor.");
			cw.AddNPCBubble("Hij zat in een gevaarlijke situatie. Hij praatte zichzelf hiermee moed in.");
			break;
		}

		GameObject aha = cw.AddButton ("Aha");
		aha.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Aha, dus zo zit het!");

			cw.AddNPCBubble("Lelijke woorden hè? Maar voor de soldaat was het een belangrijke tekst.");

			cw.AddNPCBubble("Ik ga je foto delen. Want dit lijkt op wat er bij mij gebeurt.");
		

			Invoke ("PieceOpinion", 0.5f);
		});
	}

	public void PieceOpinion() {
		string opinionText = "";

		switch (mm.story1Opinion) {
		case Story1OpinionAnswer.CLEAN:
			opinionText = "moet worden schoongemaakt";
			break;
		case Story1OpinionAnswer.LEAVE:
			opinionText = "mag blijven staan";
			break;
		case Story1OpinionAnswer.DISPLAY:
			opinionText = "moet worden ingelijst";
			break;
		}

		cw.AddNPCBubble("Wat zal ik schrijven? Net zei je dat de graffiti " + opinionText + ". Hoe zou jij die vogel noemen?");

		GameObject vandal = cw.AddButton("Vandaal");
		vandal.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een vandaal, hij heeft een gebouw beklad.");

			mm.story1OpinionDescription = Story1OpinionDescription.VANDAL;
			
			Invoke ("WritePiece", 0.5f);
		});
		
		GameObject citizen = cw.AddButton("Burger");
		citizen.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een burger, net als jij en ik. Wie weet waarom hij dit heeft gedaan!");

			mm.story1OpinionDescription = Story1OpinionDescription.CITIZEN;
			
			Invoke ("WritePiece", 0.5f);
		});
		
		GameObject artist = cw.AddButton("Kunstenaar");
		artist.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een kunstenaar, die tekst is belangrijk.");
			
			mm.story1OpinionDescription = Story1OpinionDescription.ARTIST;
			
			Invoke ("WritePiece", 0.5f);
		});

	}

	public void WritePiece() {
		cw.AddNPCBubble("Oké! Dit is mijn bericht:");

		string message = "Als iemand op een muur schrijft, is dat niet altijd slecht bedoeld. ";

		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			message += "Maar soms wel! Vandalen moeten gestraft worden. In de oorlog en nu.";
			break;
		case Story1OpinionDescription.CITIZEN:
			message += "Mensen zijn mensen, in de oorlog en nu. Er zit vast een diepere reden achter.";
			break;
		case Story1OpinionDescription.ARTIST:
			message += "Het behang van de soldaat hangt in een museum. De graffiti over vrije vogels hoort daar ook thuis!";
			break;
		}

		mm.story1Text = message;

		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

		GameObject imageBubble = cw.AddNPCImageBubble();
		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story1Image, new Rect(0, 0, mm.story1Image.width, mm.story1Image.height), new Vector2(0.5f, 0.5f));

		GameObject storyBubble = cw.AddNPCBubble(mm.story1Text);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyBubble.GetComponentInChildren<Text>().color = Color.black;

		cw.AddNPCBubble("Ik vind het een goed bericht. Jij ook?");

		GameObject send = cw.AddButton("Verzenden");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, verzenden maar!");
			
			Invoke ("SendPiece", 0.5f);
		});
	}

	public void SendPiece() {
		// Re enable the chat history's back button and hide it
		cw.EnableBack();
		chat.SetActive(false);

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/portrait katja wide");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddNPCBubble("Ik heb je bericht verzonden.");
		cw.AddNPCBubble("Wauw. Moet je zien wat er gebeurt.");

		GameObject send = cw.AddButton("Laat zien");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			Invoke ("PieceResult", 0.5f);
		});
	}

	public void PieceResult() {
		string spriteString = "";

		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			cw.AddNPCBubble("De agent krijgt zijn zin. De vandaal maakt de muur schoon.");
			spriteString = "S1 slecht wide";
			break;
		case Story1OpinionDescription.CITIZEN:
			cw.AddNPCBubble("Het loopt met een sisser af. Maar niet iedereen is blij.");

			spriteString = "S1 meh wide";
			break;
		case Story1OpinionDescription.ARTIST:
			cw.AddNPCBubble("Die kunstenaar is nu beroemd! Maar niet iedereen is blij.");

			spriteString = "S1 goed wide";
			break;
		}

		// Show the sprite for the result of this episode
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite conclusionSprite = Resources.Load<Sprite>("Sprites/" + spriteString);
		displayImage.GetComponentInChildren<Image>().sprite = conclusionSprite;

		// Add the sprite we show in the video call to the archive
		GameObject bubble = cw.archivalChat.AddNPCImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = conclusionSprite;

		cw.AddNPCBubble("Leuk zeg! Door op te schrijven wat er gebeurt, veranderen er dingen.");
		cw.AddNPCBubble("Ik bel als ik je nodig heb. In het museum is van alles te zien, dus kijk rustig rond.");

		Invoke ("ShowResultClose", 0.5f);
	}

	public void ShowResultClose() {
		GameObject ok = cw.AddButton("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");
			
			mm.callBusy = false;

			mm.story1Done = true;

			Goal g = default(Goal);
			g.goalText = "Verken het museum";
			g.overlayText = "Voel je vrij om het museum te verkennen. Je wordt gebeld als iemand je nodig heeft.";
			g.locationSprite = "";
			mm.goal = g;

			mm.storyQueue.Enqueue("OFFICERRESPONSE1");
			mm.storyQueue.Enqueue("REPORTERRESPONSE1");
			
			Destroy(chat);
			GameObject.Destroy(this);
		});
	}
}
