using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum StoryOpinionAnswer {
	SAD,
	GOOD,
	WRONG
}

public class ObserveerUI : MonoBehaviour {

	public GameObject cameraButton;
	public GameObject progressBar;
	public float progressSpeed = 0.1f;
	public AudioClip shutterSound;

	public GameObject chat;
	public ChatWindow cw;

	public StoryOpinionAnswer playerOpinion;

	void Awake()
	{
		progressBar.SetActive(false);
	}

	public void OnClickCamera()
	{
		StartCoroutine("MakePhoto");

		StartStory();
	}

	IEnumerator MakePhoto()
	{	//foto maken
		GetComponent<AudioSource>().PlayOneShot(shutterSound);
		progressBar.SetActive(true);
		progressBar.GetComponent<Image>().fillAmount = 100;
		for(float f = 1; f > 0; f-=0.02f)
		{
			progressBar.GetComponent<Image>().fillAmount = f;
			yield return new WaitForSeconds(progressSpeed);
		}
		progressBar.SetActive(false);

		//haal observatie knop weg
		this.gameObject.SetActive(false);

		//observatie +1
//		GameObject main = GameObject.Find("Main");
//		if (main != null) {
//			main.SendMessage("StartStory");
//		}
	}

	public void StartStory() {
		chat = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		chat.name = "Chat";
		
		cw = chat.GetComponent<ChatWindow>();

		cw.AddNPCBubble("Hoi!");
		cw.AddNPCBubble("Er is iets aan de hand. Moet je zien!");

		GameObject wat = cw.AddButton("Wat?");
		wat.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Wat dan?");

			Invoke ("ShowReporterResponse1", 0.5f);
		});

	}

	public void ShowReporterResponse1() {
		cw.AddNPCBubble("Mensen moeten hun huis uit. Ze zijn in de buurt aan het bouwen. Daardoor kunnen huizen instorten. Ze zeggen dat het gevaarlijk is. Maar niet iedereen wil weg.");
		cw.AddNPCBubble("Kun je mij helpen hier over te schrijven?");

		GameObject ja = cw.AddButton("Ja");
		ja.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Ja ik help je");
			
			Invoke ("ShowOpinion1", 0.5f);
		});
	}

	public void ShowOpinion1() {
		cw.AddNPCBubble("Ze willen dat mensen verhuizen omdat het gevaarlijk is waar ze nu wonen.");
		cw.AddNPCBubble("Wat vind je daar van?");

		GameObject zielig = cw.AddButton("Zielig");
		zielig.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Zielig. Waar moeten de mensen heen?");

			playerOpinion = StoryOpinionAnswer.SAD;
			
			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});

		GameObject goed = cw.AddButton("Goed");
		goed.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Goed. Als er gevaar is dan moeten ze je daarvoor beschermen.");

			playerOpinion = StoryOpinionAnswer.GOOD;

			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});

		GameObject verkeerd = cw.AddButton("Verkeerd");
		verkeerd.GetComponentInChildren<Button>().onClick.AddListener(() => {
			cw.ClearButtons();
			cw.AddPlayerBubble("Verkeerd. Mensen mogen zelf weten of ze weg gaan.");

			playerOpinion = StoryOpinionAnswer.WRONG;

			Invoke ("ShowReporterOpinionResponse1", 0.5f);
		});

	}

	public void ShowReporterOpinionResponse1() {
		cw.AddNPCBubble("Helemaal mee eens. Laten we de mensen hier van overtuigen.");

	}

}