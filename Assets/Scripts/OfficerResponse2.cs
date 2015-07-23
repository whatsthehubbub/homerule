using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OfficerResponse2 : MonoBehaviour {

	public MuseumManager mm;
	
	public GameObject chat;
	public ChatWindow cw;

	public AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent belt"));
		call.name = "Agent belt";

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

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/agent video 2");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());

		cw.AddDivider();
		
		cw.AddNPCBubble("Aha, daar bent u. Kunnen we even praten?");
		
		GameObject button = cw.AddButton ("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja hoor.");
			
			Invoke ("ShowChatButton", 0.5f);
		});
	}
	
	
	public void ShowChatButton() {
		GameObject.Destroy(chat);
	
		chat = mm.officerChatHistory;
		mm.officerChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
		
		cw.AddNPCBubble("Fijn. We zien net dit bericht.");
		
		// Display the story image
		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

		GameObject imageBubble = cw.AddNPCImageBubble();
		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		GameObject imageObject = imageBubble.transform.Find ("BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story2Image, new Rect(0, 0, mm.story2Image.width, mm.story2Image.height), new Vector2(0.5f, 0.5f));
		
		GameObject storyBubble = cw.AddNPCBubble(mm.story2Text);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;
		storyBubble.GetComponentInChildren<Text>().color = Color.black;
		
		GameObject button = cw.AddButton ("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("OK.");
			
			Invoke ("ShowPossibilities", 0.5f);
		});
	}
	
	public void ShowPossibilities() {
		if (mm.story2FinalOpinion == Story2OpinionAnswer.SAD) {
			cw.AddNPCBubble("Door dit bericht hebben we extra werk. Mensen moeten verzorgd worden. Dat vinden we niet nodig.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.GOOD) {
			cw.AddNPCBubble("Dit bericht heeft geholpen. Mensen luisteren naar wat we zeggen, ze gaan netjes hun huis uit. Fijn.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.WRONG) {
			cw.AddNPCBubble("We zijn boos over dit bericht. Mensen doen niet wat we zeggen. Maar dat moet wel. We zijn nog steeds bezig mensen uit hun huis te halen!");
		}
		
		cw.AddNPCBubble("Door zo'n bericht gaan mensen anders denken. Het lijkt ons handig als de journalist eerst aan ons vraagt of dat wel mag.");
		
		GameObject agree = cw.AddButton ("Eens");
		agree.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Mee eens, dat zou moeten.");

			mm.officer2Opinion = OfficerResponse2Opinion.AGREE;
			
			Invoke ("ShowOpinionResponse", 0.5f);
		});
		
		GameObject neutral = cw.AddButton ("Als u dat zegt");
		neutral.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Als u dat zegt, dan zal het wel zo zijn.");

			mm.officer2Opinion = OfficerResponse2Opinion.NEUTRAL;
			
			Invoke ("ShowOpinionResponse", 0.5f);
		});
		
		GameObject disagree = cw.AddButton ("Oneens");
		disagree.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Absoluut niet mee eens! Mensen mogen zelf schrijven wat ze denken.");

			mm.officer2Opinion = OfficerResponse2Opinion.DISAGREE;
			
			Invoke ("ShowOpinionResponse", 0.5f);
		});
	}
	
	public void ShowOpinionResponse() {
		if (mm.officer2Opinion == OfficerResponse2Opinion.AGREE) {
			cw.AddNPCBubble("Natuurlijk! Goed dat u het ook zo ziet.");
		}
		else if (mm.officer2Opinion == OfficerResponse2Opinion.NEUTRAL) {
			cw.AddNPCBubble("Ja, dat is zo. Luister maar gewoon naar wat wij zeggen.");
		}
		else if (mm.officer2Opinion == OfficerResponse2Opinion.DISAGREE) {
			cw.AddNPCBubble("O ja? Vindt u dat? Dat is goed om te weten. We houden u in de gaten…");
		}
		
		Invoke ("ShowOfficerCloseOff", 0.5f);
	}
	
	public void ShowOfficerCloseOff() {
		cw.AddNPCBubble("Dat was het wel.");
		
		GameObject button = cw.AddButton ("Oké");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Oké.");
			
			Invoke ("ShowOfficerBye", 0.5f);
		});
	}
	
	public void ShowOfficerBye() {
		
		cw.AddNPCBubble("Gaat u maar weer verder. We spreken u nog wel.");
		
		GameObject button = cw.AddButton ("Tot ziens");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens.");
			
			Invoke ("Done", 0.5f);
		});
	}
	
	public void Done() {
		mm.callBusy = false;

		cw.EnableBack();
		chat.SetActive(false);

		GameObject.Destroy (this);
	}
}
