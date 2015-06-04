using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory0 : MonoBehaviour {

	public GameObject chat;
	public ChatWindow cw;
	
	public MuseumManager mm;
	
	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		AudioClip ringtone = Resources.Load<AudioClip>("Audio/ringtone");
		this.audioSource = main.GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = ringtone;
		audioSource.Play ();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
		call.name = "Katja belt";
		
		call.GetComponentInChildren<Button>().onClick.AddListener(() => {
			audioSource.Stop ();
			
			GameObject.Destroy(call);
			
			StartStory ();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartStory() {
		// Pause the change scene
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";
		
		// Show the correct sprite (Journalist)
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Hoi!");
		cw.AddNPCBubble("Dit is de content bij Beacon 0!");
		
		GameObject ok = cw.AddButton("OK.");
		ok.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("OK!");
			
			Invoke ("ShowResultClose", 0.5f);
		});
	}

	public void ShowResultClose() {
		GameObject bye = cw.AddButton("Tot ziens");
		bye.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens, Katja!");
			
			mm.callBusy = false;
			
			mm.story0Done = true;
			
			Destroy(chat);
			Destroy (this.audioSource);
		});
	}
}
