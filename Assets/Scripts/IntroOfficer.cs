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

	public void IntroDoneButton() {
		GameObject main = GameObject.Find("Main");
		if (main != null) {
			main.SendMessage("IntroOfficerDone");
		}
	}

	public void ShowChatButton() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";

		cw = chat.GetComponent<ChatWindow>();

		GameObject bubble = cw.AddNPCBubble("Niks ernstigs. Heeft u deze persoon gezien?");

		GameObject nooit = cw.AddButton("Nooit gezien");
		nooit.GetComponentInChildren<Button>().onClick.AddListener(() => {
			Debug.Log ("Never clicked");
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

		string responseText = "";

		if (playerAnswer == SurveillanceIntroAnswer.NEVER_SEEN) {
			responseText = "Sorry. Die heb ik nog nooit gezien.";
		} else if (playerAnswer == SurveillanceIntroAnswer.YES) {
			responseText = "Ja, die heb ik wel eens gezien.";
		} else if (playerAnswer == SurveillanceIntroAnswer.WHY) {
			responseText = "Waarom vraagt u mij dit?";
		}

		GameObject response = cw.AddPlayerBubble(responseText);

		Invoke("ShowOfficerResponse", 0.5f);
	}

	public void ShowOfficerResponse() {
		string responseText = "";
		
		if (playerAnswer == SurveillanceIntroAnswer.NEVER_SEEN) {
			responseText = "Jammer want we willen met haar praten…";
		} else if (playerAnswer == SurveillanceIntroAnswer.YES) {
			responseText = "Is dat zo? Interessant!";
		} else if (playerAnswer == SurveillanceIntroAnswer.WHY) {
			responseText = "Ze is een journalist. We zijn het niet eens met wat ze schrijft. Dus we willen graag met haar praten… ";
		}
		
		GameObject response = cw.AddNPCBubble(responseText);

		Invoke ("ShowOfficerCommand", 0.5f);
	}

	public void ShowOfficerCommand() {
		GameObject response = cw.AddNPCBubble("Als u de reporter spreekt zeg dan maar dat ze ons moet opzoeken.");

		GameObject ok = cw.AddButton ("Ok…");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Ok.");
			cw.ClearButtons();

			Invoke ("ShowOfficerCloseOff", 0.5f);
		});
	}

	public void ShowOfficerCloseOff() {
		GameObject response = cw.AddNPCBubble("Bedankt voor uw tijd. U kunt gaan.");

		GameObject ok = cw.AddButton ("Tot ziens.");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Tot ziens.");
			cw.ClearButtons();
			
			Invoke ("IntroDoneButton", 0.5f);
		});
	}
}
