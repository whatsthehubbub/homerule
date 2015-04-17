using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum StoryOpinionAnswer {
	SAD,
	GOOD,
	WRONG
}

public enum StoryFactAnswer {
	FIGHTING,
	HELPING,
	STEALING
}

public class ObserveerUI : MonoBehaviour {

	public GameObject cameraButton;
	public GameObject progressBar;
	public float progressSpeed = 0.1f;
	public AudioClip shutterSound;

	public GameObject chat;
	public ChatWindow cw;

	public StoryOpinionAnswer playerOpinion;
	public StoryFactAnswer playerFact;

	void Awake()
	{
		progressBar.SetActive(false);
	}

	public void OnClickCamera()
	{
		StartCoroutine("MakePhoto");

		StartStory();
	}

	IEnumerator MakePhoto()
	{	//foto maken
		GetComponent<AudioSource>().PlayOneShot(shutterSound);
		progressBar.SetActive(true);
		progressBar.GetComponent<Image>().fillAmount = 100;
		for(float f = 1; f > 0; f-=0.02f)
		{
			progressBar.GetComponent<Image>().fillAmount = f;
			yield return new WaitForSeconds(progressSpeed);
		}
		progressBar.SetActive(false);

		//haal observatie knop weg
		this.gameObject.SetActive(false);

		//observatie +1
//		GameObject main = GameObject.Find("Main");
//		if (main != null) {
//			main.SendMessage("StartStory");
//		}
	}

	public void StartStory() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";
		
		cw = chat.GetComponent<ChatWindow>();

		cw.AddNPCBubble("Hoi!");
		cw.AddNPCBubble("Er is iets aan de hand. Moet je zien!");

		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");

