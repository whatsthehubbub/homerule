using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArtistResponse2 : MonoBehaviour {

	public MuseumManager mm;
	
	public GameObject chat;
	public ChatWindow cw;

	public AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
		call.name = "Katja belt";

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
		
//		// Remove the static image in the video call
//		Destroy (GameObject.Find ("DisplayImage"));
//		
//		// Add the animated officer as a child of the chat
//		GameObject animatedOfficer = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent Animated"));
//		animatedOfficer.transform.parent = chat.transform;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.artistChatHistory.GetComponent<ChatWindow>());
		
		cw.AddNPCBubble("Hé, hoi.");
		
		GameObject button = cw.AddButton ("Hoi");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.AddPlayerBubble("Jij ook hoi.");
			
			Invoke ("ShowChat", 0.5f);
		});
	}
	
	public void ShowChatButton() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.name = "VideoCall";
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());
		
		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
		displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
		
		cw.AddNPCBubble("Dit is het antwoord van de kunstenaar 2.");
		
		GameObject button = cw.AddButton("Hoi");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			ShowClose();
		});
	}
	
	public void ShowClose() {
		
		GameObject button = cw.AddButton ("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			this.mm.callBusy = false;
			
			GameObject.Destroy(chat);
			GameObject.Destroy(this);
		});
	}
}
