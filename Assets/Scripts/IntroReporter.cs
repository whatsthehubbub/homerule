﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroReporter : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;

	public Sprite medalSprite;

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
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";
		cw = chat.GetComponent<ChatWindow>();

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw.AddNPCBubble("Hoi! Ik ben Katja. Ik werk als journalist.");

		GameObject button = cw.AddButton("Hoi");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			ShowChatButton2();
		});
	}

	public void ShowChatButton2() {
		GameObject.Destroy(chat);
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";
		cw = chat.GetComponent<ChatWindow>();
		cw.SetNPCAvatar("katja");

		cw.AddPlayerBubble("Hoi Katja");

		cw.AddNPCBubble("Ik zie dat je in het Airborne Museum bent. Wat cool!");
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

		GameObject npcImageBubble = cw.AddNPCImageBubble();
		GameObject imageObject = npcImageBubble.transform.Find ("BubbleImage").gameObject;
		Image npcImage = imageObject.GetComponentInChildren<Image>();
		npcImage.sprite = medalSprite;

		GameObject button = cw.AddButton ("Is goed");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			ShowReporterClose();
		});
	}

	public void ShowReporterClose() {
		GameObject.Destroy(chat);
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";
		cw = chat.GetComponent<ChatWindow>();

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw.AddPlayerBubble("Is goed, ik ga op zoek naar de kast met medailles.");
		cw.AddNPCBubble("Super! Als je daar bent dan roep ik je op.");

		GameObject button = cw.AddButton ("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			GameObject main = GameObject.Find("Main");
			MuseumManager mm = main.GetComponentInChildren<MuseumManager>();
		
			mm.callBusy = false;

			Application.LoadLevel ("Underway");
		});
	}
}
