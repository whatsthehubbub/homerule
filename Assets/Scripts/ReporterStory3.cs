using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory3 : MonoBehaviour {

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
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/portrait katja wide coffeeshop");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddDivider();
		
		cw.AddNPCBubble("Hoi!");
		cw.AddNPCBubble("Ben je bij die foto?");
		
		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, ik ben er.");
			
			Invoke ("ShowSituation", 0.5f);
		});
		
	}

	public void ShowSituation() {
		cw.AddNPCBubble("Super! Hier is ook weer iets aan de hand.");

		GameObject button = cw.AddButton("Wat?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			Invoke ("ShowSituationImage", 0.5f);
		});
	}

	public void ShowSituationImage() {
		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite introSprite = Resources.Load<Sprite>("Sprites/S3 intro wide");
		displayImage.GetComponentInChildren<Image>().sprite = introSprite;

		// Add the sprite we show in the video call to the archive
		GameObject bubble = cw.archivalChat.AddPlayerImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = introSprite;

		cw.AddNPCBubble("Ik zit hier met Frank. Hij heeft me alles verteld. Er is bewijs, de vrije vogels worden echt tegengewerkt.");

		cw.AddNPCBubble("Ik wil erover schrijven. Maar de politie is ons op het spoor! Kun je helpen?");

		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, dat weet je nu toch wel!");
			
			Invoke ("ShowTakePicture", 0.5f);
		});
	}

	public void ShowTakePicture() {
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		cw.AddNPCBubble("Kun je een foto maken van koningin Wilhelmina? Een foto van de foto?");

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				NativeToolkit.TakeCameraShot();
			} else {
				cw.ClearButtons();

				// Create a blank texture
				mm.story3Image = new Texture2D(2, 2, TextureFormat.ARGB32, false);

				Invoke ("ShowPictureResponse", 0.5f);
			}
		});
	}

	/*
	 * Methods to handle taking an image using CamerShot
	 */
	void CameraShotComplete(Texture2D img, string path)
	{
		if (path.Equals("Cancelled")) {
			// We don't actually have to do anything, the interface is still there
		} else {
			cw.ClearButtons();
			
			mm.story3Image = img;
			
			GameObject bubble = cw.AddPlayerImageBubble();
			bubble.name = "PlayerImage3";

			GameObject bubbleImage = bubble.transform.Find ("BubbleImage").gameObject;
			Image im = bubbleImage.GetComponentInChildren<Image>();
			
			im.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, mm.story3Image.width, mm.story3Image.height), new Vector2(0.5f, 0.5f));
			
			Invoke ("ShowPictureResponse", 0.5f);
		}

	}

	public void ShowPictureResponse() {
		cw.AddNPCBubble("Goeie foto!");

		Invoke ("ShowFactQuestion", 0.5f);
	}

	public void ShowFactQuestion() {
		cw.AddNPCBubble("In de oorlog mocht je niet laten zien dat je van het koningshuis hield.");

		cw.AddNPCBubble("Waarom, denk jij? Je mag overleggen!");

		cw.AddNPCBubble("Was dit omdat de Duitsers (1) tegen koningin Wilhelmina waren, (2) wilden dat je voor Hitler was, of (3) bang waren dat mensen zich zouden organiseren.");

		GameObject button1 = cw.AddButton("Tegen de koningin");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk omdat de Duitsers tegen koningin Wilhelmina waren.");

			mm.story3Fact = Story3FactAnswer.QUEEN;

			Invoke ("ShowFactAnswer", 0.5f);
		});

		GameObject button2 = cw.AddButton("Voor Hitler");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk omdat ze wilden dat je voor Hitler was, hun leider.");

			mm.story3Fact = Story3FactAnswer.HITLER;

			Invoke ("ShowFactAnswer", 0.5f);
		});

		GameObject button3 = cw.AddButton("Organiseren");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk dat ze bang waren dat mensen zich zouden organiseren.");

			mm.story3Fact = Story3FactAnswer.ORGANIZATION;

			Invoke ("ShowFactAnswer", 0.5f);
		});

	}

	public void ShowFactAnswer() {
		if (mm.story3Fact == Story3FactAnswer.QUEEN) {
			cw.AddNPCBubble("Klopt! De Duitse bezetter was de baas. De koningin was gevlucht naar Engeland.");
			cw.AddNPCBubble("Maar ze waren ook bang dat mensen met dezelfde politieke ideeën zich zouden organiseren. Bijvoorbeeld in verzetsgroepen.");

		} else if (mm.story3Fact == Story3FactAnswer.HITLER) {
			cw.AddNPCBubble("Klopt! De Duitse bezetter was de baas.");
			cw.AddNPCBubble("Maar ze waren ook bang dat mensen met dezelfde politieke ideeën zich zouden organiseren. Bijvoorbeeld in verzetsgroepen.");

		} else if (mm.story3Fact == Story3FactAnswer.ORGANIZATION) {
			cw.AddNPCBubble("Klopt! De Duitse bezetter was bang dat mensen met dezelfde politieke ideeën zich zouden organiseren. Bijvoorbeeld in verzetsgroepen.");
		}

		GameObject button = cw.AddButton("Waarom?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Waarom waren de Duitsers daar bang voor?");
			
			Invoke ("ShowMoreFacts", 0.5f);
		});
	}

	public void ShowMoreFacts() {
		cw.AddNPCBubble("Samen staan mensen sterk. Dat kon het Duitse gezag in gevaar brengen.");
		
		cw.AddNPCBubble("Daarom verdween de vrijheid om je politieke ideeën te laten zien. Zelfs foto's van de koningin werden verboden.");
			
		GameObject button = cw.AddButton ("Jemig");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jemig, wat een verhaal.");

			Invoke ("SharePicture", 0.5f);
		});
	}

	public void SharePicture() {
		cw.AddNPCBubble("De mensen konden niet meer zichzelf zijn. Net als de vogels nu. En de politie liegt erover!");

		cw.AddNPCBubble("Ik ga je foto delen. Maar hoe?");

		string source = "";
		if (mm.reporter2Source == Reporter2Source.FRANK) {
			source = "van Frank had";
		} else if (mm.reporter2Source == Reporter2Source.SELF) {
			source = "zelf had ontdekt";
		} else if (mm.reporter2Source == Reporter2Source.ANONYMOUS) {
			source = "van een geheime bron had";
		}

		cw.AddNPCBubble("Je zei dat je het nieuws over de vrije vogels " + source + ". Welke naam komt er bij het bericht?");

		GameObject button1 = cw.AddButton("Frank");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Frank kwam ermee. Zijn naam moet erbij!");

			mm.story3Attribution = Story3Attribution.FRANK;

			Invoke ("ShowArticle", 0.5f);
		});

		GameObject button2 = cw.AddButton("Katja");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jij bent de verslaggever. Jij hebt gekeken of het verhaal klopt. Dus jouw naam moet erbij.");

			mm.story3Attribution = Story3Attribution.KATJA;

			Invoke ("ShowArticle", 0.5f);
		});

		GameObject button3 = cw.AddButton("Geen naam");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Frank is de bron, maar hij is kwetsbaar. We moeten geheimhouden wie hij is. Anders wordt hij opgepakt.");

			mm.story3Attribution = Story3Attribution.ANONYMOUS;

			Invoke ("ShowArticle", 0.5f);
		});
	}

	public void ShowArticle() {
		cw.AddNPCBubble("Mee eens. Ik heb dit bericht geschreven:");

		string message = "Vrije vogel of koningsgezind, je moet jezelf kunnen zijn! Ook als de politie je dan lastig vindt. Was getekend… ";

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			message += "Frank de dichter";
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			message += "Katja de verslaggever";
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			message += "een geheime bron";
		}

		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

		GameObject imageBubble = cw.AddNPCImageBubble();
		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		GameObject imageObject = imageBubble.transform.Find ("BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, mm.story3Image.width, mm.story3Image.height), new Vector2(0.5f, 0.5f));

		GameObject storyBubble = cw.AddNPCBubble(message);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyBubble.GetComponentInChildren<Text>().color = Color.black;

		mm.story3Text = message;

		Invoke ("ShowConfirmSend", 0.5f);
	}

	public void ShowConfirmSend() {
		cw.AddNPCBubble("Weet je het zeker? De politie zit hier bovenop. Wat vinden zij hiervan?");
		
		GameObject button = cw.AddButton("Verzenden");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, verzenden maar!");
			
			Invoke ("ShowResult", 0.5f);
		});
	}

	public void ShowResult() {
		cw.AddNPCBubble("Ik heb je bericht verzonden…");

		cw.AddNPCBubble("Wauw, wat gebeurt er nu!");

		GameObject button = cw.AddButton("Laat zien");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			Invoke ("ShowResultImage", 0.5f);
		});
	}

	public void ShowResultImage() {
		cw.EnableBack();
		chat.SetActive(false);

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		string spriteString = "";

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("O nee, Frank wordt opgepakt. We hadden hem moeten beschermen!");

			spriteString = "S3 frank arrest wide";
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Help!");

			spriteString = "S3 katja arrest wide";
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Die agent is niet blij. Maar hij kan niks doen! En de mensen weten nu van de vrije vogels.");

			spriteString = "S3 agent weg wide";
		}

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite resultSprite = Resources.Load<Sprite>("Sprites/" + spriteString);
		displayImage.GetComponentInChildren<Image>().sprite = resultSprite;

		// Add the sprite we show in the video call to the archive
		GameObject bubble = cw.archivalChat.AddPlayerImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = resultSprite;

		Invoke ("ShowResultConclusion", 0.5f);
	}

	public void ShowResultConclusion() {
		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Jemig! Door op te schrijven wat er gebeurt, veranderen er dingen. En niet zo'n beetje ook.");
			cw.AddNPCBubble("Hopelijk komt het goed met Frank. Kijk jij ondertussen nog even rond in het museum?");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Katja heeft iets goeds gedaan. En is daar de dupe van geworden!");
			cw.AddNPCBubble("Hopelijk komt het goed. Kijk jij ondertussen nog even rond in het museum?");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Dit is geweldig! Door op te schrijven wat er gebeurt, veranderen er dingen. En niet zo'n beetje ook");
			cw.AddNPCBubble("Kijk rustig rond in het museum, er is nog van alles te zien.");
		}

		GameObject button = cw.AddButton("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké.");
			
			Invoke ("Close", 0.5f);
		});
	}
	
	public void Close() {
		mm.callBusy = false;
		
		mm.story3Done = true;

		Goal g = default(Goal);
		g.minor = -1;
		g.goalTextUnkown = "";
		g.goalTextFar = "Verken het museum";
		g.overlayTextUnknown = "";
		g.overlayTextFar = "Voel je vrij om het museum te verkennen. Je wordt gebeld als iemand je nodig heeft.";
		g.locationSprite = "";
		mm.goal = g;
		
		mm.storyQueue.Enqueue("OFFICERRESPONSE3");
		
		Destroy(chat);
		GameObject.Destroy(this);
	}
}