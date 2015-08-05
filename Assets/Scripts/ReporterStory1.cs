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

		AudioClip ringtone = Resources.Load<AudioClip>("Audio/ringtone");
		this.audioSource = main.GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = ringtone;
		audioSource.Play ();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/ReporterCalling"));
		call.transform.SetParent(GameObject.Find ("Canvas").transform, false);
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
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/NewVideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";
		
		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddDivider();
		
		cw.AddNPCBubble("Hela!");
		cw.AddNPCBubble("Er is hier in de stad iets aan de hand. Kijk eens op die muur!");
		
		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			Invoke ("ShowStorySituation", 0.5f);
		});
	}

	public void ShowStorySituation() {
		// TODO input start image here
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite introSprite = Resources.Load<Sprite>("Sprites/S1 intro");
		displayImage.GetComponentInChildren<Image>().sprite = introSprite;
		
		cw.AddNPCBubble("Die man wordt opgepakt. Volgens mij heeft hij de muur beklad.");
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
		cw.AddNPCBubble("Het is verboden om muren te bekladden, maar die tekst is goed bedacht.");

		Invoke ("GiveOpinion1", 0.5f);
	}
	
	public void GiveOpinion1() {
		cw.AddNPCBubble("Wat vind jij dat er moet gebeuren? Als je samen speelt, overleg!");
		
		GameObject clean = cw.AddButton("Schoonmaken");
		clean.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verboden, dus die man moet het schoonmaken.");

			mm.story1Opinion = Story1OpinionAnswer.CLEAN;

			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject leave = cw.AddButton("Laten staan");
		leave.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verboden, maar ook een goede tekst. De regen spoelt het wel weg.");

			mm.story1Opinion = Story1OpinionAnswer.LEAVE;
		
			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject display = cw.AddButton("Inlijsten");
		display.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zo'n goede tekst! Het moet ingelijst worden.");

			mm.story1Opinion = Story1OpinionAnswer.DISPLAY;
			
			Invoke ("OpinionResponse1", 0.5f);
		});
	}

	public void OpinionResponse1() {
		cw.AddNPCBubble("Helemaal mee eens. Laten we de mensen hiervan overtuigen.");

		GameObject hoe = cw.AddButton("Hoe?");
		hoe.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké, maar hoe?");
			
			Invoke ("TakePhotoOfObject", 0.5f);
		});
	}

	public void TakePhotoOfObject() {
		cw.AddNPCBubble("In het museum hangt een stuk afgescheurd behang. Kun je daar een foto van maken?");

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				GameObject bubble = cw.AddPlayerImageBubble();
				bubble.name = "PlayerImage1";

				NativeToolkit.TakeCameraShot();
			} else {
				// Create a blank texture
				mm.story1Image = new Texture2D(2, 2, TextureFormat.ARGB32, false);

				Invoke ("FactQuestion", 0.5f);
			}
		});
	}
	
	// Method to handle taking the picture
	void CameraShotComplete(Texture2D img, string path) {
		// TODO check if taking the picture has been cancelled.

		mm.story1Image = img;

		GameObject bubble = GameObject.Find ("PlayerImage1");
		GameObject bubbleImage = bubble.transform.Find ("BubbleImage").gameObject;
		Image im = bubbleImage.GetComponentInChildren<Image>();

		im.sprite = Sprite.Create (mm.story1Image, new Rect(0, 0, mm.story1Image.width, mm.story1Image.height), new Vector2(0.5f, 0.5f));
		
		Invoke ("FactQuestion", 0.5f);
	}

	public void FactQuestion() {
		cw.AddNPCBubble("Goeie foto!");

		cw.AddNPCBubble("Een Engelse soldaat, Tony Crane, heeft in de oorlog op dat behang geschreven.");
		cw.AddNPCBubble("Waarom deed hij dat, denk jij? Je mag overleggen!");

		cw.AddNPCBubble("Was dat (1) omdat hij wilde tellen hoeveel vijanden hij had neergeschoten, (2) omdat hij bang was, of (3) omdat hij gewoon hield van muren bekladden?");

		GameObject count = cw.AddButton("Tellen");
		count.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk omdat hij wilde tellen hoeveel Duitsers hij had neergeschoten.");

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
			cw.AddPlayerBubble("Ik denk omdat hij hield van muren bekladden.");

			mm.story1Fact = Story1FactAnswer.VANDALISM;
			
			Invoke ("FactAnswer", 0.5f);
		});
	}

	public void FactAnswer() {
		switch (mm.story1Fact) {
		case Story1FactAnswer.COUNT:
			cw.AddNPCBubble("Klopt! Je ziet dat Tony Crane heeft geteld hoeveel Duitsers hij heeft verwond of gedood.");
			cw.AddNPCBubble("Bedenk wel, het was een gevaarlijke situatie. Tony deed dit vooral om zichzelf moed in te praten.");
			break;
		case Story1FactAnswer.FEAR:
			cw.AddNPCBubble("Klopt! Tony Crane zat in een gevaarlijke situatie. Hij deed dit vooral om zichzelf moed in te praten.");
			break;
		case Story1FactAnswer.VANDALISM:
			cw.AddNPCBubble("Dat was niet de reden. Hij schaamt zich er nu zelfs een beetje voor!");
			cw.AddNPCBubble("Tony Crane zat in een gevaarlijke situatie. Hij deed dit vooral om zichzelf moed in te praten.");
			break;
		}

		GameObject aha = cw.AddButton ("Aha");
		aha.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Aha, dus zo zit het!");

			cw.AddNPCBubble("Wat een verhaal, hè? Lelijke woorden, maar zo voelde Tony zich nou eenmaal.");

			cw.AddNPCBubble("Het lijkt op wat er hier bij mij gebeurt. Dus ik ga je foto delen.");
		

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

		cw.AddNPCBubble("Maar wat zal ik erbij zeggen? Net zei je dat de muurschildering " + opinionText + ". Hoe zou jij die vogel noemen?");

		GameObject vandal = cw.AddButton("Vandaal");
		vandal.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een vandaal, hij heeft de openbare ruimte beklad.");

			mm.story1OpinionDescription = Story1OpinionDescription.VANDAL;
			
			Invoke ("WritePiece", 0.5f);
		});
		
		GameObject citizen = cw.AddButton("Burger");
		citizen.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is gewoon een burger, net als jij en ik. Wie weet waarom hij dit heeft gedaan!");

			mm.story1OpinionDescription = Story1OpinionDescription.CITIZEN;
			
			Invoke ("WritePiece", 0.5f);
		});
		
		GameObject artist = cw.AddButton("Kunstenaar");
		artist.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een kunstenaar, hij heeft iets goeds gemaakt.");
			
			mm.story1OpinionDescription = Story1OpinionDescription.ARTIST;
			
			Invoke ("WritePiece", 0.5f);
		});

	}

	public void WritePiece() {
		cw.AddNPCBubble("Oké! Ik heb er dit bericht van gemaakt:");

		string message = "Als er op muren wordt geklad, is dat niet altijd bedoeld om dingen kapot of vies te maken… ";

		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			message += "maar soms wel! Wie de openbare ruimte bekladt, moet gestraft worden. In de oorlog en nu.";
			break;
		case Story1OpinionDescription.CITIZEN:
			message += "er zit vast een diepere reden achter. Mensen zijn mensen, in de oorlog en nu. Ze zitten ingewikkeld in elkaar.";
			break;
		case Story1OpinionDescription.ARTIST:
			message += "soms doen ze het om de wereld beter te maken. Tony's behang is een museumstuk, en die tekst over vrijheid verdient dat ook.";
			break;
		}

		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

		GameObject imageBubble = cw.AddNPCImageBubble();
		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		GameObject imageObject = imageBubble.transform.Find ("BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story1Image, new Rect(0, 0, mm.story1Image.width, mm.story1Image.height), new Vector2(0.5f, 0.5f));

		GameObject storyBubble = cw.AddNPCBubble(message);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyBubble.GetComponentInChildren<Text>().color = Color.black;

		mm.story1Text = message;

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

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/NewVideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddNPCBubble("Ik heb je bericht verzonden.");
		cw.AddNPCBubble("Wauw. Moet je zien wat er nu gebeurt!");

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
			cw.AddNPCBubble("De agent heeft zijn zin gekregen, de muur wordt schoongemaakt.");
			spriteString = "S1 slecht";
			break;
		case Story1OpinionDescription.CITIZEN:
			cw.AddNPCBubble("Het loopt met een sisser af. Maar niet iedereen is blij.");

			spriteString = "S1 meh";
			break;
		case Story1OpinionDescription.ARTIST:
			cw.AddNPCBubble("Die man is nu een beroemdheid! Maar niet iedereen is blij.");

			spriteString = "S1 goed";
			break;
		}

		// Show the sprite for the result of this episode
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite conclusionSprite = Resources.Load<Sprite>("Sprites/" + spriteString);
		displayImage.GetComponentInChildren<Image>().sprite = conclusionSprite;

		cw.AddNPCBubble("We hebben verteld wat er gebeurt. We hebben onze mening gegeven. Daardoor veranderen er dingen!");
		cw.AddNPCBubble("Er is nog meer te zien in het museum. Kijk rustig rond, ik bel je als ik iets voor je heb.");

		Invoke ("ShowResultClose", 0.5f);
	}

	public void ShowResultClose() {
		GameObject ok = cw.AddButton("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");
			
			mm.callBusy = false;

			mm.story1Done = true;

			mm.targetText = "Verken het museum";
			mm.targetImage = null;
			mm.UpdateTargetText();

			mm.storyQueue.Enqueue("OFFICERRESPONSE1");
			mm.storyQueue.Enqueue("REPORTERRESPONSE1");
			
			Destroy(chat);
			GameObject.Destroy(this);
		});
	}
}
