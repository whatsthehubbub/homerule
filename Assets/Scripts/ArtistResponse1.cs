using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArtistResponse1 : MonoBehaviour {

	public MuseumManager mm;
	
	public GameObject chat;
	public ChatWindow cw;

	public AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		// Enable the artist button after our first chat
		mm.artistButton.SetActive(true);

		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/ArtistCalling"));
		call.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		call.name = "Artist Calling";

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

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.artistChatHistory.GetComponent<ChatWindow>());

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/portrait kunstenaar wide");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Hoi, weet je wie ik ben?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject what = cw.AddButton ("Ja");
		what.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			if (mm.story1OpinionDescription == Story1OpinionDescription.VANDAL) {
				cw.AddPlayerBubble("Jij bent die vandaal, toch?");
			} else if (mm.story1OpinionDescription == Story1OpinionDescription.CITIZEN) {
				cw.AddPlayerBubble("Jij bent die man van de graffiti, toch?");
			} else if (mm.story1OpinionDescription == Story1OpinionDescription.ARTIST) {
				cw.AddPlayerBubble("Jij bent die kunstenaar, toch?");
			}
				
			StartCoroutine(ShowChat());
		});
	}
	
	public IEnumerator ShowChat() {
		GameObject.Destroy(chat);

		chat = mm.artistChatHistory;
		mm.artistChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		yield return new WaitForSeconds(0.5f);

		if (mm.story1OpinionDescription == Story1OpinionDescription.VANDAL) {
			cw.AddNPCBubble("Vandaal? Even dimmen, ik ben een dichter!");
		} else if (mm.story1OpinionDescription == Story1OpinionDescription.CITIZEN) {
			cw.AddNPCBubble("Klopt. Zelf noem ik het een stadsgedicht.");
		} else if (mm.story1OpinionDescription == Story1OpinionDescription.ARTIST) {
			cw.AddNPCBubble("Klopt. Ik was bijna opgepakt vanwege mijn gedichten.");
		}

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Ik weet dat ik niet op gebouwen mag schrijven. Maar soms doe ik het toch. Als ik echt iets te zeggen heb.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Ik ben trouwens Frank. Aangenaam kennis te maken.");

		yield return new WaitForSeconds(0.5f);
		
		GameObject nice = cw.AddButton("Aangenaam");
		nice.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Aangenaam, Frank.");

			StartCoroutine(ShowArtistStatement());
		});
	}

	public IEnumerator ShowArtistStatement() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Zeg, is je iets raars opgevallen net, met die huizen?");

		yield return new WaitForSeconds(0.5f);

		GameObject no = cw.AddButton("Nee");
		no.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Nee, hoezo?");
			
			StartCoroutine(ShowArtistLook());
		});
	}

	public IEnumerator ShowArtistLook() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Kijk nog eens goed.");

		yield return new WaitForSeconds(0.5f);

		GameObject imageBubble = cw.AddNPCImageBubble();
		Sprite homesSprite = Resources.Load<Sprite>("Sprites/S2 intro closeup");
		imageBubble.transform.Find("Bubble/BubbleImage").GetComponentInChildren<Image>().sprite = homesSprite;

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton("Vogels");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Het zijn allemaal vogels!");
			
			StartCoroutine(ShowArtistSafety());
		});
	}

	public IEnumerator ShowArtistSafety() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Inderdaad. Wij vogels houden van vrijheid. We vinden het niks als iemand zegt wat we moeten doen.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Daarom zijn we uit onze huizen gezet.");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Veiligheid?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Maar dat was toch vanwege de veiligheid?");
			
			StartCoroutine(ShowArtistTrouble());
		});
	}

	public IEnumerator ShowArtistTrouble() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Dat zeiden ze, maar het klopt niet. We moesten eruit omdat ze ons lastig vinden.");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton("Lastig?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Hoe bedoel je, lastig?");
			
			StartCoroutine(ShowArtistExplanation1());
		});
	}

	public IEnumerator ShowArtistExplanation1() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("We zitten vaak buiten op de stoep, muziek te maken.");

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Dat mag niet, de politie zegt dat het zorgt voor overlast. Maar wij zijn dol op onze fluitconcerten.");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton("Lastig!");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Ik kan me voorstellen dat ze dat lastig vinden!");

			StartCoroutine(ShowArtistExplanation2());
		});
	}

	public IEnumerator ShowArtistExplanation2() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Lastig of niet, ze moeten niet liegen. Vrije vogels vinden dat verschrikkelijk!");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Maar ik bel om je iets te vragen.");

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton("Wat?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Wat wil je vragen?");
			
			StartCoroutine(ShowArtistQuestion());
		});
	}

	public IEnumerator ShowArtistQuestion() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Jij kent toch die verslaggever? Wil je me helpen dit verhaal naar buiten te brengen?");

		yield return new WaitForSeconds(0.5f);

		GameObject scared = cw.AddButton("Durf niet");
		scared.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			mm.artist1Answer = Artist1Answer.SCARED;

			cw.AddPlayerBubble("Ik durf het niet.");
			
			StartCoroutine(ShowArtistResponse());
		});

		GameObject dunno = cw.AddButton("Weet niet");
		dunno.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			mm.artist1Answer = Artist1Answer.DUNNO;

			cw.AddPlayerBubble("Ik weet het nog niet.");
			
			StartCoroutine(ShowArtistResponse());
		});

		GameObject good = cw.AddButton("Oké");
		good.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			mm.artist1Answer = Artist1Answer.CONFIRM;

			cw.AddPlayerBubble("Oké. Ik doe het.");
			
			StartCoroutine(ShowArtistResponse());
		});
	}

	public IEnumerator ShowArtistResponse() {
		yield return new WaitForSeconds(0.5f);

		if (mm.artist1Answer == Artist1Answer.SCARED) {
			cw.AddNPCBubble("Ik begrijp het. De politie houdt je in de gaten, het is spannend. Maar dit is zo belangrijk. Wees moedig!");
		} else if (mm.artist1Answer == Artist1Answer.DUNNO) {
			cw.AddNPCBubble("Ik begrijp het. De politie houdt je in de gaten, het is spannend. Maar dit is zo belangrijk. Wees moedig!");
		} else if (mm.artist1Answer == Artist1Answer.CONFIRM) {
			cw.AddNPCBubble("Wat fijn! Kun je haar vertellen wat er aan de hand is?");
		}

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Oké…");

			StartCoroutine(ShowConclusion());
		});
	}

	public IEnumerator ShowConclusion() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Het is schandalig hoe ze ons behandelen. Wij zijn altijd de gebeten honden! Dat moet stoppen.");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Zeg, ik voel een gedicht opkomen. Ik hang op.");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Succes");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Succes ermee. Tot later!");
			
			cw.EnableBack();
			chat.SetActive(false);
			
			mm.callBusy = false;
			
			GameObject.Destroy(this);
		});
	}
}
