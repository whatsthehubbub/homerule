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

	public SurveillanceIntroAnswer playerAnswer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IntroDone() {
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("IntroOfficerDone");
		}
	}

	public void ShowChatButton() {
		GameObject.Find("PickUpButton").SetActive(false);

		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";

		cw = chat.GetComponent<ChatWindow>();

		cw.AddNPCBubble("Niks ernstigs. Heeft u deze persoon gezien?");

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
			officerResponseText = "Ze is een journalist. We zijn het niet eens met wat ze schrijft. Dus we willen graag met haar praten… ";
		}
		
		cw.AddNPCBubble(officerResponseText);

		Invoke ("ShowOfficerCommand", 0.5f);
	}

	public void ShowOfficerCommand() {
		cw.AddNPCBubble("Als u de reporter spreekt zeg dan maar dat ze ons moet opzoeken.");

		GameObject ok = cw.AddButton ("Ok…");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Ok.");
			cw.ClearButtons();

			Invoke ("ShowOfficerCloseOff", 0.5f);
		});
	}

	public void ShowOfficerCloseOff() {
		cw.AddNPCBubble("Bedankt voor uw tijd. U kunt gaan.");

		GameObject ok = cw.AddButton ("Tot ziens.");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Tot ziens.");
			cw.ClearButtons();
			
			Invoke ("IntroDone", 1.0f);
		});
	}
}
