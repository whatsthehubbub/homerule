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
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Hela!");
		cw.AddNPCBubble("Er is iets aan de hand hier in de stad. Kijk eens op die muur!");
		
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
		
		cw.AddNPCBubble("Die man wordt opgepakt. Ik denk dat hij de muur heeft beklad.");
		cw.AddNPCBubble("Kun je mij helpen hier over te schrijven?");
		
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
		cw.AddNPCBubble("Het is verboden om zomaar muren te bekladden, maar met die tekst ben ik het eens.");

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
			cw.AddPlayerBubble("Het is verboden, maar ook een mooie tekst. Het mag blijven staan.");

			mm.story1Opinion = Story1OpinionAnswer.LEAVE;
		
			Invoke ("OpinionResponse1", 0.5f);
		});

		GameObject display = cw.AddButton("Inlijsten");
		display.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zo'n mooie tekst dat iemand het zou moeten inlijsten.");

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
		cw.AddNPCBubble("Er hangt bij jou een stuk afgescheurd behang. Kun je daar een foto van maken?");

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddNPCBubble("Goeie foto!");

			Invoke ("FactQuestion", 0.5f);
		});
	}

	public void FactQuestion() {
		cw.AddNPCBubble("Een Engelse soldaat, Tony Crane, heeft in de oorlog op dat behang geschreven.");
		cw.AddNPCBubble("Waarom denk jij dat hij dat heeft gedaan? Je mag overleggen!");

		cw.AddNPCBubble("Was dat (1) omdat hij wilde tellen hoeveel Duitsers hij had neergeschoten, (2) omdat hij bang was, of (3) omdat hij zin had om de muur te bekladden.");

		GameObject count = cw.AddButton("Tellen");
		count.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk omdat hij wilde tellen hoeveel vijanden hij had neergeschoten.");

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
			cw.AddPlayerBubble("Ik denk omdat hij ervan hield om muren te bekladden.");

			mm.story1Fact = Story1FactAnswer.VANDALISM;
			
			Invoke ("FactAnswer", 0.5f);
		});
	}

	public void FactAnswer() {
		switch (mm.story1Fact) {
		case Story1FactAnswer.COUNT:
			cw.AddNPCBubble("Klopt! Je ziet dat Tony Crane heeft opgeschreven hoeveel Duitsers hij heeft verwond of gedood.");
			cw.AddNPCBubble("Maar hij bevond zich ook in een gevaarlijke situatie. Hij deed dit vooral om zichzelf moed in te praten.");
			break;
		case Story1FactAnswer.FEAR:
			cw.AddNPCBubble("Klopt! Tony Crane bevond zich in een gevaarlijke situatie en hij deed dit om zichzelf moed in te praten.");
			break;
		case Story1FactAnswer.VANDALISM:
			cw.AddNPCBubble("Dat was niet de reden. Hij deed het niet om schade aan te richten en schaamt zich er nu een beetje voor.");
			cw.AddNPCBubble("Tony Crane bevond zich in een gevaarlijke situatie en deed dit om zichzelf moed in te praten.");
			break;
		}

		GameObject aha = cw.AddButton ("Aha");
		aha.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Aha, dus zo zit het!");

			cw.AddNPCBubble("Wat een verhaal, hè? Het lijkt op wat er hier bij mij gebeurt. Dus ik ga je foto delen.");

			Invoke ("PieceOpinion", 0.5f);
		});
	}

	public void PieceOpinion() {
		string opinionText = "";

		switch (mm.story1Opinion) {
		case Story1OpinionAnswer.CLEAN:
			opinionText = "moest worden schoongemaakt";
			break;
		case Story1OpinionAnswer.LEAVE:
			opinionText = "mocht blijven staan";
			break;
		case Story1OpinionAnswer.DISPLAY:
			opinionText = "moest worden tentoongesteld";
			break;
		}

		cw.AddNPCBubble("Maar wat zal ik erbij zeggen? Net zei je dat de muurschildering " + opinionText + ". Hoe zou jij die man noemen?");

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
			cw.AddPlayerBubble("Die man is ook maar gewoon een burger, net als jij en ik. Wie weet waarom hij dit heeft gedaan!");

			mm.story1OpinionDescription = Story1OpinionDescription.CITIZEN;
			
			Invoke ("WritePiece", 0.5f);
		});
		
		GameObject artist = cw.AddButton("Kunstenaar");
		artist.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Die man is een kunstenaar, hij heeft iets moois gemaakt.");
			
			mm.story1OpinionDescription = Story1OpinionDescription.ARTIST;
			
			Invoke ("WritePiece", 0.5f);
		});

	}

	public void WritePiece() {
		cw.AddNPCBubble("Oké! Ik heb er dit bericht van gemaakt:");

		string message = "Als er op muren wordt geklad, is dat niet altijd bedoeld om dingen kapot of vies te maken… ";

		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			message += "maar soms wel! Mensen die de openbare ruimte bekladden moeten gestraft worden, in de oorlog en nu.";
			break;
		case Story1OpinionDescription.CITIZEN:
			message += "er zit vast een diepere reden achter. Mensen zijn mensen, in de oorlog en nu. Ze zitten ingewikkeld in elkaar.";
			break;
		case Story1OpinionDescription.ARTIST:
			message += "soms doen ze het om de wereld mooier te maken. Het behang van Tony Crane kwam in een museum te hangen, en dat verdient die tekst bij mij ook.";
			break;
		}

		GameObject storyBubble = cw.AddNPCBubble(message);
		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;

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

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
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
			cw.AddNPCBubble("De agent heeft zijn zin gekregen, de tekst wordt weggehaald.");
			spriteString = "S1 slecht";
			break;
		case Story1OpinionDescription.CITIZEN:
			cw.AddNPCBubble("Het loopt met een sisser af. Maar niet iedereen is blij.");

			spriteString = "";
			break;
		case Story1OpinionDescription.ARTIST:
			cw.AddNPCBubble("Die man is nu een beroemdheid! Maar niet iedereen is blij. ");

			spriteString = "S1 goed";
			break;
		}

		// Show the sprite for the result of this episode
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite conclusionSprite = Resources.Load<Sprite>("Sprites/" + spriteString);
		displayImage.GetComponentInChildren<Image>().sprite = conclusionSprite;

		cw.AddNPCBubble("We hebben verteld wat er gebeurt. En we hebben gezegd wat we ervan vinden. Daardoor veranderen er dingen!");
		cw.AddNPCBubble("Op andere plekken in het museum zijn ook nog dingen te zien. Ik bel je weer als ik iets voor je heb.");

		Invoke ("ShowResultClose", 0.5f);
	}

	public void ShowResultClose() {
		GameObject ok = cw.AddButton("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");
			
			mm.callBusy = false;

			mm.story1Done = true;

			mm.UpdateTargetText();

			mm.storyQueue.Enqueue("OFFICERRESPONSE1");
			mm.storyQueue.Enqueue("REPORTERRESPONSE1");
			
			Destroy(chat);
			GameObject.Destroy(this);
		});
	}
}
