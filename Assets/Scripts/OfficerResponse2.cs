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
		
		GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/OfficerCalling"));
		call.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		call.name = "Officer Calling";

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
//	void Update () {
//		
//	}

	public void ShowVideoCall() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
		chat.transform.SetParent(GameObject.Find ("Canvas").transform, false);
		chat.name = "VideoCall";

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/portrait agent wide");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.officerChatHistory.GetComponent<ChatWindow>());

		cw.AddDivider();
		
		cw.AddNPCBubble("Daar bent u. Kunnen wij even praten?");
		
		GameObject button = cw.AddButton ("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja hoor.");
			
			StartCoroutine(ShowChatButton());
		});
	}
	
	
	public IEnumerator ShowChatButton() {
		yield return new WaitForSeconds(0.5f);

		GameObject.Destroy(chat);
	
		chat = mm.officerChatHistory;
		mm.officerChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();
		
		cw.AddNPCBubble("Fijn. Wij zien net dit bericht.");

		yield return new WaitForSeconds(0.5f);
		
		// Display the story image
		GameObject imageBubble = cw.AddArticleImageBubble();
		GameObject imageObject = imageBubble.transform.Find ("Bubble/BubbleImage").gameObject;
		Image storyImage = imageObject.GetComponentInChildren<Image>();
		storyImage.sprite = Sprite.Create (mm.story2Image, new Rect(0, 0, mm.story2Image.width, mm.story2Image.height), new Vector2(0.5f, 0.5f));
		
		cw.AddArticleBubble(mm.story2Text);

		yield return new WaitForSeconds(0.5f);
		
		GameObject button = cw.AddButton ("OK");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("OK.");
			
			StartCoroutine(ShowPossibilities());
		});
	}
	
	public IEnumerator ShowPossibilities() {
		yield return new WaitForSeconds(0.5f);

		if (mm.story2FinalOpinion == Story2OpinionAnswer.SAD) {
			cw.AddNPCBubble("Door dit bericht hebben wij extra werk. Wat een toestand! Was die extra zorg echt nodig?");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Het zou fijn zijn als de verslaggever ons voortaan vraagt of het bericht in orde is.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.GOOD) {
			cw.AddNPCBubble("Dit bericht heeft geholpen. De mensen luisterden naar ons, ze gingen netjes hun huizen uit.");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Toch zou het fijn zijn als de verslaggever eerst aan ons vraagt of het bericht in orde is.");
		} else if (mm.story2FinalOpinion == Story2OpinionAnswer.WRONG) {
			cw.AddNPCBubble("Dit bericht is heel vervelend. Nu luisteren de mensen niet meer naar ons!");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("We zijn ze nog steeds uit hun huizen aan het halen. Wat een ellende!");

			yield return new WaitForSeconds(0.5f);

			cw.AddNPCBubble("Voortaan moet de verslaggever eerst aan ons vragen of het bericht in orde is.");
		}

		yield return new WaitForSeconds(0.5f);

		GameObject agree = cw.AddButton ("Mee eens");
		agree.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Mee eens, dat zou moeten.");

			mm.officer2Opinion = OfficerResponse2Opinion.AGREE;
			
			StartCoroutine(ShowOpinionResponse());
		});
		
		GameObject neutral = cw.AddButton ("Als u dat zegt");
		neutral.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Als u dat zegt, dan zal het wel zo zijn.");

			mm.officer2Opinion = OfficerResponse2Opinion.NEUTRAL;
			
			StartCoroutine(ShowOpinionResponse());
		});
		
		GameObject disagree = cw.AddButton ("Niet mee eens");
		disagree.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Niet mee eens! Je mag schrijven wat je wil.");

			mm.officer2Opinion = OfficerResponse2Opinion.DISAGREE;
			
			StartCoroutine(ShowOpinionResponse());
		});
	}
	
	public IEnumerator ShowOpinionResponse() {
		yield return new WaitForSeconds(0.5f);

		if (mm.officer2Opinion == OfficerResponse2Opinion.AGREE) {
			cw.AddNPCBubble("Natuurlijk! Goed dat u het ook zo ziet. Tot later.");
		}
		else if (mm.officer2Opinion == OfficerResponse2Opinion.NEUTRAL) {
			cw.AddNPCBubble("Ja, het is zo. Luister maar gewoon naar ons. Tot ziens!");
		}
		else if (mm.officer2Opinion == OfficerResponse2Opinion.DISAGREE) {
			cw.AddNPCBubble("O ja? Vindt u dat? Dat is goed om te weten. Wij houden u in de gaten…");
		}

		yield return new WaitForSeconds(0.5f);

		GameObject button = cw.AddButton ("Tot ziens");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Tot ziens.");
			
			Invoke ("Done", 0.5f);
		});
	}
	
	public void Done() {
		cw.EnableBack();
		chat.SetActive(false);

		mm.callBusy = false;

		GameObject.Destroy (this);
	}
}
