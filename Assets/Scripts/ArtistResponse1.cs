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
			ShowVideoCall();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowVideoCall() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/NewVideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.artistChatHistory.GetComponent<ChatWindow>());

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/portrait kunstenaar wide");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;
		
		cw.AddNPCBubble("Hoi, weet je wie ik ben?");
		
		GameObject what = cw.AddButton ("Ja");
		what.GetComponentInChildren<Button>().onClick.AddListener(() => {

			if (mm.story1OpinionDescription == Story1OpinionDescription.VANDAL) {
				cw.AddPlayerBubble("Jij bent die vandaal, toch?");
			} else if (mm.story1OpinionDescription == Story1OpinionDescription.CITIZEN) {
				cw.AddPlayerBubble("Jij bent die man van de graffiti, toch?");
			} else if (mm.story1OpinionDescription == Story1OpinionDescription.ARTIST) {
				cw.AddPlayerBubble("Jij bent die kunstenaar, toch?");
			}
				
			Invoke ("ShowChat", 0.5f);
		});
	}
	
	public void ShowChat() {
		GameObject.Destroy(chat);

		chat = mm.artistChatHistory;
		mm.artistChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		if (mm.story1OpinionDescription == Story1OpinionDescription.VANDAL) {
			cw.AddPlayerBubble("Vandaal? Even dimmen, ik ben een dichter!");
		} else if (mm.story1OpinionDescription == Story1OpinionDescription.CITIZEN) {
			cw.AddPlayerBubble("Klopt. Zelf noem ik het een stadsgedicht.");
		} else if (mm.story1OpinionDescription == Story1OpinionDescription.ARTIST) {
			cw.AddPlayerBubble("Klopt. Ik was bijna opgepakt vanwege mijn gedichten.");
		}
		
		cw.AddNPCBubble("Ik weet dat ik niet op gebouwen mag schrijven. Maar soms doe ik het toch. Als ik echt iets te zeggen heb.");
		cw.AddNPCBubble("Ik ben trouwens Frank. Aangenaam kennis te maken.");
		
		GameObject nice = cw.AddButton("Aangenaam");
		nice.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Aangenaam, Frank.");

			Invoke ("ShowArtistStatement", 0.5f);
		});
	}

	public void ShowArtistStatement() {
		cw.AddNPCBubble("Zeg, is je iets raars opgevallen net, met die huizen?");

		GameObject no = cw.AddButton("Nee");
		no.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Nee, hoezo?");
			
			Invoke ("ShowArtistLook", 0.5f);
		});
	}

	public void ShowArtistLook() {
		cw.AddNPCBubble("Kijk nog eens goed.");

		GameObject imageBubble = cw.AddNPCImageBubble();
		Sprite homesSprite = Resources.Load<Sprite>("Sprites/S2 intro wide");
		imageBubble.transform.Find("BubbleImage").GetComponentInChildren<Image>().sprite = homesSprite;
		
		GameObject button = cw.AddButton("Vogels");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Het zijn allemaal vogels!");
			
			Invoke ("ShowArtistSafety", 0.5f);
		});
	}

	public void ShowArtistSafety() {
		cw.AddNPCBubble("Inderdaad. Wij vogels houden van vrijheid. We vinden het niks als iemand zegt wat we moeten doen.");

		cw.AddNPCBubble("Daarom zijn we uit onze huizen gezet.");

		GameObject button = cw.AddButton("Veiligheid?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Maar dat was toch vanwege de veiligheid?");
			
			Invoke ("ShowArtistTrouble", 0.5f);
		});
	}

	public void ShowArtistTrouble() {
		cw.AddNPCBubble("Dat zeiden ze, maar het klopt niet. We moesten eruit omdat ze ons lastig vinden.");
		
		GameObject button = cw.AddButton("Lastig?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Hoe bedoel je, lastig?");
			
			Invoke ("ShowArtistExplanation1", 0.5f);
		});
	}

	public void ShowArtistExplanation1() {
		cw.AddNPCBubble("We zitten vaak buiten op de stoep, muziek te maken.");
		
		cw.AddNPCBubble("Dat mag niet, de politie zegt dat het zorgt voor overlast. Maar wij zijn dol op onze fluitconcerten.");
		
		GameObject button = cw.AddButton("Lastig!");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Ik kan me voorstellen dat ze dat lastig vinden!");

			Invoke ("ShowArtistExplanation2", 0.5f);
		});
	}

	public void ShowArtistExplanation2() {
		cw.AddNPCBubble("Lastig of niet, ze moeten niet liegen. Vrije vogels vinden dat verschrikkelijk!");

		cw.AddNPCBubble("Maar ik bel om je iets te vragen.");
		
		GameObject button = cw.AddButton("Wat?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Wat wil je vragen?");
			
			Invoke ("ShowArtistQuestion", 0.5f);
		});
	}

	public void ShowArtistQuestion() {
		cw.AddNPCBubble("Jij kent toch die verslaggever? Wil je me helpen dit verhaal naar buiten te brengen?");

		GameObject scared = cw.AddButton("Durf niet");
		scared.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			mm.artist1Answer = Artist1Answer.SCARED;

			cw.AddPlayerBubble("Ik durf het niet.");
			
			Invoke ("ShowArtistResponse", 0.5f);
		});

		GameObject dunno = cw.AddButton("Weet niet");
		dunno.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			mm.artist1Answer = Artist1Answer.DUNNO;

			cw.AddPlayerBubble("Ik weet het nog niet.");
			
			Invoke ("ShowArtistResponse", 0.5f);
		});

		GameObject good = cw.AddButton("Oké");
		good.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			mm.artist1Answer = Artist1Answer.CONFIRM;

			cw.AddPlayerBubble("Oké. Ik doe het.");
			
			Invoke ("ShowArtistResponse", 0.5f);
		});
	}

	public void ShowArtistResponse() {
		if (mm.artist1Answer == Artist1Answer.SCARED) {
			cw.AddNPCBubble("Ik begrijp het. De politie houdt je in de gaten, het is spannend. Maar dit is zo belangrijk. Wees moedig!");

		} else if (mm.artist1Answer == Artist1Answer.DUNNO) {
			cw.AddNPCBubble("Ik begrijp het. De politie houdt je in de gaten, het is spannend. Maar dit is zo belangrijk. Wees moedig!");
		} else if (mm.artist1Answer == Artist1Answer.CONFIRM) {
			cw.AddNPCBubble("Wat fijn! Kun je haar vertellen wat er aan de hand is?");
		}

		Invoke ("ShowConclusion", 0.5f);
	}

	public void ShowConclusion() {

		GameObject button = cw.AddButton("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Oké…");

			cw.AddNPCBubble("Het is schandalig hoe ze ons behandelen. Wij zijn altijd de gebeten honden! Dat moet stoppen.");
			cw.AddNPCBubble("Zeg, ik voel een gedicht opkomen. Ik hang op.");

			
			Invoke ("ShowClose", 0.5f);
		});
	}
	
	public void ShowClose() {
		
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
