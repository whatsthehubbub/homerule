using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using CameraShot;

public class ReporterStory : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	public MuseumManager mm;

	// Use this for initialization
	
	void OnEnable() {
		CameraShotEventListener.onImageLoad += OnImageLoad;
		CameraShotEventListener.onImageSaved += OnImageSaved;
		CameraShotEventListener.onCancel += OnCancel;
	}

	void Start () {
		StartStory ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartStory() {
		// Pause the change scene
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		mm.changeScene = false;
		
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";

		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
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
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite introSprite = Resources.Load<Sprite>("Sprites/situatie intro");
		displayImage.GetComponentInChildren<Image>().sprite = introSprite;

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
		GameObject.Destroy(chat);
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetNPCAvatar("katja");

		cw.AddNPCBubble("Ze willen dat mensen verhuizen omdat het gevaarlijk is waar ze nu wonen.");
		cw.AddNPCBubble("Wat vind je daar van?");
		
		GameObject zielig = cw.AddButton("Zielig");
		zielig.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zielig. Waar moeten de mensen heen?");
			
			mm.playerOpinion = StoryOpinionAnswer.SAD;
			
			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});
		
		GameObject goed = cw.AddButton("Goed");
		goed.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Goed. Als er gevaar is dan moeten ze je daarvoor beschermen.");
			
			mm.playerOpinion = StoryOpinionAnswer.GOOD;
			
			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});
		
		GameObject verkeerd = cw.AddButton("Verkeerd");
		verkeerd.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Verkeerd. Mensen mogen zelf weten of ze weg gaan.");
			
			mm.playerOpinion = StoryOpinionAnswer.WRONG;
			
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

			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				cw.AddPlayerImageBubble();
				IOSCameraShot.LaunchCameraForImageCapture();
			} else {
				Invoke ("ShowFindObjectResponse", 0.5f);
			}
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
			
			mm.playerFact = StoryFactAnswer.FIGHTING;
			
			Invoke ("ShowFactResponse", 0.5f);
		});
		
		GameObject answer2 = cw.AddButton("Antwoord 2");
		answer2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zodat mensen de geallieerden niet zouden helpen.");
			
			mm.playerFact = StoryFactAnswer.HELPING;
			
			Invoke ("ShowFactResponse", 0.5f);
		});
		
		GameObject answer3 = cw.AddButton("Antwoord 3");
		answer3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zodat de Duitsers hun spullen konden te stelen.");
			
			mm.playerFact = StoryFactAnswer.STEALING;
			
			Invoke ("ShowFactResponse", 0.5f);
		});
	}
	
	public void ShowFactResponse() {
		if (mm.playerFact == StoryFactAnswer.FIGHTING) {
			cw.AddNPCBubble("Klopt! De Duitsers waren ook bang dat de mensen de geallieerden zouden helpen. Er werden ook spullen gestolen door de Duitsers en door burgers.");
		} else if (mm.playerFact == StoryFactAnswer.HELPING) {
			cw.AddNPCBubble("Klopt! Er werd ook nog gevochten en dat is gevaarlijk. Er werden ook spullen gestolen door de Duitsers en door burgers.");
		} else if (mm.playerFact == StoryFactAnswer.STEALING) {
			cw.AddNPCBubble("Dat was niet de reden maar het gebeurde wel. Er werden spullen gestolen door de Duitsers en door burgers.");
			
			cw.AddNPCBubble("De mensen moesten weg omdat er gevochten werd en dat is gevaarlijk. De Duitsers waren ook bang dat de mensen de geallieerden zouden helpen.");
		}

		GameObject button = cw.AddButton("Oh");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oh, zit het zo!");

			Invoke ("ShowFactResponseClose", 0.5f);
		});
	}
	
	public void ShowFactResponseClose() {
		cw.AddNPCBubble("Mensen moesten binnen twee dagen weg. Daarna was het verboden om nog in de stad te zijn. Als je toch bleef kon je worden neergeschoten.");
		
		cw.AddNPCBubble("Wat een verhaal he? Het lijkt op wat er nu hier gebeurt. Dus ik ga je foto delen.");
		
		Invoke ("ShowArgument", 0.5f);
	}
	
	public void ShowArgument() {
		string opinion = "";
		
		if (mm.playerOpinion == StoryOpinionAnswer.SAD) opinion = "zielig";
		else if (mm.playerOpinion == StoryOpinionAnswer.GOOD) opinion = "goed";
		else if (mm.playerOpinion == StoryOpinionAnswer.WRONG) opinion = "verkeerd";
		
		cw.AddNPCBubble("Maar wat zal ik er bij zeggen? Eerder vond je het " + opinion + ". Vind je dat nog steeds?");
		
		GameObject opinion1 = cw.AddButton("Zielig");
		opinion1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zielig want je moet al je spullen en vrienden achterlaten en je weet niet waar je heen gaat.");
			
			mm.playerOpinion = StoryOpinionAnswer.SAD;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});
		
		GameObject opinion2 = cw.AddButton("Goed");
		opinion2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is goed want als je gevaar loopt dan moeten ze je daarvoor beschermen.");
			
			mm.playerOpinion = StoryOpinionAnswer.GOOD;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});
		
		GameObject opinion3 = cw.AddButton("Verkeerd");
		opinion3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verkeerd want mensen moeten zelf kunnen kiezen of ze weg gaan of niet.");
			
			mm.playerOpinion = StoryOpinionAnswer.WRONG;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});
	}
	
	public void ShowArgumentResponse() {
		cw.AddNPCBubble("OK! Ik heb er dit bericht van gemaakt:");
		
		cw.AddNPCBubble("Net als nu moesten de mensen in Arnhem van de Duitsers ook uit hun huis. Dat was omdat er gevochten werd en de Duitsers bang waren dat de mensen de geallieerden zouden helpen. Daarna werden er spullen gestolen door de Duitsers en door de mensen zelf.");
		
		string argument = "";
		if (mm.playerOpinion == StoryOpinionAnswer.SAD) argument = "zielig want je moet al je spullen en vrienden achterlaten en je weet niet waar je heen gaat. Ze moeten dus goed voor de mensen zorgen!";
		else if (mm.playerOpinion == StoryOpinionAnswer.GOOD) argument = "goed want als je gevaar loopt dan moeten ze je daarvoor beschermen. De mensen moeten dus gewoon doen wat de agenten zeggen.";
		else if (mm.playerOpinion == StoryOpinionAnswer.WRONG) argument = "verkeerd want je moet zelf kunnen kiezen of je weg gaat of niet. De mensen moeten dus blijven als ze dat willen. Ook als dat niet mag van de agenten.";
		
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
			
			Invoke ("ShowResultResponse", 0.5f);
		});
	}
	
	public void ShowResultResponse() {
		GameObject.Destroy(chat);
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();

		// Show the correct sprite (Journalist)
		string spriteName = "";
		if (mm.playerOpinion == StoryOpinionAnswer.GOOD) { spriteName = "situatie uitkomst goed"; }
		else if (mm.playerOpinion == StoryOpinionAnswer.SAD) { spriteName = "situatie uitkomst zielig"; }
		else if (mm.playerOpinion == StoryOpinionAnswer.WRONG) { spriteName = "situatie uitkomst slecht"; }
		Sprite showSprite = Resources.Load<Sprite>("Sprites/" + spriteName);;

		GameObject displayImage = GameObject.Find ("DisplayImage");
		displayImage.GetComponentInChildren<Image>().sprite = showSprite;
		
		
		if (mm.playerOpinion == StoryOpinionAnswer.SAD) {
			cw.AddNPCBubble("Er wordt gelukkig goed gezorgd voor de mensen.");
		} else if (mm.playerOpinion == StoryOpinionAnswer.GOOD) {
			cw.AddNPCBubble("Iedereen volgt netjes de regels. Maar niet iedereen is blij.");
		} else if (mm.playerOpinion == StoryOpinionAnswer.WRONG) {
			cw.AddNPCBubble("Sommige mensen luisteren niet. Ze krijgen ruzie en er wordt gevochten! ");
		}

		GameObject ok = cw.AddButton("Ok");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ok");
			
			Invoke ("ShowResultClose", 0.5f);
		});
	}
	
	public void ShowResultClose() {
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;

		cw.AddNPCBubble("We hebben verteld wat er gebeurt. We hebben ook gezegd wat we er van vinden. Daardoor veranderen er dingen!");
		
		cw.AddNPCBubble("Op andere plekken zijn ook nog dingen te zien. Ik bel je weer als er iets te doen is!");

		GameObject main = GameObject.Find("Main");
		MuseumManager mm = main.GetComponentInChildren<MuseumManager>();
		mm.changeScene = true;
		mm.storyCompleted = true;
		mm.showOfficerStoryResponse = true;

		// Update the current story to be no longer active
		Story s = mm.stories[mm.currentStory];
		s.active = false;
		mm.stories[mm.currentStory] = s;

	}

	/*
	 * Methods to handle taking an image using CamerShot
	 */	
	void OnImageLoad(string path, Texture2D tex) {
		Debug.Log ("Image Captured by camera saved at location : " + path);
	}
	
	void OnImageSaved(string path) {
		Debug.Log ("A photograph has been saved");
		StartCoroutine(displayTexture(path));
	}
	
	IEnumerator displayTexture(string path) {
		Debug.Log ("Display texture " + path);
		
		path = path.Replace (" ", "%20");
		
		WWW www = new WWW("file://" + path);
		Debug.Log ("www" + www.ToString());
		
		yield return www;
		
		if (www.isDone) {
			
			Texture2D text = new Texture2D(1024, 1024);
			Debug.Log ("text" + text.ToString());
			
			www.LoadImageIntoTexture(text);
			Debug.Log ("Text loaded" + text.ToString());

			GameObject imageObject = GameObject.Find ("PlayerRawImage");
			RawImage raw = imageObject.GetComponentInChildren<RawImage>();
			Debug.Log ("Got raw");

			raw.texture = text;

			Invoke ("ShowFindObjectResponse", 0.5f);
		}
	}

	void OnCancel() {
		Debug.Log ("Taking a picture has been cancelled.");
	}

	private Texture2D rotateTexture(Texture2D tex, float angle)
	{
		Debug.Log("rotating");
		Texture2D rotImage = new Texture2D(tex.width, tex.height);
		int  x,y;
		float x1, y1, x2,y2;
		
		int w = tex.width;
		int h = tex.height;
		float x0 = rot_x (angle, -w/2.0f, -h/2.0f) + w/2.0f;
		float y0 = rot_y (angle, -w/2.0f, -h/2.0f) + h/2.0f;
		
		float dx_x = rot_x (angle, 1.0f, 0.0f);
		float dx_y = rot_y (angle, 1.0f, 0.0f);
		float dy_x = rot_x (angle, 0.0f, 1.0f);
		float dy_y = rot_y (angle, 0.0f, 1.0f);
		
		
		x1 = x0;
		y1 = y0;
		
		for (x = 0; x < tex.width; x++) {
			x2 = x1;
			y2 = y1;
			for ( y = 0; y < tex.height; y++) {
				//rotImage.SetPixel (x1, y1, Color.clear);          
				
				x2 += dx_x;//rot_x(angle, x1, y1);
				y2 += dx_y;//rot_y(angle, x1, y1);
				rotImage.SetPixel ( (int)Mathf.Floor(x), (int)Mathf.Floor(y), getPixel(tex,x2, y2));
			}
			
			x1 += dy_x;
			y1 += dy_y;
			
		}
		
		rotImage.Apply();
		return rotImage;
	}
	
	private Color getPixel(Texture2D tex, float x, float y)
	{
		Color pix;
		int x1 = (int) Mathf.Floor(x);
		int y1 = (int) Mathf.Floor(y);
		
		if(x1 > tex.width || x1 < 0 ||
		   y1 > tex.height || y1 < 0) {
			pix = Color.clear;
		} else {
			pix = tex.GetPixel(x1,y1);
		}
		
		return pix;
	}
	
	private float rot_x (float angle, float x, float y) {
		float cos = Mathf.Cos(angle/180.0f*Mathf.PI);
		float sin = Mathf.Sin(angle/180.0f*Mathf.PI);
		return (x * cos + y * (-sin));
	}
	private float rot_y (float angle, float x, float y) {
		float cos = Mathf.Cos(angle/180.0f*Mathf.PI);
		float sin = Mathf.Sin(angle/180.0f*Mathf.PI);
		return (x * sin + y * cos);
	}
}
