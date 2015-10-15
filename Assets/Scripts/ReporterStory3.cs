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
			
			StartCoroutine(StartStory());
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
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/portrait katja wide coffeeshop");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

//		cw.AddDivider();
		
		cw.AddNPCBubble("Hoi!");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble(mm.museum.arrivedAtThirdLocation);

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, ik ben er.");
			
			StartCoroutine(ShowSituation());
		});
	}

	public IEnumerator ShowSituation() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Super! Hier is ook weer iets aan de hand.");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Wat?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			Invoke ("ShowSituationImage", 0.5f);
		});
	}

	public void ShowSituationImage() {
		Sprite introSprite = Resources.Load<Sprite>("Sprites/S3 intro wide");

		// Show the situation in an image overlay
		GameObject imageOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/ImageOverlay"));
		imageOverlay.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		imageOverlay.name = "ImageOverlay";
		imageOverlay.GetComponent<Image>().sprite = introSprite;

		// We will only move to the next issue when this is called
		ImageOverlay.onImageOverlayClose += ShowSituationText;

		// Add the sprite we show in the video call to the archive
		GameObject bubble = cw.archivalChat.AddNPCImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = introSprite;


		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
	}

	public void ShowSituationText() {
		ImageOverlay.onImageOverlayClose -= ShowSituationText;

		StartCoroutine(ShowSituationTextCoroutine());
	}

	public IEnumerator ShowSituationTextCoroutine() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik zit hier met Frank. Hij heeft me alles verteld. Er is bewijs, de vrije vogels worden echt tegengewerkt.");

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Ik wil erover schrijven. Maar de politie is ons op het spoor! Kun je helpen?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, dat weet je nu toch wel!");
			
			StartCoroutine(ShowTakePicture());
		});
	}

	public IEnumerator ShowTakePicture() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble(mm.museum.story3QuestionPre);

		yield return new WaitForSeconds(0.5f);

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				NativeToolkit.TakeCameraShot();
			} else {
				cw.ClearButtons();

				// Create a blank texture
				Sprite tempSprite = Resources.Load<Sprite>("Sprites/Locaties/placeholder");
				mm.story3Image = tempSprite.texture;

				StartCoroutine(ShowPictureResponse());
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

			GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
			Image im = bubbleImage.GetComponentInChildren<Image>();
			
			im.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, mm.story3Image.width, mm.story3Image.height), new Vector2(0.5f, 0.5f));
			
			StartCoroutine(ShowPictureResponse());
		}

	}

	public IEnumerator ShowPictureResponse() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Goeie foto!");

		yield return new WaitForSeconds(0.5f);


		// Lift the intro phrases from the museum object
		foreach (var phrase in mm.museum.story3QuestionIntro) {
			cw.AddNPCBubble(phrase);
			
			yield return new WaitForSeconds(0.5f);
		}


		// Lift the potential answers and the player responses also from the museum object
		for (int i = 0; i < mm.museum.story3QuestionAnswerResponse.Length; i++) {
			System.Action doIt = () => {
				var localIndex = i;
				
				GameObject button = cw.AddButton(mm.museum.story3QuestionAnswerResponse[localIndex].Item1);
				
				button.GetComponentInChildren<Button>().onClick.AddListener(() => {
					cw.ClearButtons();
					
					cw.AddPlayerBubble(mm.museum.story3QuestionAnswerResponse[localIndex].Item2);

					mm.story3FactAnswer = localIndex;
					
					StartCoroutine(ShowFactAnswer());
				});
			};
			
			doIt();
		}
	}

	public IEnumerator ShowFactAnswer() {
		yield return new WaitForSeconds(0.5f);


		// Get the response from Katja based on the answer the player gave
		var tuple = mm.museum.story3QuestionAnswerResponse[mm.story3FactAnswer];
		
		foreach (var response in tuple.Item3) {
			cw.AddNPCBubble(response);
			
			yield return new WaitForSeconds(0.5f);
		}


		GameObject button = cw.AddButton("Waarom?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble(mm.museum.story3QuestionWhy);

			StartCoroutine(ShowMoreFacts());
		});
	}

	public IEnumerator ShowMoreFacts() {
		yield return new WaitForSeconds(0.5f);

		foreach (var whyAnswer in mm.museum.story3QuestionWhyAnswer) {
			cw.AddNPCBubble(whyAnswer);
			
			yield return new WaitForSeconds(0.5f);
		}

		GameObject button = cw.AddButton ("Jemig");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jemig, wat een verhaal.");

			StartCoroutine(SharePicture());
		});
	}

	public IEnumerator SharePicture() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("De mensen konden niet meer zichzelf zijn. Net als de vogels nu. En de politie liegt erover!");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik ga je foto delen. Maar hoe?");

		string source = "";
		if (mm.reporter2Source == Reporter2Source.FRANK) {
			source = "van Frank had";
		} else if (mm.reporter2Source == Reporter2Source.SELF) {
			source = "zelf had ontdekt";
		} else if (mm.reporter2Source == Reporter2Source.ANONYMOUS) {
			source = "van een geheime bron had";
		}

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Je zei dat je het nieuws over de vrije vogels " + source + ". Welke naam komt er bij het bericht?");

		yield return new WaitForSeconds(0.5f);

		GameObject button1 = cw.AddButton("Frank");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Frank kwam ermee. Zijn naam moet erbij!");

			mm.story3Attribution = Story3Attribution.FRANK;

			StartCoroutine(ShowArticle());
		});

		GameObject button2 = cw.AddButton("Katja");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jij bent de verslaggever. Jij hebt gekeken of het verhaal klopt. Dus jouw naam moet erbij.");

			mm.story3Attribution = Story3Attribution.KATJA;

			StartCoroutine(ShowArticle());
		});

		GameObject button3 = cw.AddButton("Geen naam");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Frank is de bron, maar we moeten geheimhouden wie hij is. Anders wordt hij opgepakt.");

			mm.story3Attribution = Story3Attribution.ANONYMOUS;

			StartCoroutine(ShowArticle());
		});
	}

	public IEnumerator ShowArticle() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Mee eens. Ik heb dit bericht geschreven:");

		string message = "Vrije vogel of koningsgezind, je moet jezelf kunnen zijn! Ook als de politie je dan lastig vindt. Was getekend… ";

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			message += "Frank de dichter";
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			message += "Katja de verslaggever";
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			message += "een geheime bron";
		}

		GameObject imageBubble = cw.AddArticleImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, mm.story3Image.width, mm.story3Image.height), new Vector2(0.5f, 0.5f));

		yield return new WaitForSeconds(0.5f);

		cw.AddArticleBubble(message);

		mm.story3Text = message;

		StartCoroutine(ShowConfirmSend());
	}

	public IEnumerator ShowConfirmSend() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Weet je het zeker? De politie zit hier bovenop. Wat vinden zij hiervan?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton("Verzenden");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, verzenden maar!");
			
			StartCoroutine(ShowResult());
		});
	}

	public IEnumerator ShowResult() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik heb je bericht verzonden…");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Wauw, wat gebeurt er nu!");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Laat zien");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			Invoke ("ShowResultImage", 0.5f);
		});
	}

	public void ShowResultImage() {
		string spriteString = "";
		
		if (mm.story3Attribution == Story3Attribution.FRANK) {		
			spriteString = "S3 frank arrest wide";
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {		
			spriteString = "S3 katja arrest wide";
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			spriteString = "S3 agent weg wide";
		}

		Sprite resultSprite = Resources.Load<Sprite>("Sprites/" + spriteString);

		// Show the situation in an image overlay
		GameObject imageOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/ImageOverlay"));
		imageOverlay.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		imageOverlay.name = "ImageOverlay";
		imageOverlay.GetComponent<Image>().sprite = resultSprite;
		
		ImageOverlay.onImageOverlayClose += ShowResultText;
		
		// Add the sprite we show in the overlay to the archive
		GameObject bubble = cw.AddNPCImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = resultSprite;
	}

	public void ShowResultText() {
		ImageOverlay.onImageOverlayClose -= ShowResultText;

		StartCoroutine(ShowResultTextCoroutine());
	}

	public IEnumerator ShowResultTextCoroutine() {
		yield return new WaitForSeconds(0.5f);

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("O nee, Frank wordt opgepakt. We hadden hem moeten beschermen!");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Help!");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Die agent is niet blij. Maar hij kan niks doen! En de mensen weten nu van de vrije vogels.");
		}

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Jemig");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jemig!");
			
			StartCoroutine(ShowResultConclusion());
		});
	}

	public IEnumerator ShowResultConclusion() {
		yield return new WaitForSeconds(0.5f);

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Ja, jemig! Door op te schrijven wat er gebeurt, veranderen er dingen. En niet zo’n beetje ook.");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Hopelijk komt het goed met Frank. Loop jij ondertussen nog even rond?");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Katja heeft iets goeds gedaan. En is daar de dupe van geworden!");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Hopelijk komt het goed. Loop jij ondertussen nog even rond?");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Dit is geweldig! Door op te schrijven wat er gebeurt, veranderen er dingen. En niet zo’n beetje ook.");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Loop jij ondertussen nog even rond?");
		}

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké.");
			
			Invoke ("Close", 0.5f);
		});
	}
	
	public void Close() {
		mm.story3Done = true;

		cw.EnableBack();
		chat.SetActive(false);

		mm.callBusy = false;

		mm.goal = mm.museum.GetIdleGoal();
		
		mm.storyQueue.Enqueue("OFFICERRESPONSE3");

		GameObject.Destroy(this);
	}
}