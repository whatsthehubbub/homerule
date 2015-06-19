using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReporterStory4 : MonoBehaviour {

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
		
		cw = chat.GetComponent<ChatWindow>();
		cw.SetArchivalChat(mm.reporterChatHistory.GetComponent<ChatWindow>());

		GameObject displayImage = GameObject.Find ("DisplayImage");
		Sprite videoCallSprite = Resources.Load<Sprite>("Sprites/katja video");
		displayImage.GetComponentInChildren<Image>().sprite = videoCallSprite;

		cw.AddDivider();

		cw.AddNPCBubble("Hé, hoi.");

		GameObject button = null;

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			button = cw.AddButton ("Hoi");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			button = cw.AddButton ("Hé");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			button = cw.AddButton ("Hoi");
		}

		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();

			if (mm.story3Attribution == Story3Attribution.FRANK) {
				cw.AddPlayerBubble("Jij ook hoi.");
			} else if (mm.story3Attribution == Story3Attribution.KATJA) {
				cw.AddPlayerBubble("Hé, hebben ze je vrijgelaten?.");
			} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
				cw.AddPlayerBubble("Ook hoi. Heb je nog nieuws?");
			}
			
			Invoke ("ShowChat", 0.5f);
		});
	}

	public void ShowChat() {
		GameObject.Destroy(chat);
		
		chat = mm.reporterChatHistory;
		mm.reporterChatHistory.SetActive(true);
		
		cw = chat.GetComponent<ChatWindow>();
		cw.DisableBack();

		if (mm.story3Attribution == Story3Attribution.FRANK) {
			cw.AddNPCBubble("Goed nieuws, Frank is vrijgelaten. Jij hebt op de politie ingepraat, hè? Dat heeft er vast iets mee te maken.");
		} else if (mm.story3Attribution == Story3Attribution.KATJA) {
			cw.AddPlayerBubble("Gelukkig wel! En ik heb het idee dat jij daar iets mee te maken hebt. Jij hebt op de politie ingepraat, hè?");
		} else if (mm.story3Attribution == Story3Attribution.ANONYMOUS) {
			cw.AddPlayerBubble("Ik krijg het idee dat de politie zich nu op andere dingen richt. Misschien omdat jij op ze hebt ingepraat.");
		}

		GameObject button = cw.AddButton("Weet niet");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Ik weet het niet?");
			
			Invoke ("ShowResponse", 0.5f);
		});
	}

	public void ShowResponse() {

		cw.AddNPCBubble("Niet zo bescheiden joh!");

		cw.AddNPCBubble("Je krijgt trouwens de groeten van Frank.");

		GameObject button = cw.AddButton("Doe ze terug");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Doe de groeten terug!");
			
			Invoke ("ShowHello", 0.5f);
		});
	}

	public void ShowHello() {
		cw.AddNPCBubble("Bij dezen gedaan. Frank zegt dat je de politie aan het denken hebt gezet. En dat de vogels dankzij jou hun vrijheid terug hebben.");

		cw.AddNPCBubble("Hij zegt ook dat hij zin heeft om je naam op een muur te schrijven.");

		GameObject button = cw.AddButton("Niet doen");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Doe dat maar niet!");
			
			Invoke ("ShowDont", 0.5f);
		});
	}

	public void ShowDont() {
		cw.AddNPCBubble("Dat zei ik ook al. Hoe dan ook, je hebt het echt goed gedaan.");

		cw.AddNPCBubble("Iedereen is in orde. En de mensen hebben veel nagedacht over vrijheid. Jij ook?");

		GameObject button = cw.AddButton("Ja");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Echt wel!");
			
			Invoke ("ShowBye", 0.5f);
		});
	}

	public void ShowBye() {
		cw.AddNPCBubble("Mooi.");

		cw.AddNPCBubble("Ik ben benieuwd hoe lang het rustig blijft. Vrijheid is niet vanzelfsprekend, daar moet je aan blijven werken.");
				
		cw.AddNPCBubble("Wij gaan een kopje thee drinken hier. Nog een fijne dag!");

		GameObject button = cw.AddButton("Ok");
		button.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			
			cw.AddPlayerBubble("Ok");
			
			Invoke ("ShowClose", 0.5f);
		});
	}
	
	public void ShowClose() {
		cw.EnableBack();
		chat.SetActive(false);

		mm.targetText = "Het spel is klaar";
		mm.UpdateTargetText();

		mm.story4Done = true;
		mm.callBusy = false;
		
		GameObject.Destroy(this);
	}
}
