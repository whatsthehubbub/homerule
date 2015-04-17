using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroReporter : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	// Use this for initialization
	void Start () {

		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
		call.name = "Katja belt";

		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
			GameObject.Destroy(call);
			ShowChatButton();
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowChatButton() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";
		
		cw = chat.GetComponent<ChatWindow>();

		cw.AddNPCBubble("Hoi! Ik ben Katja. Ik werk als journalist.");

		Invoke ("ShowChatButton2", 0.5f);
	}

	public void ShowChatButton2() {
		cw.AddNPCBubble("Ik zie dat je in het Airborne Museum bent. Wat cool!");

		Invoke ("ShowChatButton3", 0.5f);
	}

	public void ShowChatButton3() {
		cw.AddNPCBubble("Er is daar van alles te zien over vrijheid. Ik wil daar graag berichten over schrijven. Jij kunt me daarmee helpen.");

		GameObject hoe = cw.AddButton("Hoe?");
		hoe.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Hoe kan ik je helpen?");
			cw.ClearButtons();

			Invoke ("ShowHelp1", 0.5f);
		});
	}

	public void ShowHelp1() {
		cw.AddNPCBubble("Loop door het museum en kijk goed om je heen. Ik bel je als er iets te doen is.");

		Invoke ("ShowHelp2", 0.5f);
	}

	public void ShowHelp2() {
		cw.AddNPCBubble("Ga eerst kijken bij de kast met medailles. Die ziet er zo uit:");

		GameObject goed = cw.AddButton ("Is goed");
		goed.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Is goed, ik ga op zoek naar de kast met medailles.");

			Invoke ("ShowReporterClose", 0.5f);
		});
	}

	public void ShowReporterClose() {
		cw.AddNPCBubble("Super! Als je daar bent dan roep ik je op.");

		GameObject gaan = cw.AddButton ("Gaan");
		gaan.GetComponentInChildren<Button>().onClick.AddListener(() => {
			GameObject main = GameObject.Find("Main");
			if (main != null) {
				main.SendMessage("IntroReporterDone");
			}
		});
	}
}
