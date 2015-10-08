using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterResponse1 : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;
	
	public MuseumManager mm;

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
			StartCoroutine(ShowChatButton());
		});
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
	
	public IEnumerator ShowChatButton() {
		
		// Load the chat stuff
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		cw.AddDivider();
		
		cw.AddNPCBubble("De politie zoekt me. Maar ik doe toch niks verkeerd?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject action = cw.AddButton("Bel ze");
		action.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ze zeggen dat je ze moet bellen.");

			StartCoroutine(ShowResponse1());
		});
	}

	public IEnumerator ShowResponse1() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Dat ga ik mooi niet doen.");

		yield return new WaitForSeconds(0.5f);
		
		cw.AddNPCBubble("Het is belangrijk om op te schrijven wat er gebeurt. Daar ga ik gewoon mee door!");

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton("Doe dat");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Doe dat vooral.");

			StartCoroutine(ShowResponse2());
		});
	}

	public IEnumerator ShowResponse2() {
		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Echt wel!");

		yield return new WaitForSeconds(0.5f);

		cw.AddNPCBubble("Zeg, kun je in het museum zoeken naar het bord “Verboden Arnhem te betreden”?");

		yield return new WaitForSeconds(0.5f);
		
		GameObject ok = cw.AddButton("Doe ik");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Dat zal ik doen. Tot later!");

			cw.EnableBack();
			chat.SetActive(false);

			mm.callBusy = false;

			Goal g = default(Goal);
			g.goalText = "Zoek het bord";
			g.overlayText = "Ga op zoek naar het bord “Verboden Arnhem te betreden”. Dit hangt op de begane grond.";
			g.locationSprite = "bord";
			mm.goal = g;
			
			chat.SetActive(false);
			GameObject.Destroy(this);
		});
	}
}
