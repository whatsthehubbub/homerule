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
			
			StartCoroutine(StartStory ());
		});
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public IEnumerator StartStory() {
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

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Hela!");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Er is iets aan de hand, bij mij in de stad. Moet je zien!");

		yield return new WaitForSeconds(0.5f);
		
		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			ShowStorySituation();
		});
	}

	public void ShowStorySituation() {
		Sprite introSprite = Resources.Load<Sprite>("Sprites/S1 intro wide");

		// Show the situation in an image overlay
		GameObject imageOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/ImageOverlay"));
		imageOverlay.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		imageOverlay.name = "ImageOverlay";
		imageOverlay.GetComponent<Image>().sprite = introSprite;

		// We will only move to the next issue when this is called
		ImageOverlay.onImageOverlayClose += ShowStoryIntro;

		// Add the sprite we show in the video call to the archive
		GameObject bubble = cw.archivalChat.AddNPCImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = introSprite;

		// Already destroy the chat here
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
	}

	public void ShowStoryIntro() {
		StartCoroutine(ShowStoryIntroCoroutine());
	}

	public IEnumerator ShowStoryIntroCoroutine() {
		ImageOverlay.onImageOverlayClose -= ShowStoryIntro;

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Die man wordt opgepakt. Heeft hij die graffiti gemaakt?");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Kun je me helpen hierover te schrijven?");
		
		GameObject ja = cw.AddButton("Natuurlijk");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Natuurlijk help ik je.");

			StartCoroutine(GiveOpinion());
		});
	}

	public IEnumerator GiveOpinion() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Gebouwen bekladden is verboden. Maar dit is een belangrijke tekst.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Wat vind jij dat er moet gebeuren? Als je samen speelt, overleg!");

		yield return new WaitForSeconds(0.5f);
		
		GameObject clean = cw.AddButton("Schoonmaken");
		clean.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verboden. Die man moet het schoonmaken.");

			mm.story1Opinion = Story1OpinionAnswer.CLEAN;

			StartCoroutine(OpinionResponse1());
		});

		GameObject leave = cw.AddButton("Laten staan");
		leave.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verboden, maar een goede tekst. De regen spoelt het wel weg.");

			mm.story1Opinion = Story1OpinionAnswer.LEAVE;
		
			StartCoroutine(OpinionResponse1());
		});

		GameObject display = cw.AddButton("Inlijsten");
		display.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat een prachtige tekst! Het moet ingelijst worden.");

			mm.story1Opinion = Story1OpinionAnswer.DISPLAY;
			
			StartCoroutine(OpinionResponse1());
		});
	}

	public IEnumerator OpinionResponse1() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Mee eens. Dit moeten de mensen weten.");

		yield return new WaitForSeconds(0.5f);

		GameObject hoe = cw.AddButton("Hoe?");
		hoe.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké. Maar hoe gaan we het vertellen?");

			StartCoroutine(TakePhotoOfObject());
		});
	}

	public IEnumerator TakePhotoOfObject() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble(mm.museum.story1QuestionPre);

		yield return new WaitForSeconds(0.5f);

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				NativeToolkit.TakeCameraShot();
			} else {
				cw.ClearButtons();

				// Create a blank texture
				Sprite tempSprite = Resources.Load<Sprite>("Sprites/Locaties/placeholder");
				mm.story1Image = tempSprite.texture;

				StartCoroutine(FactQuestion());
			}
		});
	}
	
	// Method to handle taking the picture
	public void CameraShotComplete(Texture2D img, string path) {
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
			
			StartCoroutine(FactQuestion());
		}
	}

	public IEnumerator FactQuestion() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Goeie foto!");

		yield return new WaitForSeconds(0.5f);

		// Lift the intro phrases from the museum object
		foreach (var phrase in mm.museum.story1QuestionIntro) {
			cw.AddNPCBubble(phrase);
			
			yield return new WaitForSeconds(0.5f);
		}

		// Lift the potential answers and the player responses also from the museum object
		for (int i = 0; i < mm.museum.story1QuestionAnswerResponse.Length; i++) {
			System.Action doIt = () => {
				var localIndex = i;

				GameObject button = cw.AddButton(mm.museum.story1QuestionAnswerResponse[localIndex].Item1);

				button.GetComponentInChildren<Button>().onClick.AddListener(() => {
					cw.ClearButtons();
					
					cw.AddPlayerBubble(mm.museum.story1QuestionAnswerResponse[localIndex].Item2);
					
					mm.story1FactAnswer = localIndex;
					
					StartCoroutine(FactAnswer());
				});
			};

			doIt();
		}
	}

	public IEnumerator FactAnswer() {
		yield return new WaitForSeconds(0.5f);

		// Get the response from Katja based on the answer the player gave
		var tuple = mm.museum.story1QuestionAnswerResponse[mm.story1FactAnswer];

		foreach (var response in tuple.Item3) {
			cw.AddNPCBubble(response);

			yield return new WaitForSeconds(0.5f);
		}

		GameObject aha = cw.AddButton ("Aha");
		aha.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Aha, dus zo zit het!");

			StartCoroutine(PieceOpinion());
		});
	}

	public IEnumerator PieceOpinion() {
		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble(mm.museum.story1QuestionOutro);
		
		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Ik ga je foto delen. Want dit lijkt op wat er bij mij gebeurt.");
		
		yield return new WaitForSeconds(0.5f);

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

		yield return new WaitForSeconds(0.5f);

		GameObject vandal = cw.AddButton("Vandaal");
		vandal.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een vandaal, hij heeft een gebouw beklad.");

			mm.story1OpinionDescription = Story1OpinionDescription.VANDAL;

			StartCoroutine(WritePiece());
		});
		
		GameObject citizen = cw.AddButton("Burger");
		citizen.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een burger, net als jij en ik. Wie weet waarom hij dit heeft gedaan!");

			mm.story1OpinionDescription = Story1OpinionDescription.CITIZEN;
			
			StartCoroutine(WritePiece());
		});
		
		GameObject artist = cw.AddButton("Kunstenaar");
		artist.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Hij is een kunstenaar, die tekst is belangrijk.");
			
			mm.story1OpinionDescription = Story1OpinionDescription.ARTIST;
			
			StartCoroutine(WritePiece());
		});
	}

	public IEnumerator WritePiece() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Oké! Dit is mijn bericht:");

		yield return new WaitForSeconds(0.5f);

		string message = mm.museum.story1ArticleIntro + " ";

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


		GameObject imageBubble = cw.AddArticleImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story1Image, new Rect(0, 0, mm.story1Image.width, mm.story1Image.height), new Vector2(0.5f, 0.5f));

		yield return new WaitForSeconds(0.5f);

		cw.AddArticleBubble(mm.story1Text);

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik vind het een goed bericht. Jij ook?");

		yield return new WaitForSeconds(0.5f);

		GameObject send = cw.AddButton("Verzenden");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, verzenden maar!");

			StartCoroutine(SendPiece());
		});
	}

	public IEnumerator SendPiece() {
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

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Wauw. Moet je zien wat er gebeurt.");

		yield return new WaitForSeconds(0.5f);

		GameObject send = cw.AddButton("Laat zien");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			ShowResultImage();
		});
	}

	public void ShowResultImage() {
		string spriteString = "";

		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			spriteString = "S1 slecht wide";
			break;
		case Story1OpinionDescription.CITIZEN:
			spriteString = "S1 meh wide";
			break;
		case Story1OpinionDescription.ARTIST:
			spriteString = "S1 goed wide";
			break;
		}

		Sprite conclusionSprite = Resources.Load<Sprite>("Sprites/" + spriteString);

		// Show the situation in an image overlay
		GameObject imageOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/ImageOverlay"));
		imageOverlay.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		imageOverlay.name = "ImageOverlay";
		imageOverlay.GetComponent<Image>().sprite = conclusionSprite;
		
		ImageOverlay.onImageOverlayClose += ShowResultText;

		// Add the sprite we show in the overlay to the archive
		GameObject bubble = cw.archivalChat.AddNPCImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = conclusionSprite;

		// Destroy the chat here
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
	}

	public void ShowResultText() {
		StartCoroutine(ShowResultTextCoroutine());
	}

	public IEnumerator ShowResultTextCoroutine() {
		yield return new WaitForSeconds(0.5f);

		ImageOverlay.onImageOverlayClose -= ShowResultText;

		switch (mm.story1OpinionDescription) {
		case Story1OpinionDescription.VANDAL:
			cw.AddNPCBubble("De agent krijgt zijn zin. De vandaal maakt de muur schoon.");
			break;
		case Story1OpinionDescription.CITIZEN:
			cw.AddNPCBubble("Het loopt met een sisser af. Maar niet iedereen is blij.");

			break;
		case Story1OpinionDescription.ARTIST:
			cw.AddNPCBubble("Die kunstenaar is nu beroemd! Maar niet iedereen is blij.");

			break;
		}

		yield return new WaitForSeconds(0.5f);

		GameObject send = cw.AddButton("Oké");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké!");
			
			StartCoroutine(ShowResultClose());
		});
	}

	public IEnumerator ShowResultClose() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Leuk zeg. Door op te schrijven wat er gebeurt, veranderen er dingen.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik bel als ik je nodig heb. Kijk jij nog even rond?");

		yield return new WaitForSeconds(0.5f);

		GameObject ok = cw.AddButton("Doe ik");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Doe ik. Tot ziens, Katja!");

			cw.EnableBack();
			chat.SetActive(false);
			
			mm.callBusy = false;

			mm.story1Done = true;

			mm.goal = mm.museum.GetIdleGoal();

			mm.storyQueue.Enqueue("OFFICERRESPONSE1");
			mm.storyQueue.Enqueue("REPORTERRESPONSE1");

			GameObject.Destroy(this);
		});
	}
}
