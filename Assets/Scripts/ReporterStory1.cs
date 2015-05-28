using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory1 : MonoBehaviour {

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

			mm.story1Opinion = Story1OpinionAnswer.CLEAN;

			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject leave = cw.AddButton("Laten staan");
		leave.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het mag niet, maar het is niet lelijk. Laat maar staan.");

			mm.story1Opinion = Story1OpinionAnswer.LEAVE;
		
			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject display = cw.AddButton("Tentoonstellen");
		display.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zo mooi, zet er maar een hek voor en een lamp op!");

			mm.story1Opinion = Story1OpinionAnswer.DISPLAY;
			
			Invoke ("OpinionResponse1", 0.5f);
		});
	}

	public void OpinionResponse1() {
		cw.AddNPCBubble("Helemaal mee eens. Laten we de mensen hier van overtuigen.");

		GameObject hoe = cw.AddButton("Hoe?");
		hoe.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké, maar hoe?");
			
			Invoke ("TakePhotoOfObject", 0.5f);
		});
	}

	public void TakePhotoOfObject() {
		cw.AddNPCBubble("Er hangt daar een stuk behang waar een soldaat op heeft geschreven. Kun je daar een foto van maken?");

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddNPCBubble("Goeie foto!");

			Invoke ("FactQuestion", 0.5f);
		});
	}

	public void FactQuestion() {

		cw.AddNPCBubble("Die soldaat, Tony Crane, heeft ook op de muur geschreven.");
		cw.AddNPCBubble("Waarom denk jij dat hij dat heeft gedaan?");

		cw.AddNPCBubble("Was dat (1) omdat hij wilde opschrijven hoeveel Duitsers hij had neergeschoten, (2) omdat hij bang was, of (3) omdat hij zin had om de muur te bekladden.");

		GameObject count = cw.AddButton("Tellen");
		count.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Omdat hij wilde onthouden hoeveel vijanden hij had neergeschoten.");

			mm.story1Fact = Story1FactAnswer.COUNT;
			
			Invoke ("FactAnswer", 0.5f);
		});

		GameObject fear = cw.AddButton("Bang");
		fear.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Omdat hij bang was.");

			mm.story1Fact = Story1FactAnswer.FEAR;
			
			Invoke ("FactAnswer", 0.5f);
		});

		GameObject vandalism = cw.AddButton("Vandalisme");
		vandalism.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Omdat hij zin had om de muur te bekladden.");

			mm.story1Fact = Story1FactAnswer.VANDALISM;
			
			Invoke ("FactAnswer", 0.5f);
		});
	}

	public void FactAnswer() {
		switch (mm.story1Fact) {
		case Story1FactAnswer.COUNT:
			cw.AddNPCBubble("Klopt! Je ziet dat Tony Crane heeft opgeschreven hoeveel Duitsers hij heeft verwond of gedood.");
			break;
		case Story1FactAnswer.FEAR:
			cw.AddNPCBubble("Klopt! Tony Crane bevond zich in een gevaarlijke situatie en hij deed dit om zichzelf moed in te praten.");
			break;
		case Story1FactAnswer.VANDALISM:
			cw.AddNPCBubble("Dat was niet de reden. Hij deed het niet om schade aan te richten en hij schaamt zich er nu ook een beetje voor.");
			cw.AddNPCBubble("Tony Crane bevond zich in een gevaarlijke situatie en hij deed dit om zichzelf moed in te praten.");
			break;
		}

		GameObject aha = cw.AddButton ("O");
		aha.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("O, zit het zo!");

			cw.AddNPCBubble("Wat een verhaal hè? Het lijkt op wat er nu hier gebeurt. Dus ik ga je foto delen.");

			Invoke ("PieceOpinion", 0.5f);
		});
	}

	public void PieceOpinion() {
		cw.AddNPCBubble("Maar wat zal ik er bij zeggen? Net zei je dat de muurschildering [moest worden schoongemaakt / mocht blijven staan / moest worden tentoongesteld].");
		cw.AddNPCBubble("Hoe zal ik die man beschrijven?");

		GameObject vandal = cw.AddButton("Vandaal");
		vandal.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Die man is een vandaal, hij heeft de openbare ruimte beklad.");

			mm.story1OpinionDescription = Story1OpinionDescription.VANDAL;
			
			Invoke ("WritePiece", 0.5f);
		});
		
		GameObject citizen = cw.AddButton("Burger");
		citizen.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Die man is ook maar gewoon een burger. Wie weet waarom hij dit heeft gedaan!");

			mm.story1OpinionDescription = Story1OpinionDescription.CITIZEN;
			
			Invoke ("WritePiece", 0.5f);
		});
		
		GameObject artist = cw.AddButton("Kunstenaar");
		artist.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Die man is een kunstenaar, hij heeft iets prachtigs gemaakt.");
			
			mm.story1OpinionDescription = Story1OpinionDescription.ARTIST;
			
			Invoke ("WritePiece", 0.5f);
		});

	}

	public void WritePiece() {
		cw.AddNPCBubble("Oké! Ik heb er dit bericht van gemaakt:");

		string message = "Als er op de muur wordt geklad, is dat niet altijd om dingen zomaar kapot of vies te maken ";

		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			message += "maar soms wel! Mensen die de openbare ruimte bekladden moeten daarvoor verantwoordelijk worden gehouden, in de oorlog en nu ook.";
			break;
		case Story1OpinionDescription.CITIZEN:
			message += "er zit vast een diepere reden achter die we nu niet helemaal kennen. Mensen zijn mensen, in de oorlog en nu. Ze zitten ingewikkeld in elkaar.";
			break;
		case Story1OpinionDescription.ARTIST:
			message += "soms doen ze het om de wereld een stukje mooier te maken. Bij dit behang zorgde het voor een museumstuk, in het kattenstadje voor een fraaie muurschildering.";
			break;
		}

		cw.AddNPCBubble(message);
		mm.story1Text = message;

		GameObject send = cw.AddButton("Verzenden");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, verzenden maar!");
			
			Invoke ("SendPiece", 0.5f);
		});
	}

	public void SendPiece() {
		chat.SetActive(false);
		
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";

		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
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
		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			cw.AddNPCBubble("De agent heeft zijn zin gekregen, de muurschildering wordt schoongemaakt.");
			break;
		case Story1OpinionDescription.CITIZEN:
			cw.AddNPCBubble("Het loopt met een sisser af. Maar niet iedereen is blij.");
			break;
		case Story1OpinionDescription.ARTIST:
			cw.AddNPCBubble("Die man is nu een bekendheid! Maar niet iedereen is blij. ");
			break;
		}

		cw.AddNPCBubble("We hebben verteld wat er gebeurt. We hebben ook gezegd wat we er van vinden. Daardoor veranderen er dingen!");
		cw.AddNPCBubble("Op andere plekken zijn ook nog dingen te zien. Ik bel je weer als er iets te doen is.");

		Invoke ("ShowResultClose", 0.5f);
	}

	public void ShowResultClose() {
		GameObject ok = cw.AddButton("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");
			
			mm.callBusy = false;

			mm.story1Done = true;
			
			Destroy(chat);
			Destroy (this.audioSource);
		});
	}
}
