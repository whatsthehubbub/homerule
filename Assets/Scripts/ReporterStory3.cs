using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory3 : MonoBehaviour {

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
		
		cw.AddNPCBubble("Hoi!");
		cw.AddNPCBubble("Ben je al bij die lepeltjes en sieraden?");
		
		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, ik ben er.");
			
			Invoke ("ShowSituation", 0.5f);
		});
		
	}

	public void ShowSituation() {
		cw.AddNPCBubble("Super. Er is wel iets aan de hand hier. Moet je zien!");

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

		cw.AddNPCBubble("Ik zit hier met Frank, hij heeft me alles verteld. Hij heeft bewijs dat de vrije vogels worden tegengewerkt.");

		cw.AddNPCBubble("Ik wil erover schrijven, maar de politie staat al voor de deur!");

		cw.AddNPCBubble("Kun je me helpen?");

		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, ik help je. Dat weet je nu toch wel!");
			
			Invoke ("ShowTakePicture", 0.5f);
		});
	}

	public void ShowTakePicture() {
		cw.AddNPCBubble("Kun je een foto maken van de lepeltjes en sieraden met koningin Wilhelmina erop?");

		Invoke ("ShowPictureResponse", 0.5f);
	}

	public void ShowPictureResponse() {
		cw.AddNPCBubble("Goeie foto!");

		Invoke ("ShowFactQuestion", 0.5f);
	}

	public void ShowFactQuestion() {
		cw.AddNPCBubble("In de oorlog was het verboden om te laten zien dat je het koningshuis een warm hart toedroeg.");

		cw.AddNPCBubble("Waarom denk jij dat dit was? Je mag overleggen!");

		cw.AddNPCBubble("Was dit omdat de Duitsers (1) tegen koningin Wilhelmina waren, (2) wilden dat je voor Hitler was, of (3) niet wilden dat je je organiseerde.");

		GameObject button1 = cw.AddButton("Tegen de koningin");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk dat het was omdat de Duitsers tegen koningin Wilhelmina waren.");

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
			cw.AddNPCBubble("Maar de Duitsers wilden ook niet dat mensen zich organiseerden op hun politieke overtuiging. Dan konden ze wel eens de controle verliezen.");

		} else if (mm.story3Fact == Story3FactAnswer.HITLER) {
			cw.AddNPCBubble("Klopt! Toen de Duitsers Nederland hadden bezet, waren zij de baas.");
			cw.AddNPCBubble("Maar de Duitsers wilden ook niet dat mensen zich organiseerden op hun politieke overtuiging. Dan konden ze wel eens de controle verliezen.");

		} else if (mm.story3Fact == Story3FactAnswer.ORGANIZATION) {
			cw.AddNPCBubble("Klopt! De Duitsers wilden niet dat mensen zich organiseerden op hun politieke overtuiging. Dan konden ze wel eens de controle verliezen.");
		}

		GameObject button = cw.AddButton("Aha");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Aha, dus zo zit het!");
			
			Invoke ("ShowMoreFacts", 0.5f);
		});
	}

	public void ShowMoreFacts() {
		cw.AddNPCBubble("Dit was niet het enige. De vlag en wimpel mochten niet meer worden opgehangen. Afbeeldingen van leden van het koningshuis werden verboden.");

		cw.AddNPCBubble("We verloren de vrijheid om onze politieke overtuiging te uiten. Je kon niet meer zijn wie je wilde zijn. Veel mensen gingen dit daarom stiekem doen.");

		cw.AddNPCBubble("Wat een verhaal, hè? Het lijkt op wat er bij mij gebeurt. De vogels willen vrij zijn, maar mogen dat niet. En  de politie liegt er ook nog over!");

		cw.AddNPCBubble("Dus ik ga je foto delen.");

		cw.AddNPCBubble("We moeten de mensen echt op de hoogte brengen. Maar hoe gaan we dat doen?");

		string source = "";
		if (mm.reporter2Source == Reporter2Source.FRANK) {
			source = "van Frank had";
		} else if (mm.reporter2Source == Reporter2Source.SELF) {
			source = "zelf had ontdekt";
		} else if (mm.reporter2Source == Reporter2Source.ANONYMOUS) {
			source = "van een anonieme bron had";
		}

		cw.AddNPCBubble("Je zei dat je het nieuws over de vrije vogels " + source + ". Wie moet zijn naam bij het bericht zetten?");

		GameObject button1 = cw.AddButton("Frank");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Frank kwam met het nieuws. En hij wil graag vrij zijn. Zijn naam moet erbij.");

			mm.story3Attribution = Story3Attribution.FRANK;

			Invoke ("ShowArticle", 0.5f);
		});

		GameObject button2 = cw.AddButton("Katja");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Jij bent hier de reporter. Jij wilt vertellen wat er gebeurt. En jij hebt gecontroleerd of het bericht klopt, dus jouw naam moet erbij.");

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

		cw.AddNPCBubble("Net als nu mochten de mensen in Nederland niet uitkomen voor hun politieke voorkeur, vrije vogel of koningsgezind!");

		cw.AddNPCBubble("Was getekend…");

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Frank de kunstenaar");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Katja de reporter");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Anonieme bron");
		}

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

		cw.AddNPCBubble("Wauw. Moet je zien wat er gebeurt!");

		GameObject button = cw.AddButton("Laat zien");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			Invoke ("ShowResultImage", 0.5f);
		});
	}

	public void ShowResultImage() {
		// TODO we're still in a video call so there's no need to instantiate a new one
		//		cw.EnableBack();
//		chat.SetActive(false);

//		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
//		chat.name = "VideoCall";

		// Show the correct sprite (Journalist)
//		GameObject displayImage = GameObject.Find ("DisplayImage");
//		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
//		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("O nee, Frank wordt opgepakt. We hadden hem moeten beschermen!");

			Invoke ("ShowResultConclusion", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddNPCBubble("Help!");

			Invoke ("ShowResultConclusion", 0.5f);
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddNPCBubble("Die agent is goed chagrijnig. Maar we zijn van hem af! En de mensen weten dat de vogels niet vrij mogen zijn.");

			Invoke ("ShowResultConclusion", 0.5f);
		}
	}

	public void ShowResultConclusion() {
		// TODO swap out the correct image

		string spriteString = "";

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			spriteString = "S3 vogel gearresteerd";

			cw.AddNPCBubble("Nou ja, we hebben verteld wat er gebeurt. We hebben ook gezegd wat we ervan vinden. Daardoor veranderen er dingen.");

			cw.AddNPCBubble("Kijk nog even rond als je wilt, er is van alles te zien in het museum. Hopelijk komt het goed met Frank en de vrije vogels.");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			spriteString = "S3 katja gearresteerd";

			cw.AddNPCBubble("Katja heeft verteld wat er gebeurt, dat is goed. De mensen weten dat wij vogels niet vrij mogen zijn. Maar nu is ze er zelf de dupe van geworden!");
			cw.AddNPCBubble("Kijk rustig rond als je wilt, er is nog van alles te zien in het museum. Hopelijk komt het goed met Katja.");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			spriteString = "S3 agent dissed";

			cw.AddNPCBubble("Die agent is goed chagrijnig. Maar we zijn van hem af! En de mensen weten dat de vogels niet vrij mogen zijn.");
			cw.AddNPCBubble("Dit is geweldig. We hebben verteld wat er gebeurt. We hebben ook gezegd wat we ervan vinden. Daardoor veranderen er dingen!");
			cw.AddNPCBubble("Kijk rustig rond als je wilt, er is nog van alles te zien in het museum.");
		}

		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/" + spriteString);
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;

		GameObject button = cw.AddButton("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("OK.");
			
			Invoke ("Close", 0.5f);
		});
	}
	
	public void Close() {
		mm.callBusy = false;
		
		mm.story3Done = true;

		mm.targetText = "";
		mm.UpdateTargetText();
		
		mm.storyQueue.Enqueue("OFFICERRESPONSE3");
		mm.storyQueue.Enqueue("ARTISTRESPONSE2");
		
		Destroy(chat);
		GameObject.Destroy(this);
	}
}