			Invoke ("ShowReporterResponse1", 0.5f);
		});

	}

	public void ShowReporterResponse1() {
		cw.AddNPCBubble("Mensen moeten hun huis uit. Ze zijn in de buurt aan het bouwen. Daardoor kunnen huizen instorten. Ze zeggen dat het gevaarlijk is. Maar niet iedereen wil weg.");
		cw.AddNPCBubble("Kun je mij helpen hier over te schrijven?");

		GameObject ja = cw.AddButton("Ja");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja ik help je");
			
			Invoke ("ShowOpinion1", 0.5f);
		});
	}

	public void ShowOpinion1() {
		cw.AddNPCBubble("Ze willen dat mensen verhuizen omdat het gevaarlijk is waar ze nu wonen.");
		cw.AddNPCBubble("Wat vind je daar van?");

		GameObject zielig = cw.AddButton("Zielig");
		zielig.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zielig. Waar moeten de mensen heen?");

			playerOpinion = StoryOpinionAnswer.SAD;
			
			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});

		GameObject goed = cw.AddButton("Goed");
		goed.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Goed. Als er gevaar is dan moeten ze je daarvoor beschermen.");

			playerOpinion = StoryOpinionAnswer.GOOD;

			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});

		GameObject verkeerd = cw.AddButton("Verkeerd");
		verkeerd.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Verkeerd. Mensen mogen zelf weten of ze weg gaan.");

			playerOpinion = StoryOpinionAnswer.WRONG;

			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});

	}

	public void ShowReporterOpinionResponse1() {
		cw.AddNPCBubble("Helemaal mee eens. Laten we de mensen hier van overtuigen.");

		GameObject ok = cw.AddButton("Ok");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ok, maar hoe?");
			
			Invoke ("ShowFindObject", 0.5f);
		});
	}

	public void ShowFindObject() {
		cw.AddNPCBubble("Er is daar een bord met “verboden Arnhem te betreden”. Kun je daar een foto van maken?");

		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			Invoke ("ShowFindObjectResponse", 0.5f);
		});
	}

	public void ShowFindObjectResponse() {
		cw.AddNPCBubble("Goeie foto!");

		Invoke ("ShowObjectFacts", 0.5f);
	}

	public void ShowObjectFacts() {
		cw.AddNPCBubble("De mensen in Arnhem moesten ook hun huis uit. Dat wilden de Duitsers.");
		cw.AddNPCBubble("Waarom denk jij dat zij dat wilden?");

		cw.AddNPCBubble("Kies uit (1) omdat er werd gevochten, (2) zodat mensen de geallieerden niet zouden helpen, of (3) om hun spullen te stelen.");

		GameObject answer1 = cw.AddButton("Antwoord 1");
		answer1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Omdat er werd gevochten en dat is gevaarlijk.");

			playerFact = StoryFactAnswer.FIGHTING;
			
			Invoke ("ShowFactResponse", 0.5f);
		});

		GameObject answer2 = cw.AddButton("Antwoord 2");
		answer2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zodat mensen de geallieerden niet zouden helpen.");

			playerFact = StoryFactAnswer.HELPING;

			Invoke ("ShowFactResponse", 0.5f);
		});

		GameObject answer3 = cw.AddButton("Antwoord 3");
		answer3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zodat de Duitsers hun spullen konden te stelen.");

			playerFact = StoryFactAnswer.STEALING;

			Invoke ("ShowFactResponse", 0.5f);
		});
	}

	public void ShowFactResponse() {
		if (playerFact == StoryFactAnswer.FIGHTING) {
			cw.AddNPCBubble("Klopt! De Duitsers waren ook bang dat de mensen de geallieerden zouden helpen. Er werden ook spullen gestolen door de Duitsers en door burgers.");
		} else if (playerFact == StoryFactAnswer.HELPING) {
			cw.AddNPCBubble("Klopt! Er werd ook nog gevochten en dat is gevaarlijk. Er werden ook spullen gestolen door de Duitsers en door burgers.");
		} else if (playerFact == StoryFactAnswer.STEALING) {
			cw.AddNPCBubble("Dat was niet de reden maar het gebeurde wel. Er werden spullen gestolen door de Duitsers en door burgers.");

			cw.AddNPCBubble("De mensen moesten weg omdat er gevochten werd en dat is gevaarlijk. De Duitsers waren ook bang dat de mensen de geallieerden zouden helpen.");
		}

		Invoke ("ShowFactResponseClose", 0.5f);
	}

	public void ShowFactResponseClose() {
		cw.AddNPCBubble("Mensen moesten binnen twee dagen weg. Daarna was het verboden om nog in de stad te zijn. Als je toch bleef kon je worden neergeschoten.");

		cw.AddNPCBubble("Wat een verhaal he? Het lijkt op wat er nu hier gebeurt. Dus ik ga je foto delen.");

		Invoke ("ShowArgument", 0.5f);
	}

	public void ShowArgument() {
		string opinion = "";

		if (playerOpinion == StoryOpinionAnswer.SAD) opinion = "zielig";
		else if (playerOpinion == StoryOpinionAnswer.GOOD) opinion = "goed";
		else if (playerOpinion == StoryOpinionAnswer.WRONG) opinion = "verkeerd";

		cw.AddNPCBubble("Maar wat zal ik er bij zeggen? Eerder vond je het " + opinion + ". Vind je dat nog steeds?");

		GameObject opinion1 = cw.AddButton("Zielig");
		opinion1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zielig want je moet al je spullen en vrienden achterlaten en je weet niet waar je heen gaat.");

			playerOpinion = StoryOpinionAnswer.SAD;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});

		GameObject opinion2 = cw.AddButton("Goed");
		opinion1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is goed want als je gevaar loopt dan moeten ze je daarvoor beschermen.");
			
			playerOpinion = StoryOpinionAnswer.GOOD;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});

		GameObject opinion3 = cw.AddButton("Verkeerd");
		opinion3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verkeerd want mensen moeten zelf kunnen kiezen of ze weg gaan of niet.");
			
			playerOpinion = StoryOpinionAnswer.WRONG;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});
	}

	public void ShowArgumentResponse() {
		cw.AddNPCBubble("OK! Ik heb er dit bericht van gemaakt:");

		cw.AddNPCBubble("Net als nu moesten de mensen in Arnhem van de Duitsers ook uit hun huis. Dat was omdat er gevochten werd en de Duitsers bang waren dat de mensen de geallieerden zouden helpen. Daarna werden er spullen gestolen door de Duitsers en door de mensen zelf.");

		string argument = "";
		if (playerOpinion == StoryOpinionAnswer.SAD) argument = "zielig want je moet al je spullen en vrienden achterlaten en je weet niet waar je heen gaat. Ze moeten dus goed voor de mensen zorgen!";
		else if (playerOpinion == StoryOpinionAnswer.GOOD) argument = "goed want als je gevaar loopt dan moeten ze je daarvoor beschermen. De mensen moeten dus gewoon doen wat de agenten zeggen.";
		else if (playerOpinion == StoryOpinionAnswer.WRONG) argument = "verkeerd want je moet zelf kunnen kiezen of je weg gaat of niet. De mensen moeten dus blijven als ze dat willen. Ook als dat niet mag van de agenten.";

		cw.AddNPCBubble("Mensen uit hun huis zetten vanwege gevaar is " + argument);

		Invoke ("ShowSend", 0.5f);
	}

	public void ShowSend() {
		cw.AddNPCBubble("Ik vind het een goed bericht. Jij ook?");

		GameObject send = cw.AddButton("Verzenden");
		send.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, verzenden maar!");
			
			Invoke ("ShowResult", 0.5f);
		});
	}

	public void ShowResult() {
		cw.AddNPCBubble("Ik heb je bericht verzonden…");
		cw.AddNPCBubble("Wow. Moet je zien hoe de mensen reageren!");

		GameObject show = cw.AddButton("Laat zien");
		show.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			Invoke ("ShowResult", 0.5f);
		});
	}

	public void ShowResultResponse() {

		if (playerOpinion == StoryOpinionAnswer.SAD) {
			cw.AddNPCBubble("Er wordt gelukkig goed gezorgd voor de mensen.");
		} else if (playerOpinion == StoryOpinionAnswer.GOOD) {
			cw.AddNPCBubble("Iedereen volgt netjes de regels. Maar niet iedereen is blij.");
		} else if (playerOpinion == StoryOpinionAnswer.WRONG) {
			cw.AddNPCBubble("Sommige mensen luisteren niet. Ze krijgen ruzie en er wordt gevochten! ");
		}

		Invoke ("ShowResultClose", 0.5f);
	}

	public void ShowResultClose() {
		cw.AddNPCBubble("We hebben verteld wat er gebeurt. We hebben ook gezegd wat we er van vinden. Daardoor veranderen er dingen. Goed gedaan!");

		cw.AddNPCBubble("Op andere plekken zijn ook nog dingen te zien. Ik bel je weer als er iets te doen is!");

		// TODO exit the chat
	}

}