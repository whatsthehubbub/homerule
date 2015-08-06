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

			StartStory ();
		});


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartStory() {
		// Pause the change scene
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/NewVideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddDivider();
		
		cw.AddNPCBubble("Hoi!");
		cw.AddNPCBubble("Er is weer iets aan de hand. Moet je zien!");
		
		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");
			
			Invoke ("ShowReporterResponse1", 0.5f);
		});
		
	}
	
	public void ShowReporterResponse1() {
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite introSprite = Resources.Load<Sprite>("Sprites/S2 intro");
		displayImage.GetComponentInChildren<Image>().sprite = introSprite;

		cw.AddNPCBubble("Mensen moeten hun huis uit, omdat er in de buurt gebouwd wordt. Maar niet iedereen wil weg.");
		cw.AddNPCBubble("Help je me hierover te schrijven?");
		
		GameObject ja = cw.AddButton("Ja");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja, ik help je.");
			
			Invoke ("ShowOpinion1", 0.5f);
		});
	}
	
	public void ShowOpinion1() {
		GameObject.Destroy(chat);

		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);

		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		cw.AddNPCBubble("Ze willen dat mensen tijdelijk verhuizen, omdat het gevaarlijk is waar ze wonen.");
		cw.AddNPCBubble("Wat vind je daarvan? Overleg gerust!");
		
		GameObject zielig = cw.AddButton("Zielig");
		zielig.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zielig. Waar moeten de mensen heen?");
			
			mm.story2Opinion = Story2OpinionAnswer.SAD;
			
			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});
		
		GameObject goed = cw.AddButton("Goed");
		goed.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Goed. Als er gevaar is, moeten ze de mensen beschermen.");
			
			mm.story2Opinion = Story2OpinionAnswer.GOOD;
			
			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});
		
		GameObject verkeerd = cw.AddButton("Verkeerd");
		verkeerd.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Verkeerd. Mensen mogen zelf bepalen of ze weggaan.");
			
			mm.story2Opinion = Story2OpinionAnswer.WRONG;
			
			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});
		
	}
	
	public void ShowReporterOpinionResponse1() {
		cw.AddNPCBubble("Mee eens. Dit moeten de mensen weten.");
		
		GameObject ok = cw.AddButton("Hoe?");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké. Maar hoe gaan we het vertellen?");
			
			Invoke ("ShowFindObject", 0.5f);
		});
	}
	
	public void ShowFindObject() {
		cw.AddNPCBubble("Ik weet iets. Maak jij een foto van het bord met “verboden Arnhem te betreden”?");
		
		GameObject camera = cw.AddButton("Camera starten");
		camera.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				GameObject imageBubble = cw.AddPlayerImageBubble();
				imageBubble.name = "PlayerImage2";

				NativeToolkit.TakeCameraShot();
			} else {
				// Create a blank texture
				mm.story2Image = new Texture2D(2, 2, TextureFormat.ARGB32, false);

				Invoke ("ShowFindObjectResponse", 0.5f);
			}
		});
	}

	/*
	 * Methods to handle taking an image using CamerShot
	 */
	void CameraShotComplete(Texture2D img, string path)
	{
		mm.story2Image = img;
		
		GameObject bubble = GameObject.Find ("PlayerImage2");
		GameObject bubbleImage = bubble.transform.Find ("BubbleImage").gameObject;
		Image im = bubbleImage.GetComponentInChildren<Image>();
		
		im.sprite = Sprite.Create (mm.story2Image, new Rect(0, 0, mm.story2Image.width, mm.story2Image.height), new Vector2(0.5f, 0.5f));
		
		Invoke ("ShowFindObjectResponse", 0.5f);
	}
	
	public void ShowFindObjectResponse() {
		cw.AddNPCBubble("Goeie foto!");

		GameObject button = cw.AddButton("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké.");
		
			Invoke ("ShowObjectFacts", 0.5f);
		});
	}
	
	public void ShowObjectFacts() {
		cw.AddNPCBubble("De inwoners van Arnhem moesten ook hun huis uit.");
		cw.AddNPCBubble("Waarom denk jij dat de Duitse bezetter dat wilde? Als je samen speelt, kun je overleggen.");
		
		cw.AddNPCBubble("Was dat (1) omdat het er gevaarlijk was, (2) zodat inwoners de geallieerden niet konden helpen, of (3) zodat de Duitsers hun spullen konden stelen?");
		
		GameObject answer1 = cw.AddButton("Gevaarlijk");
		answer1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk omdat het er gevaarlijk was. Er werd gevochten en gebombardeerd.");
			
			mm.story2Fact = Story2FactAnswer.FIGHTING;
			
			Invoke ("ShowFactResponse", 0.5f);
		});
		
		GameObject answer2 = cw.AddButton("Geallieerden helpen");
		answer2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk zodat inwoners de geallieerden niet konden helpen.");
			
			mm.story2Fact = Story2FactAnswer.HELPING;
			
			Invoke ("ShowFactResponse", 0.5f);
		});
		
		GameObject answer3 = cw.AddButton("Spullen stelen");
		answer3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ik denk zodat de Duitsers hun spullen konden stelen.");
			
			mm.story2Fact = Story2FactAnswer.STEALING;
			
			Invoke ("ShowFactResponse", 0.5f);
		});
	}
	
	public void ShowFactResponse() {
		if (mm.story2Fact == Story2FactAnswer.FIGHTING) {
			cw.AddNPCBubble("Klopt! Maar de Duitse bezetter was ook bang dat inwoners de geallieerden zouden helpen.");
		} else if (mm.story2Fact == Story2FactAnswer.HELPING) {
			cw.AddNPCBubble("Klopt! Daarnaast was het er gevaarlijk. Er werd gevochten en gebombardeerd.");
		} else if (mm.story2Fact == Story2FactAnswer.STEALING) {
			cw.AddNPCBubble("Dat was niet de reden, maar het gebeurde wel. Er werden spullen gestolen door Duitsers én door burgers in nood.");
			
			cw.AddNPCBubble("Maar de mensen moesten weg omdat het er gevaarlijk was. Er werd gevochten en gebombardeerd.");
		}

		GameObject button = cw.AddButton("Aha");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Aha, dus zo zit het.");

			Invoke ("ShowFactResponseClose", 0.5f);
		});
	}
	
	public void ShowFactResponseClose() {
		cw.AddNPCBubble("Je moest binnen twee dagen weg. Daarna was Arnhem verboden terrein.");
		
		cw.AddNPCBubble("Ik ga je foto delen. Want dit lijkt op wat er bij mij gebeurt.");
		
		Invoke ("ShowArgument", 0.5f);
	}
	
	public void ShowArgument() {
		string opinion = "";
		
		if (mm.story2Opinion == Story2OpinionAnswer.SAD) {
			opinion = "zielig";
		} else if (mm.story2Opinion == Story2OpinionAnswer.GOOD) {
			opinion = "goed";
		} else if (mm.story2Opinion == Story2OpinionAnswer.WRONG) {
			opinion = "verkeerd";
		}
		
		cw.AddNPCBubble("Wat zal ik schrijven? Net vond je het " + opinion + ". Vind je dat nog steeds?");
		
		GameObject opinion1 = cw.AddButton("Zielig");
		opinion1.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is zielig. Je moet alles achterlaten en je weet niet waar je terechtkomt.");
			
			mm.story2FinalOpinion = Story2OpinionAnswer.SAD;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});
		
		GameObject opinion2 = cw.AddButton("Goed");
		opinion2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is goed. Als je gevaar loopt, moeten ze je daartegen beschermen.");
			
			mm.story2FinalOpinion = Story2OpinionAnswer.GOOD;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});
		
		GameObject opinion3 = cw.AddButton("Verkeerd");
		opinion3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Het is verkeerd. Je moet zelf kunnen bepalen of je weggaat of niet.");
			
			mm.story2FinalOpinion = Story2OpinionAnswer.WRONG;
			
			Invoke ("ShowArgumentResponse", 0.5f);
		});
	}
	
	public void ShowArgumentResponse() {
		cw.AddNPCBubble("Oké! Dit is mijn bericht:");

		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

		// Display the story image
		GameObject imageBubble = cw.AddNPCImageBubble();
		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		GameObject imageObject = imageBubble.transform.Find ("BubbleImage").gameObject;
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

		string storyText = "De inwoners van Arnhem moesten hun huis uit vanwege gevaar, net als nu." +
			"\n" + "Dat is " + argument;
		mm.story2Text = storyText;

		GameObject storyFactBubble = cw.AddNPCBubble(storyText);
		storyFactBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyFactBubble.GetComponentInChildren<Text>().color = Color.black;
		
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
		cw.AddNPCBubble("Wauw. Moet je zien wat er gebeurt.");
		
		GameObject show = cw.AddButton("Laat zien");
		show.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Laat zien!");
			
			Invoke ("ShowResultResponse", 0.5f);
		});
	}
	
	public void ShowResultResponse() {
		cw.EnableBack();
		chat.SetActive(false);

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/NewVideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		// Show the correct sprite (Journalist)
		string spriteName = "";
		if (mm.story2FinalOpinion == Story2OpinionAnswer.GOOD) { 
			spriteName = "S2 goed";
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.SAD) {
			spriteName = "S2 zielig";
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.WRONG) {
			spriteName = "S2 slecht";
		}
		Sprite showSprite = Resources.Load<Sprite>("Sprites/" + spriteName);;

		GameObject displayImage = GameObject.Find ("DisplayImage");
		displayImage.GetComponentInChildren<Image>().sprite = showSprite;
		
		
		if (mm.story2FinalOpinion == Story2OpinionAnswer.SAD) {
			cw.AddNPCBubble("Gelukkig wordt er goed voor de mensen gezorgd.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.GOOD) {
			cw.AddNPCBubble("De mensen houden zich aan de regels. Maar niet iedereen is blij.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.WRONG) {
			cw.AddNPCBubble("Sommige mensen luisteren niet. Ze krijgen ruzie en er wordt gevochten!");
		}

		GameObject ok = cw.AddButton("Oké");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké.");
			
			Invoke ("ShowResultClose", 0.5f);
		});
	}
	
	public void ShowResultClose() {
		// Switch back to Katja from the image
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;

		cw.AddNPCBubble("Heftig! Door op te schrijven wat er gebeurt, veranderen er dingen.");
		
		cw.AddNPCBubble("Ik bel als ik je nodig heb. Kijk rustig rond in het museum.");

		GameObject ok = cw.AddButton("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");

			mm.callBusy = false;

			mm.story2Done = true;

			mm.targetText = "Verken het museum";
			mm.UpdateTargetText();

			mm.storyQueue.Enqueue("OFFICERRESPONSE2");
			mm.storyQueue.Enqueue("ARTISTRESPONSE1");
			mm.storyQueue.Enqueue("REPORTERRESPONSE2");


			Destroy(chat);
			GameObject.Destroy(this);
		});
	}

