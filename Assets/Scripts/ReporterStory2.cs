using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory2 : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	public MuseumManager mm;

	public AudioSource audioSource;

	// Use this for initialization

	void OnEnable() {
		NativeToolkit.OnCameraShotComplete += CameraShotComplete;
	}
	
	void OnDisable ()
	{
		NativeToolkit.OnCameraShotComplete -= CameraShotComplete;
	}

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
			audioSource.Stop();

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
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/portrait katja wide");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

//		cw.AddDivider();

		cw.AddNPCBubble("Hoi!");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Er is weer iets aan de hand. Moet je zien!");

		yield return new WaitForSeconds(0.5f);
		
		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			StartCoroutine(ShowReporterResponse1());
		});
		
	}
	
	public IEnumerator ShowReporterResponse1() {
		yield return new WaitForSeconds(0.5f);

		Sprite introSprite = Resources.Load<Sprite>("Sprites/S2 intro wide");

		// Show the situation in an image overlay
		GameObject imageOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/ImageOverlay"));
		imageOverlay.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		imageOverlay.name = "ImageOverlay";
		imageOverlay.GetComponent<Image>().sprite = introSprite;
		
		// We will only move to the next issue when this is called
		ImageOverlay.onImageOverlayClose += ShowOpinion0;

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

	public void ShowOpinion0() {
		StartCoroutine(ShowOpinion0Coroutine());
	}

	public IEnumerator ShowOpinion0Coroutine() {
		ImageOverlay.onImageOverlayClose -= ShowOpinion0;

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Mensen moeten hun huis uit, omdat er in de buurt gebouwd wordt. Maar niet iedereen wil weg.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Help je me hierover te schrijven?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject ja = cw.AddButton("Ja");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, ik help je.");
			
			StartCoroutine(ShowOpinion1());
		});
	}
	
	public IEnumerator ShowOpinion1() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ze willen dat mensen tijdelijk verhuizen, omdat het gevaarlijk is waar ze wonen.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Wat vind je daarvan? Overleg gerust!");

		yield return new WaitForSeconds(0.5f);
		
		GameObject zielig = cw.AddButton("Zielig");
		zielig.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zielig. Waar moeten de mensen heen?");
			
			mm.story2Opinion = Story2OpinionAnswer.SAD;

			StartCoroutine(ShowReporterOpinionResponse1());
		});
		
		GameObject goed = cw.AddButton("Goed");
		goed.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Goed. Als er gevaar is, moeten ze de mensen beschermen.");
			
			mm.story2Opinion = Story2OpinionAnswer.GOOD;
			
			StartCoroutine(ShowReporterOpinionResponse1());
		});
		
		GameObject verkeerd = cw.AddButton("Verkeerd");
		verkeerd.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Verkeerd. Mensen mogen zelf bepalen of ze weggaan.");
			
			mm.story2Opinion = Story2OpinionAnswer.WRONG;
			
			StartCoroutine(ShowReporterOpinionResponse1());
		});
		
	}
	
	public IEnumerator ShowReporterOpinionResponse1() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Mee eens. Dit moeten de mensen weten.");

		yield return new WaitForSeconds(0.5f);
		
		GameObject ok = cw.AddButton("Hoe?");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké. Maar hoe gaan we het vertellen?");
			
			StartCoroutine(ShowFindObject());
		});
	}
	
	public IEnumerator ShowFindObject() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble(mm.museum.story2QuestionPre);

		yield return new WaitForSeconds(0.5f);
		
		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				NativeToolkit.TakeCameraShot();
			} else {
				cw.ClearButtons();

				// Create a blank texture
				Sprite tempSprite = Resources.Load<Sprite>("Sprites/Locaties/placeholder");
				mm.story2Image = tempSprite.texture;

				StartCoroutine(ShowFindObjectResponse());
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

			mm.story2Image = img;
			
			GameObject imageBubble = cw.AddPlayerImageBubble();
			imageBubble.name = "PlayerImage2";
			
			GameObject bubbleImage = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
			Image im = bubbleImage.GetComponentInChildren<Image>();
			
			im.sprite = Sprite.Create (mm.story2Image, new Rect(0, 0, mm.story2Image.width, mm.story2Image.height), new Vector2(0.5f, 0.5f));
			
			StartCoroutine(ShowFindObjectResponse());
		}

	}
	
	public IEnumerator ShowFindObjectResponse() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Goeie foto!");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké.");
		
			StartCoroutine(ShowObjectFacts());
		});
	}
	
	public IEnumerator ShowObjectFacts() {
		yield return new WaitForSeconds(0.5f);

		// Lift the intro phrases from the museum object
		foreach (var phrase in mm.museum.story2QuestionIntro) {
			cw.AddNPCBubble(phrase);
			
			yield return new WaitForSeconds(0.5f);
		}


		// Lift the potential answers and the player responses also from the museum object
		for (int i = 0; i < mm.museum.story2QuestionAnswerResponse.Length; i++) {
			System.Action doIt = () => {
				var localIndex = i;
				
				GameObject button = cw.AddButton(mm.museum.story2QuestionAnswerResponse[localIndex].Item1);
				
				button.GetComponentInChildren<Button>().onClick.AddListener(() => {
					cw.ClearButtons();
					
					cw.AddPlayerBubble(mm.museum.story2QuestionAnswerResponse[localIndex].Item2);
					
					mm.story2FactAnswer = localIndex;
					
					StartCoroutine(ShowFactResponse());
				});
			};
			
			doIt();
		}
	}
	
	public IEnumerator ShowFactResponse() {
		yield return new WaitForSeconds(0.5f);

		// Get the response from Katja based on the answer the player gave
		var tuple = mm.museum.story2QuestionAnswerResponse[mm.story2FactAnswer];
		
		foreach (var response in tuple.Item3) {
			cw.AddNPCBubble(response);
			
			yield return new WaitForSeconds(0.5f);
		}


		GameObject button = cw.AddButton("Aha");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Aha, dus zo zit het.");

			StartCoroutine(ShowFactResponseClose());
		});
	}
	
	public IEnumerator ShowFactResponseClose() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble(mm.museum.story2QuestionOutro);

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Ik ga je foto delen. Want dit lijkt op wat er bij mij gebeurt.");
		
		StartCoroutine(ShowArgument());
	}
	
	public IEnumerator ShowArgument() {
		yield return new WaitForSeconds(0.5f);

		string opinion = "";
		
		if (mm.story2Opinion == Story2OpinionAnswer.SAD) {
			opinion = "zielig";
		} else if (mm.story2Opinion == Story2OpinionAnswer.GOOD) {
			opinion = "goed";
		} else if (mm.story2Opinion == Story2OpinionAnswer.WRONG) {
			opinion = "verkeerd";
		}
		
		cw.AddNPCBubble("Wat zal ik schrijven? Net vond je het " + opinion + ". Vind je dat nog steeds?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject opinion1 = cw.AddButton("Zielig");
		opinion1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zielig. Je moet alles achterlaten en je weet niet waar je terechtkomt.");
			
			mm.story2FinalOpinion = Story2OpinionAnswer.SAD;
			
			StartCoroutine(ShowArgumentResponse());
		});
		
		GameObject opinion2 = cw.AddButton("Goed");
		opinion2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is goed. Als je gevaar loopt, moeten ze je daartegen beschermen.");
			
			mm.story2FinalOpinion = Story2OpinionAnswer.GOOD;
			
			StartCoroutine(ShowArgumentResponse());
		});
		
		GameObject opinion3 = cw.AddButton("Verkeerd");
		opinion3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verkeerd. Je moet zelf kunnen bepalen of je weggaat of niet.");
			
			mm.story2FinalOpinion = Story2OpinionAnswer.WRONG;
			
			StartCoroutine(ShowArgumentResponse());
		});
	}
	
	public IEnumerator ShowArgumentResponse() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Oké! Dit is mijn bericht:");

		yield return new WaitForSeconds(0.5f);

		// Display the story image
		GameObject imageBubble = cw.AddArticleImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story2Image, new Rect(0, 0, mm.story2Image.width, mm.story2Image.height), new Vector2(0.5f, 0.5f));

		string argument = "";
		if (mm.story2FinalOpinion == Story2OpinionAnswer.SAD) {
			argument = "zielig, want je moet alles achterlaten en je weet niet waar je terechtkomt. Als ze maar goed voor iedereen zorgen!";
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.GOOD) {
			argument = "goed, want als je gevaar loopt, moeten ze je daartegen beschermen. Dus de mensen moeten goed naar de politie luisteren.";
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.WRONG) {
			argument = "verkeerd, want je moet zelf kunnen bepalen of je weggaat of niet. Mensen die willen blijven moeten niet naar de politie luisteren.";
		}

		string storyText = mm.museum.story2ArticleIntro +
			"\n" + "Dat is " + argument;
		mm.story2Text = storyText;

		yield return new WaitForSeconds(0.5f);

		cw.AddArticleBubble(storyText);
		
		StartCoroutine(ShowSend ());
	}
	
	public IEnumerator ShowSend() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik vind het een goed bericht. Jij ook?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject send = cw.AddButton("Verzenden");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, verzenden maar!");
			
			StartCoroutine(ShowResult());
		});
	}
	
	public IEnumerator ShowResult() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik heb je bericht verzonden…");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Wauw. Moet je zien wat er gebeurt.");

		yield return new WaitForSeconds(0.5f);
		
		GameObject show = cw.AddButton("Laat zien");
		show.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			StartCoroutine(ShowResultResponse());
		});
	}
	
	public IEnumerator ShowResultResponse() {
		yield return new WaitForSeconds(0.5f);

		// Show the correct sprite (Journalist)
		string spriteName = "";
		if (mm.story2FinalOpinion == Story2OpinionAnswer.GOOD) { 
			spriteName = "S2 goed wide";
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.SAD) {
			spriteName = "S2 zielig wide";
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.WRONG) {
			spriteName = "S2 slecht wide";
		}
		Sprite showSprite = Resources.Load<Sprite>("Sprites/" + spriteName);

		// Show the situation in an image overlay
		GameObject imageOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/ImageOverlay"));
		imageOverlay.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		imageOverlay.name = "ImageOverlay";
		imageOverlay.GetComponent<Image>().sprite = showSprite;
		
		ImageOverlay.onImageOverlayClose += ShowResultResponseText;

		// Add the sprite we show in the overlay to the archive
		GameObject bubble = cw.AddNPCImageBubble();
		GameObject bubbleImage = bubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image image = bubbleImage.GetComponent<Image>();
		image.sprite = showSprite;
	}

	public void ShowResultResponseText() {
		ImageOverlay.onImageOverlayClose -= ShowResultResponseText;

		StartCoroutine(ShowResultResponseTextCoroutine());
	}

	public IEnumerator ShowResultResponseTextCoroutine() {
		yield return new WaitForSeconds(0.5f);

		if (mm.story2FinalOpinion == Story2OpinionAnswer.SAD) {
			cw.AddNPCBubble("Gelukkig wordt er goed voor de mensen gezorgd.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.GOOD) {
			cw.AddNPCBubble("De mensen houden zich aan de regels. Maar niet iedereen is blij.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.WRONG) {
			cw.AddNPCBubble("Sommige mensen luisteren niet. Ze krijgen ruzie en er wordt gevochten!");
		}

		yield return new WaitForSeconds(0.5f);
		
		GameObject ok = cw.AddButton("Oké");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké.");
			
			StartCoroutine(ShowResultClose());
		});
	}
	
	public IEnumerator ShowResultClose() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Heftig! Door op te schrijven wat er gebeurt, veranderen er dingen.");

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Ik bel als ik je nodig heb. Tot ziens!");

		yield return new WaitForSeconds(0.5f);

		GameObject ok = cw.AddButton("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");

			cw.EnableBack();
			chat.SetActive(false);

			mm.callBusy = false;

			mm.story2Done = true;

			mm.goal = mm.museum.GetIdleGoal();

			mm.storyQueue.Enqueue("OFFICERRESPONSE2");
			mm.storyQueue.Enqueue("ARTISTRESPONSE1");
			mm.storyQueue.Enqueue("REPORTERRESPONSE2");

			GameObject.Destroy(this);
		});
	}
}
