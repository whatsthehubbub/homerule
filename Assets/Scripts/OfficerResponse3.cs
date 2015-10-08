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
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/OfficerCalling"));
		call.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		call.name = "Officer Calling";

		AudioClip ringtone = Resources.Load<AudioClip>("Audio/ringtone");
		this.audioSource = main.GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = ringtone;
		audioSource.Play ();
		
		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
			audioSource.Stop ();

			GameObject.Destroy(call);
			StartCoroutine(ShowVideoCall());
		});
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public IEnumerator ShowVideoCall() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/portrait agent wide");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());

		cw.AddDivider();
		
		cw.AddNPCBubble("Ahum. Ik wil toch nog even met u spreken.");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton ("Vooruit");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Vooruit dan.");

			StartCoroutine(ShowArticle());
		});
	}

	public IEnumerator ShowArticle() {
		// Change to Chat UI
		GameObject.Destroy (chat);

		chat = mm.officerChatHistory;
		mm.officerChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Fijn. Dit bericht hebt u vast eerder gezien.");

		yield return new WaitForSeconds(0.5f);

		GameObject imageBubble = cw.AddArticleImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story3Image, new Rect(0, 0, mm.story3Image.width, mm.story3Image.height), new Vector2(0.5f, 0.5f));

		yield return new WaitForSeconds(0.5f);

		cw.AddArticleBubble(mm.story3Text);

		yield return new WaitForSeconds(0.5f);

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Die flierefluiters zorgden voor onrust. Daarom hebben we de dichter gearresteerd.");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Dit soort nieuws zorgt voor onrust. We konden die schrijvende pers niet laten lopen.");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("U weet donders goed wie hierachter zit! We houden u in de gaten.");
		}

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("We moeten hard optreden. Dan kunnen we dit gedoe voortaan voorkomen.");

		yield return new WaitForSeconds(0.5f);

		GameObject disagreeButton = cw.AddButton ("Oneens");
		disagreeButton.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Daar ben ik het niet mee eens.");
			
			mm.officer3Response = Officer3Response.DISAGREE;

			StartCoroutine(OfficerQuestionWhy1());
		});
		
		GameObject understandButton = cw.AddButton ("Snap ik");
		understandButton.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik snap dat u dat zegt.");
			
			mm.officer3Response = Officer3Response.UNDERSTAND;

			StartCoroutine(OfficerQuestionWhy2());
		});
	}

	public IEnumerator OfficerQuestionWhy1() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("O nee, waarom dan niet?");

		yield return new WaitForSeconds(0.5f);

		StartPlayerRecap();
	}

	public IEnumerator OfficerQuestionWhy2() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Fijn! Hoe kijkt u hier zelf tegenaan?");

		yield return new WaitForSeconds(0.5f);

		StartPlayerRecap();
	}

	public void StartPlayerRecap() {
		GameObject button = cw.AddButton ("Berichten");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("De berichten van de afgelopen tijd zeggen eigenlijk alles.");

			StartCoroutine(PlayerRecap1());
		});
	}

	public IEnumerator PlayerRecap1() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Hoe bedoelt u?");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Graffiti");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			StartCoroutine(PlayerRecap1Coroutine());
		});
	}

	public IEnumerator PlayerRecap1Coroutine() {
		cw.AddPlayerBubble("U weet vast nog wel dat de dichter graffiti had gespoten.");

		yield return new WaitForSeconds(0.5f);
		
		// Boiler plate to include the story
		GameObject imageBubble = cw.AddArticleImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story1Image, new Rect(0, 0, mm.story1Image.width, mm.story1Image.height), new Vector2(0.5f, 0.5f));

		yield return new WaitForSeconds(0.5f);

		cw.AddArticleBubble(mm.story1Text);
		
		string resultText = "";
		
		switch (mm.story1Opinion) {
		case Story1OpinionAnswer.CLEAN:
			resultText = "moest hij de muur boenen";
			break;
		case Story1OpinionAnswer.LEAVE:
			resultText = "mocht de graffiti blijven staan";
			break;
		case Story1OpinionAnswer.DISPLAY:
			resultText = "werd hij beroemd";
			break;
		}

		yield return new WaitForSeconds(0.5f);
		
		cw.AddPlayerBubble("Door wat wij schreven " + resultText + ".");
		
		StartCoroutine(PlayerRecap2());
	}

	public IEnumerator PlayerRecap2() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ja…");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Huizen");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			StartCoroutine(PlayerRecap2Coroutine());
		});
	}

	public IEnumerator PlayerRecap2Coroutine() {
		cw.AddPlayerBubble("En u weet vast ook nog wat we schreven over de huizen van de vogels.");

		yield return new WaitForSeconds(0.5f);
		
		// Boiler plate to include the story
		GameObject imageBubble = cw.AddArticleImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story2Image, new Rect(0, 0, mm.story2Image.width, mm.story2Image.height), new Vector2(0.5f, 0.5f));

		yield return new WaitForSeconds(0.5f);

		cw.AddArticleBubble(mm.story2Text);
		
		string resultText = "";
		switch (mm.story2FinalOpinion) {
		case Story2OpinionAnswer.GOOD:
			resultText = "het rustig bleef";
			break;
		case Story2OpinionAnswer.SAD:
			resultText = "er extra voor de mensen werd gezorgd";
			break;
		case Story2OpinionAnswer.WRONG:
			resultText = "er rellen uitbraken";
			break;
		}

		yield return new WaitForSeconds(0.5f);
		
		cw.AddPlayerBubble("Dat zorgde ervoor dat " + resultText + ".");
		
		StartCoroutine(PlayerRecapClose());
	}

	public IEnumerator PlayerRecapClose() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Dat weet ik nog wel, ja!");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Niet te stoppen");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Verslaggevers zijn niet te stoppen. Ze zullen altijd schrijven over wat er gebeurt. Daar moet de politie rekening mee houden.");
			
			StartCoroutine(ShowStatement3());
		});
	}

	public IEnumerator ShowStatement3() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Maar…");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton ("Wat nu gebeurt");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			if (mm.story3Attribution == Story3Attribution.FRANK) {
				cw.AddPlayerBubble("Wat gebeurt er bijvoorbeeld als de arrestatie van de dichter in het nieuws komt?");
			} else if (mm.story3Attribution == Story3Attribution.KATJA) {
				cw.AddPlayerBubble("Wat gebeurt er bijvoorbeeld als de arrestatie van Katja in het nieuws komt?");
			} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
				cw.AddPlayerBubble("Wat gebeurt er bijvoorbeeld als ik opschrijf wat u net zei over hard optreden?");
			}

			StartCoroutine(ShowQuestion());
		});
	}

	public IEnumerator ShowQuestion() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Maar… we kunnen iedereen toch niet zomaar zijn gang laten gaan?");

		yield return new WaitForSeconds(0.5f);

		GameObject button1 = cw.AddButton ("Dat kan best");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Dat kan best! Orde is belangrijk. Maar niet zo belangrijk dat mensen hun vrijheid ervoor moeten opgeven.");
			
			StartCoroutine(ShowResponse());
		});

		GameObject button2 = cw.AddButton ("Niet ten koste van alles");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Niet ten koste van alles! Hier in het museum zie je wat er gebeurt als mensen hun vrijheid moeten opgeven. Dan maar iets minder orde!");
			
			StartCoroutine(ShowResponse());
		});

		GameObject button3 = cw.AddButton ("U bent gewoon bang");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("U bent gewoon bang dat het een rommeltje wordt. Maar zo'n vaart zal het niet lopen.");
			
			StartCoroutine(ShowResponse());
		});
	}

	public IEnumerator ShowResponse() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik weet het niet… Ik volg ook maar gewoon de regels.");

		yield return new WaitForSeconds(0.5f);

		GameObject button = null;
		if (mm.officer3Response == Officer3Response.DISAGREE) {
			button = cw.AddButton ("Dan niet");
		} else if (mm.officer3Response == Officer3Response.UNDERSTAND) {
			button = cw.AddButton ("Begrijp ik");
		}

		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			if (mm.officer3Response == Officer3Response.DISAGREE) {
				cw.AddPlayerBubble("Nou, dan niet.");
			} else if (mm.officer3Response == Officer3Response.UNDERSTAND) {
				cw.AddPlayerBubble("Dat begrijp ik. U doet uw best.");
			}
			
			StartCoroutine(ShowConclusion());
		});
	}

	public IEnumerator ShowConclusion() {
		cw.EnableBack();
		chat.SetActive(false);

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/portrait agent wide");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;

		yield return new WaitForSeconds(0.5f);

		if (mm.officer3Response == Officer3Response.DISAGREE) {
			cw.AddNPCBubble("Wat brutaal. U gaat duidelijk met de verkeerde mensen om.");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Ik help u toch nog even, dat is mijn plicht als agent. U moet terug naar het geweer.");
		} else if (mm.officer3Response == Officer3Response.UNDERSTAND) {
			cw.AddNPCBubble("Dat waardeer ik. De vogelproblematiek is pittig.");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Gaat u terug naar het geweer als u klaar bent in het museum?");
		}

		yield return new WaitForSeconds(0.5f);


		GameObject button = cw.AddButton ("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Oké, tot ziens.");
			
			Invoke ("ShowClose", 0.5f);
		});
	}
	
	public void ShowClose() {
		mm.callBusy = false;

		Goal g = default(Goal);
		g.goalText = "Ga terug naar het geweer";
		g.overlayText = "Ga terug naar het geweer op de eerste verdieping.";
		g.locationSprite = "geweer";
		mm.goal = g;
		
		GameObject.Destroy(chat);
		GameObject.Destroy(this);
	}
}
