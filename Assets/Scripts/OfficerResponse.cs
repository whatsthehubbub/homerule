using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OfficerResponse : MonoBehaviour {
	
	public GameObject chat;
	public ChatWindow cw;

	public MuseumManager mm;

	public OfficerResponseOpinion playerOpinion;
	
	// Use this for initialization
	void Start () {
		GameObject main = GameObject.Find("Main");
		mm = main.GetComponentInChildren<MuseumManager>();

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
		
		cw = chat.GetComponent<ChatWindow>();
		
		cw.AddNPCBubble("Aha daar bent u. Kunnen we even praten?");
		
		GameObject button = cw.AddButton ("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja hoor");
			
			Invoke ("ShowChatButton", 0.5f);
		});
	}
	
	
	public void ShowChatButton() {
		GameObject.Destroy(chat);
		
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetNPCAvatar("agent");
		
		cw.AddNPCBubble("Fijn. We zien net dit bericht.");

		// Display the story image
		Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");

//		GameObject imageBubble = cw.AddNPCImageBubble();
//		imageBubble.GetComponentInChildren<Image>().sprite = articleSprite;
//		GameObject imageObject = imageBubble.transform.Find ("BubbleImage").gameObject;
//		Image storyImage = imageObject.GetComponentInChildren<Image>();
//		storyImage.sprite = Sprite.Create (mm.storyImage, new Rect(0, 0, 200, 300), new Vector2(0.5f, 0.5f));

		GameObject storyBubble = cw.AddNPCBubble(mm.storyText);
		storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;

		GameObject button = cw.AddButton ("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("OK.");
			
			Invoke ("ShowPossibilities", 0.5f);
		});
	}

	public void ShowPossibilities() {
		if (mm.playerOpinion == StoryOpinionAnswer.SAD) {
			cw.AddNPCBubble("Door dat bericht hebben we extra werk. Mensen moeten verzorgd worden. Dat vinden we niet nodig.");
		}
		else if (mm.playerOpinion == StoryOpinionAnswer.GOOD) {
			cw.AddNPCBubble("Dat bericht heeft geholpen. Mensen luisteren naar wat we zeggen. Ze gaan netjes hun huis uit. Goed zo.");
		}
		else if (mm.playerOpinion == StoryOpinionAnswer.WRONG) {
			cw.AddNPCBubble("We zijn boos over dat bericht. Mensen doen niet wat we zeggen. Maar dat moet wel. We zijn nog steeds bezig met mensen uit hun huis halen.");
		}

		cw.AddNPCBubble("Door zo'n bericht gaan mensen anders denken. We willen dat de journalist eerst aan ons vraagt of dat wel mag.");

		GameObject agree = cw.AddButton ("Eens");
		agree.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Mee eens, dat zou moeten.");

			playerOpinion = OfficerResponseOpinion.AGREE;

			Invoke ("ShowOpinionResponse", 0.5f);
		});

		GameObject neutral = cw.AddButton ("Als u dat zegt");
		neutral.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Als u dat zegt dan zal het wel zo zijn.");

			playerOpinion = OfficerResponseOpinion.NEUTRAL;
			
			Invoke ("ShowOpinionResponse", 0.5f);
		});

		GameObject disagree = cw.AddButton ("Oneens");
		disagree.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Absoluut niet mee eens! Mensen mogen zelf schrijven wat ze denken.");

			playerOpinion = OfficerResponseOpinion.DISAGREE;
			
			Invoke ("ShowOpinionResponse", 0.5f);
		});
	}

	public void ShowOpinionResponse() {

		if (playerOpinion == OfficerResponseOpinion.AGREE) {
			cw.AddNPCBubble("Natuurlijk! Goed dat u het ook zo ziet.");
		}
		else if (playerOpinion == OfficerResponseOpinion.NEUTRAL) {
			cw.AddNPCBubble("Ja dat is zo. Luister maar gewoon naar wat wij zeggen.");
		}
		else if (playerOpinion == OfficerResponseOpinion.DISAGREE) {
			cw.AddNPCBubble("Oh ja? Vindt u dat? Dat is goed om te weten. Dan gaan we u in de gaten houden…");
		}

		Invoke ("ShowOfficerCloseOff", 0.5f);
	}

	public void ShowOfficerCloseOff() {
		cw.AddNPCBubble("Dat was het wel.");

		GameObject button = cw.AddButton ("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("OK.");
			
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
		mm.showOfficerStoryResponse = false;

//		mm.NewLocation ("UNDERWAY");
	}
}