// TODO remove this old camerashot code at some point

//	IEnumerator displayTexture(string path) {
//		Debug.Log ("Display texture " + path);
//		
//		path = path.Replace (" ", "%20");
//		
//		WWW www = new WWW("file://" + path);
//		Debug.Log ("www" + www.ToString());
//		
//		yield return www;
//		
//		if (www.isDone) {
//			
//			Texture2D text = new Texture2D(1024, 1024);
//			Debug.Log ("text" + text.ToString());
//			
//			www.LoadImageIntoTexture(text);
//			Debug.Log ("Text loaded" + text.ToString() + " " + text.width + " " + text.height);
//
//			TextureScale.Point (text, 816, 612);
//
//			text = rotateTexture(text, 180.0f);
//
//			GameObject imageObject = GameObject.Find ("PlayerRawImage");
//			RawImage raw = imageObject.GetComponentInChildren<RawImage>();
//			Debug.Log ("Got raw");
//
//			raw.texture = text;
//			mm.storyImage = text;
//
//			Invoke ("ShowFindObjectResponse", 0.5f);
//		}
//	}

//	void OnCancel() {
//		Debug.Log ("Taking a picture has been cancelled.");
//	}

//	private Texture2D rotateTexture(Texture2D tex, float angle)
//	{
//		Debug.Log("rotating");
//		Texture2D rotImage = new Texture2D(tex.width, tex.height);
//		int  x,y;
//		float x1, y1, x2,y2;
//		
//		int w = tex.width;
//		int h = tex.height;
//		float x0 = rot_x (angle, -w/2.0f, -h/2.0f) + w/2.0f;
//		float y0 = rot_y (angle, -w/2.0f, -h/2.0f) + h/2.0f;
//		
//		float dx_x = rot_x (angle, 1.0f, 0.0f);
//		float dx_y = rot_y (angle, 1.0f, 0.0f);
//		float dy_x = rot_x (angle, 0.0f, 1.0f);
//		float dy_y = rot_y (angle, 0.0f, 1.0f);
//		
//		
//		x1 = x0;
//		y1 = y0;
//		
//		for (x = 0; x < tex.width; x++) {
//			x2 = x1;
//			y2 = y1;
//			for ( y = 0; y < tex.height; y++) {
//				//rotImage.SetPixel (x1, y1, Color.clear);          
//				
//				x2 += dx_x;//rot_x(angle, x1, y1);
//				y2 += dx_y;//rot_y(angle, x1, y1);
//				rotImage.SetPixel ( (int)Mathf.Floor(x), (int)Mathf.Floor(y), getPixel(tex,x2, y2));
//			}
//			
//			x1 += dy_x;
//			y1 += dy_y;
//			
//		}
//		
//		rotImage.Apply();
//		return rotImage;
//	}
//	
//	private Color getPixel(Texture2D tex, float x, float y)
//	{
//		Color pix;
//		int x1 = (int) Mathf.Floor(x);
//		int y1 = (int) Mathf.Floor(y);
//		
//		if(x1 > tex.width || x1 < 0 ||
//		   y1 > tex.height || y1 < 0) {
//			pix = Color.clear;
//		} else {
//			pix = tex.GetPixel(x1,y1);
//		}
//		
//		return pix;
//	}
//	
//	private float rot_x (float angle, float x, float y) {
//		float cos = Mathf.Cos(angle/180.0f*Mathf.PI);
//		float sin = Mathf.Sin(angle/180.0f*Mathf.PI);
//		return (x * cos + y * (-sin));
//	}
//	private float rot_y (float angle, float x, float y) {
//		float cos = Mathf.Cos(angle/180.0f*Mathf.PI);
//		float sin = Mathf.Sin(angle/180.0f*Mathf.PI);
//		return (x * sin + y * cos);
//	}
}
