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
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Kunstenaar belt"));
		call.name = "Kunstenaar belt";

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
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";
		
		// Remove the static image in the video call
		Destroy (GameObject.Find ("DisplayImage"));
		
		// Add the animated officer as a child of the chat
		GameObject animatedOfficer = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent Animated"));
		animatedOfficer.transform.parent = chat.transform;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.artistChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Hoi, weet je wie ik ben?");
		
		GameObject what = cw.AddButton ("Ja");
		what.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Jij bent die kunstenaar, toch?");
			
			Invoke ("ShowChat", 0.5f);
		});
	}
	
	public void ShowChat() {
		GameObject.Destroy(chat);

		chat = mm.artistChatHistory;
		mm.artistChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
		
		cw.AddNPCBubble("Klopt.");
		cw.AddNPCBubble("Ik was bijna opgepakt vanwege mijn poëzie.");
		cw.AddNPCBubble("Ik heet Frank, trouwens. Aangenaam.");
		
		GameObject nice = cw.AddButton("Aangenaam");
		nice.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			cw.AddPlayerBubble("Aangenaam, Frank.");

			Invoke ("ShowArtistStatement", 0.5f);
		});
	}

	public void ShowArtistStatement() {
		cw.AddNPCBubble("Zeg, is je niets raars opgevallen net, met die huizen?");

		GameObject no = cw.AddButton("Nee");
		no.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Nee, hoezo?");
			
			Invoke ("ShowArtistLook", 0.5f);
		});
	}

	public void ShowArtistLook() {
		cw.AddNPCBubble("Kijk nog eens goed.");

		// TODO add picture of the birds
		
		GameObject button = cw.AddButton("Vogels");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Het zijn allemaal vogels!");
			
			Invoke ("ShowArtistSafety", 0.5f);
		});
	}

	public void ShowArtistSafety() {
		cw.AddNPCBubble("Inderdaad. En wij vogels houden van vrijheid. We vinden het niks als iemand ons vertelt wat we moeten doen.");

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
			
			Invoke ("ShowArtistExplanation", 0.5f);
		});
	}

	public void ShowArtistExplanation() {
		cw.AddNPCBubble("We luisteren alleen als we daar zin in hebben. En oké, misschien betalen we de huur niet altijd.");

		cw.AddNPCBubble("Lastig of niet, ze moeten niet de waarheid verdraaien. Dat kan echt niet door de beugel.");

		cw.AddNPCBubble("Maar ik bel eigenlijk om je iets te vragen.");
		
		GameObject button = cw.AddButton("Wat?");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Wat wil je vragen?");
			
			Invoke ("ShowArtistQuestion", 0.5f);
		});
	}

	public void ShowArtistQuestion() {
		cw.AddNPCBubble("Jij kent toch die journalist? Wil je me helpen om dit verhaal naar buiten te brengen? Overleg als je wilt.");

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

		GameObject good = cw.AddButton("Goed");
		good.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			mm.artist1Answer = Artist1Answer.CONFIRM;

			cw.AddPlayerBubble("Oké, ik doe het.");
			
			Invoke ("ShowArtistResponse", 0.5f);
		});
	}

	public void ShowArtistResponse() {
		if (mm.artist1Answer == Artist1Answer.SCARED) {
			cw.AddNPCBubble("Kom op! Ik snap dat je het spannend vindt, met de politie die je op de vingers kijkt. Maar dit is zo belangrijk!");

		} else if (mm.artist1Answer == Artist1Answer.DUNNO) {
			cw.AddNPCBubble("Ik snap dat je het spannend vindt, met de politie die je op de vingers kijkt. Maar dit is zo belangrijk!");
		} else if (mm.artist1Answer == Artist1Answer.CONFIRM) {
			cw.AddNPCBubble("Wat fijn! Kun je het tegen haar zeggen als je haar spreekt?");
		}

		Invoke ("ShowConclusion", 0.5f);
	}

	public void ShowConclusion() {
		cw.AddNPCBubble("Het is echt schandalig hoe ze ons behandelen. De vogels zijn altijd de gebeten honden. De mensen moeten dat te weten komen!");

		GameObject button = cw.AddButton("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Oké…");
			
			Invoke ("ShowClose", 0.5f);
		});
	}
	
	public void ShowClose() {
		
		GameObject button = cw.AddButton ("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.EnableBack();
			chat.SetActive(false);
			
			mm.callBusy = false;

			GameObject.Destroy(this);
		});
	}
}
