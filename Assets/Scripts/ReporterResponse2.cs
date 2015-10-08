using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterResponse2 : MonoBehaviour {

	public MuseumManager mm;
	
	public GameObject chat;
	public ChatWindow cw;

	public AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/ReporterCalling"));
		call.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		call.name = "Reporter Calling";

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
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/portrait katja wide");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		cw.AddDivider();
		
		cw.AddNPCBubble("Hoi, ik verveel me! Niks om over te schrijven.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Heb jij nog nieuws?");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Vogels");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Ik hoorde dat die mensen niet uit hun huis zijn gezet vanwege de veiligheid.");
			
			StartCoroutine(ShowMotivation());
		});
	}

	public IEnumerator ShowMotivation() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Waarom dan wel?");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Vrijheid");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons ();

			cw.AddPlayerBubble("Omdat ze lastig zijn. Omdat ze van vrijheid houden.");
			
			StartCoroutine(ShowChat());
		});
	}
	
	public IEnumerator ShowChat() {
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Als dat klopt, moeten we erover schrijven! Maar hoe weet je dat?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button1 = cw.AddButton("Frank");
		button1.GetComponentInChildren<Button>().onClick.AddListener(() => {

			if (mm.story1OpinionDescription == Story1OpinionDescription.VANDAL) {
				cw.AddPlayerBubble("Ik hoorde het van Frank. Je weet wel, die vandaal. Hij noemt zichzelf dichter.");
			} else if (mm.story1OpinionDescription == Story1OpinionDescription.CITIZEN) {
				cw.AddPlayerBubble("Ik hoorde het van Frank, die man van de graffiti. Zelf noemt hij het een gedicht.");
			} else if (mm.story1OpinionDescription == Story1OpinionDescription.ARTIST) {
				cw.AddPlayerBubble("Ik hoorde het van Frank. Je weet wel, die kunstenaar. Zelf noemt hij zich dichter.");
			}

			cw.AddPlayerBubble("Als we erover schrijven, weet ik niet of we Frank in het bericht moeten zetten. Anders wordt hij misschien opgepakt.");
			
			mm.reporter2Source = Reporter2Source.FRANK;

			cw.ClearButtons();

			StartCoroutine(ShowAnswer());
		});

		GameObject button2 = cw.AddButton("Zelf ontdekt");
		button2.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Ik heb het zelf ontdekt. Als we erover schrijven, weet ik niet of we het bericht moeten ondertekenen. De politie houdt ons in de gaten.");

			mm.reporter2Source = Reporter2Source.SELF;

			cw.ClearButtons();

			StartCoroutine(ShowAnswer());
		});

		GameObject button3 = cw.AddButton("Geheime bron");
		button3.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Dat heb ik van iemand gehoord. We moeten geheimhouden wie hij is. Anders wordt hij misschien opgepakt.");

			mm.reporter2Source = Reporter2Source.ANONYMOUS;

			cw.ClearButtons();

			StartCoroutine(ShowAnswer());
		});
	}

	public IEnumerator ShowAnswer() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Oké, ik begrijp het.");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Wat nu?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Wat moeten we nu doen?");
			
			StartCoroutine(ShowGoal());
		});
	}

	public IEnumerator ShowGoal() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik heb een idee. Kun je op zoek gaan naar de foto van koningin Wilhelmina? Ik leg het straks wel uit.");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Oké.");
			
			mm.callBusy = false;

			Goal g = default(Goal);
			g.goalText = "Zoek de foto";
			g.overlayText = "Ga op zoek naar de foto van koningin Wilhelmina. Deze hangt bij de trap tussen de eerste verdieping en de begane grond.";
			g.locationSprite = "wilhelmina";
			mm.goal = g;

			cw.EnableBack();
			chat.SetActive(false);

			GameObject.Destroy(this);
		});
	}
}
