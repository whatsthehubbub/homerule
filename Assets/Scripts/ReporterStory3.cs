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

		cw.AddDivider();
		
		cw.AddNPCBubble("Hoi!");
		cw.AddNPCBubble("Ben je al bij die munten?");
		
		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, ik ben er.");
			
			Invoke ("ShowSituation", 0.5f);
		});
		
	}

	public void ShowSituation() {
		cw.AddNPCBubble("Super. Er is ook weer iets aan de hand hier. Moet je zien!");

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
		Sprite introSprite = Resources.Load<Sprite>("Sprites/S3 intro");
		displayImage.GetComponentInChildren<Image>().sprite = introSprite;

		cw.AddNPCBubble("Ik zit hier met Frank, hij heeft me alles verteld. Er is bewijs dat de vrije vogels worden tegengewerkt.");

		cw.AddNPCBubble("Ik wil erover schrijven, maar de politie staat al voor de deur!");

		cw.AddNPCBubble("Kun je me helpen?");

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

		cw.AddNPCBubble("Kun je een foto maken van de munten met koningin Wilhelmina erop?");

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				GameObject imageBubble = cw.AddPlayerImageBubble();
				imageBubble.name = "PlayerRawImage3";

				NativeToolkit.TakeCameraShot();
			} else {
				Invoke ("ShowPictureResponse", 0.5f);
			}
		});
	}

	/*
	 * Methods to handle taking an image using CamerShot
	 */
	void CameraShotComplete(Texture2D img, string path)
	{
		//		string imagePath = path;
		//		Debug.Log ("Camera shot saved to: " + imagePath);
		//		Destroy (img);
		
		// TODO check if taking the picture has been cancelled.
		mm.story3Image = img;
		
		GameObject imageObject = GameObject.Find ("PlayerRawImage3");
		RawImage raw = imageObject.GetComponentInChildren<RawImage>();
		
		raw.texture = img;
		
		Invoke ("ShowPictureResponse", 0.5f);
	}

	public void ShowPictureResponse() {
		cw.AddNPCBubble("Goeie foto!");

		Invoke ("ShowFactQuestion", 0.5f);
	}

	public void ShowFactQuestion() {
		cw.AddNPCBubble("In de oorlog mocht je niet laten zien dat je liefhebber van het koningshuis was.");

		cw.AddNPCBubble("Waarom, denk jij? Je mag overleggen!");

		cw.AddNPCBubble("Was dit omdat de Duitsers (1) tegen koningin Wilhelmina waren, (2) wilden dat je voor Hitler was, of (3) niet wilden dat je je organiseerde.");

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
			cw.AddPlayerBubble("Ik denk omdat ze wilden dat je voor hun leider was, Adolf Hitler.");

			mm.story3Fact = Story3FactAnswer.HITLER;

			Invoke ("ShowFactAnswer", 0.5f);
		});

		GameObject button3 = cw.AddButton("Niet organiseren");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk dat ze niet wilden dat mensen zich organiseerden.");

			mm.story3Fact = Story3FactAnswer.ORGANIZATION;

			Invoke ("ShowFactAnswer", 0.5f);
		});

	}

	public void ShowFactAnswer() {
		if (mm.story3Fact == Story3FactAnswer.QUEEN) {
			cw.AddNPCBubble("Klopt! Toen de Duitsers Nederland hadden bezet, waren zij de baas. De koningin was gevlucht naar Engeland.");
			cw.AddNPCBubble("Maar de Duitsers wilden ook niet dat mensen aan de hand van hun politieke ideeën groepen vormden. Dan konden ze wel eens de controle verliezen.");

		} else if (mm.story3Fact == Story3FactAnswer.HITLER) {
			cw.AddNPCBubble("Klopt! Toen de Duitsers Nederland hadden bezet, waren zij de baas.");
			cw.AddNPCBubble("Maar de Duitsers wilden ook niet dat mensen aan de hand van hun politieke ideeën groepen vormden. Dan konden ze wel eens de controle verliezen.");

		} else if (mm.story3Fact == Story3FactAnswer.ORGANIZATION) {
			cw.AddNPCBubble("Klopt! De Duitsers wilden niet dat mensen aan de hand van hun politieke ideeën groepen vormden. Dan konden ze wel eens de controle verliezen.");
		}

		GameObject button = cw.AddButton("Aha");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Aha, dus zo zit het!");
			
			Invoke ("ShowMoreFacts", 0.5f);
		});
	}

	public void ShowMoreFacts() {
		cw.AddNPCBubble("Dit was niet het enige. De vlag en wimpel, afbeeldingen van koningshuisleden. Alles werd verboden.");

		cw.AddNPCBubble("De vrijheid om je politieke overtuiging te uiten, die verdween. Je kon niet meer zijn wie je wilde zijn. Veel mensen gingen het daarom stiekem doen.");
	
		GameObject button = cw.AddButton ("Jemig");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jemig, wat een verhaal.");

			Invoke ("SharePicture", 0.5f);
		});
	}

	public void SharePicture() {
		cw.AddNPCBubble("Het lijkt op wat er hier bij mij gebeurt. De vogels willen vrij zijn, maar mogen dat niet. En de politie liegt erover!");

		cw.AddNPCBubble("Dus ik ga je foto delen. Maar hoe?");

		string source = "";
		if (mm.reporter2Source == Reporter2Source.FRANK) {
			source = "van Frank had";
		} else if (mm.reporter2Source == Reporter2Source.SELF) {
			source = "zelf had ontdekt";
		} else if (mm.reporter2Source == Reporter2Source.ANONYMOUS) {
			source = "van een anonieme bron had";
		}

		cw.AddNPCBubble("Je zei dat je het nieuws over de vrije vogels " + source + ". Welke naam komt er bij het bericht?");

		GameObject button1 = cw.AddButton("Frank");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Frank kwam ermee. En hij wil graag vrij zijn. Zijn naam moet erbij!");

			mm.story3Attribution = Story3Attribution.FRANK;

			Invoke ("ShowArticle", 0.5f);
		});

		GameObject button2 = cw.AddButton("Katja");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jij bent hier de journalist. En jij hebt gecontroleerd of het bericht klopt. Dus jouw naam moet erbij.");

			mm.story3Attribution = Story3Attribution.KATJA;

			Invoke ("ShowArticle", 0.5f);
		});

		GameObject button3 = cw.AddButton("Anoniem");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Frank is de bron, maar hij is kwetsbaar. We moeten geheimhouden wie hij is, anders wordt hij opgepakt.");

			mm.story3Attribution = Story3Attribution.ANONYMOUS;

			Invoke ("ShowArticle", 0.5f);
		});
	}

	public void ShowArticle() {
		cw.AddNPCBubble("Oké! Ik heb er dit bericht van gemaakt:");

		string message = "Net als nu mochten de mensen in Nederland niet uitkomen voor hun politieke ideeën, vrije vogel of koningsgezind! Was getekend… ";

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			message += " Frank de kunstenaar";
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			message += " Katja de journalist";
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			message += " Anonieme bron";
		}

		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

		GameObject imageBubble = cw.AddNPCImageBubble();
		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		GameObject imageObject = imageBubble.transform.Find ("BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, 200, 300), new Vector2(0.5f, 0.5f));

		GameObject storyBubble = cw.AddNPCBubble(message);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyBubble.GetComponentInChildren<Text>().color = Color.black;

		mm.story3Text = message;

		Invoke ("ShowConfirmSend", 0.5f);
	}

	public void ShowConfirmSend() {
		cw.AddNPCBubble("Weet je het zeker? De politie leest zéker mee. Wat vinden zij hiervan?");
		
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
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		string spriteString = "";

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("O nee, Frank wordt opgepakt. We hadden hem moeten beschermen!");

			spriteString = "S3 vogel gearresteerd";
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Help!");

			spriteString = "S3 katja gearresteerd";
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Die agent is goed chagrijnig. Maar hij kan niks doen! En de mensen weten nu van de vrije vogels.");

			spriteString = "S3 agent dissed";
		}

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite resultSprite = Resources.Load<Sprite>("Sprites/" + spriteString);
		displayImage.GetComponentInChildren<Image>().sprite = resultSprite;

		Invoke ("ShowResultConclusion", 0.5f);
	}

	public void ShowResultConclusion() {
		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Nou ja, we hebben verteld wat er gebeurt. We hebben onze mening gegeven. Daardoor veranderen er dingen.");

			cw.AddNPCBubble("Hopelijk komt het goed met Frank en de vrije vogels. Kijk jij nog even rond in het museum?");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Katja heeft het juiste gedaan. Maar ze is er wel de dupe van geworden!");
			cw.AddNPCBubble("Hopelijk komt het goed. Kijk jij ondertussen nog even rond in het museum?");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Dit is geweldig! We hebben verteld wat er gebeurt. We hebben onze mening gegeven. Daardoor veranderen er dingen.");
			cw.AddNPCBubble("Kijk rustig rond als je wilt, er is nog van alles te zien in het museum.");
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

		mm.targetText = "Verken het museum";
		mm.UpdateTargetText();
		
		mm.storyQueue.Enqueue("OFFICERRESPONSE3");
		
		Destroy(chat);
		GameObject.Destroy(this);
	}
}