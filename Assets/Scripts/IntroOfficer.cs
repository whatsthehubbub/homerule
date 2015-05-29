using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SurveillanceIntroAnswer {
	NEVER_SEEN,
	WHY,
	YES
}

public class IntroOfficer : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	public MuseumManager mm;

	public SurveillanceIntroAnswer playerAnswer;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

		Debug.Log ("Start");
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent belt"));
		call.name = "Agent belt";
		
		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
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
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());

		cw.AddNPCBubble("Hallo. Een moment alstublieft.");

		GameObject what = cw.AddButton ("Wat is er?");
		what.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Wat is er aan de hand?");

			Invoke ("ShowChatButton", 0.5f);
		});
	}


	public void ShowChatButton() {
		GameObject.Destroy(chat);

		chat = mm.officerChatHistory;
		mm.officerChatHistory.SetActive(true);

		cw = chat.GetComponent<ChatWindow>();

		cw.AddNPCBubble("Niks ernstigs.");
		cw.AddNPCBubble("Heeft u deze persoon gezien?");

		cw.AddNPCImageBubble();

		GameObject nooit = cw.AddButton("Nooit gezien");
		nooit.GetComponentInChildren<Button>().onClick.AddListener(() => {
			playerAnswer = SurveillanceIntroAnswer.NEVER_SEEN;
			ShowPlayerResponse();	
		});

		GameObject waarom = cw.AddButton("Waarom?");
		waarom.GetComponentInChildren<Button>().onClick.AddListener(() => {
			playerAnswer = SurveillanceIntroAnswer.WHY;
			ShowPlayerResponse();
		});

		GameObject ja = cw.AddButton("Ja");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			playerAnswer = SurveillanceIntroAnswer.YES;
			ShowPlayerResponse();
		});
	
	}

	public void ShowPlayerResponse() {
		cw.ClearButtons();

		string playerResponseText = "";

		if (playerAnswer == SurveillanceIntroAnswer.NEVER_SEEN) {
			playerResponseText = "Sorry. Die heb ik nog nooit gezien.";
		} else if (playerAnswer == SurveillanceIntroAnswer.YES) {
			playerResponseText = "Ja, die heb ik wel eens gezien.";
		} else if (playerAnswer == SurveillanceIntroAnswer.WHY) {
			playerResponseText = "Waarom vraagt u mij dit?";
		}

		cw.AddPlayerBubble(playerResponseText);

		Invoke("ShowOfficerResponse", 0.8f);
	}

	public void ShowOfficerResponse() {
		string officerResponseText = "";
		
		if (playerAnswer == SurveillanceIntroAnswer.NEVER_SEEN) {
			officerResponseText = "Jammer want we willen met haar praten…";
		} else if (playerAnswer == SurveillanceIntroAnswer.YES) {
			officerResponseText = "Is dat zo? Interessant!";
		} else if (playerAnswer == SurveillanceIntroAnswer.WHY) {
			officerResponseText = "Ze is een journalist. We zijn het niet eens met wat ze schrijft.";
		}
		
		cw.AddNPCBubble(officerResponseText);

		Invoke ("ShowOfficerCommand", 0.5f);
	}

	public void ShowOfficerCommand() {
		cw.AddNPCBubble("Als u de reporter spreekt zeg dan maar dat ze ons moet opzoeken.");

		GameObject ok = cw.AddButton ("OK");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("OK…");
			cw.ClearButtons();

			Invoke ("ShowOfficerCloseOff", 0.5f);
		});
	}

	public void ShowOfficerCloseOff() {
		cw.AddNPCBubble("Bedankt voor uw tijd. U kunt gaan.");

		GameObject ok = cw.AddButton ("Tot ziens");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Tot ziens.");
			cw.ClearButtons();
			
			Invoke ("IntroDone", 1.0f);
		});
	}

	public void IntroDone() {
		mm.callBusy = false;
		mm.storyQueue.Enqueue("REPORTERRESPONSE");

		chat.SetActive(false);
		GameObject.Destroy(this);
	}

}
