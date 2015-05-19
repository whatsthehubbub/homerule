using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public struct Location {
	public string name;
	public string sceneName;
	public int minor;
	
	public Location(string name, string sceneName, int minor) {
		this.name = name;
		this.sceneName = sceneName;
		this.minor = minor;
	}
}

public struct Story {
	public string name;

	public bool active;

	public Story(string name) {
		this.name = name;

		this.active = true;
	}
}

public enum StoryOpinionAnswer {
	SAD,
	GOOD,
	WRONG
}

public enum StoryFactAnswer {
	FIGHTING,
	HELPING,
	STEALING
}

public enum OfficerResponseOpinion {
	AGREE,
	NEUTRAL,
	DISAGREE
}

public class MuseumManager : MonoBehaviour {

	private Dictionary<string, Location> locations = new Dictionary<string, Location>(){
		{"CAMERAS", new Location("CAMERAS", "Home Scene", 48618)},
		{"MEDALS", new Location("MEDALS", "Office Scene", 22290)},
		{"SIGN", new Location("SIGN", "Square Scene", 48174)},
//		{"MARKET", new Location("MARKET", "Market Scene", 53868)},
//		{"STATION", new Location("STATION", "Station Scene", 45444)},
		{"UNDERWAY", new Location("UNDERWAY", "Underway", -1)}
	};

	public Queue<string> storyQueue = new Queue<string>();
	public bool callBusy = false;

	public GameObject reporterChatHistory;
	public GameObject officerChatHistory;

	public bool showKatjaIntroSurveillanceResponse = false;
	public bool showOfficerStoryResponse = false;

	public StoryOpinionAnswer playerOpinion;
	public StoryFactAnswer playerFact;
	public bool storyCompleted = false;

	public Texture2D storyImage;
	public string storyText;
	
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	public string currentStory;
	public string playerState;

	/**
	 * MuseumManager general beacon housekeeping.
	 */

	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");

		showKatjaIntroSurveillanceResponse = false;
		storyCompleted = false;

		// Create the chat windows to keep the history in (and make sure they don't get destroyed on scene change)
		reporterChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		reporterChatHistory.name = "ReporterChatHistory";
		UnityEngine.Object.DontDestroyOnLoad(reporterChatHistory);
		ChatWindow reporterChatWindow = reporterChatHistory.GetComponent<ChatWindow>();
		reporterChatWindow.SetNPCAvatar("katja");
		reporterChatHistory.SetActive(false);

		officerChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		officerChatHistory.name = "OfficerChatHistory";
		UnityEngine.Object.DontDestroyOnLoad(officerChatHistory);
		ChatWindow officerChatWindow = officerChatHistory.GetComponent<ChatWindow>();
		officerChatWindow.SetNPCAvatar("agent");
		officerChatHistory.SetActive(false);
	}
	
	void OnDestroy() {
		iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent -= OnBluetoothStateChanged;
	}

	void Update () {
		// Debug code to move between Scenes

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			NewLocation("CAMERAS", true);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			NewLocation("MEDALS", true);
		}		
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			NewLocation("SIGN", true);
		}		
//		if (Input.GetKeyDown(KeyCode.Alpha4)) {
//			NewLocation("MARKET");
//		}		
//		if (Input.GetKeyDown(KeyCode.Alpha5)) {
//			NewLocation("STATION");
//		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			NewLocation ("UNDERWAY", true);
		}
	}
	
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
	
	
	private void OnBluetoothStateChanged(BluetoothLowEnergyState newstate) {
		switch (newstate) {
		case BluetoothLowEnergyState.POWERED_ON:
			iBeaconReceiver.Init();
			Debug.Log ("It is on, go searching");
			break;
		case BluetoothLowEnergyState.POWERED_OFF:
			iBeaconReceiver.EnableBluetooth();
			Debug.Log ("It is off, switch it on");
			break;
		case BluetoothLowEnergyState.UNAUTHORIZED:
			Debug.Log("User doesn't want this app to use Bluetooth, too bad");
			break;
		case BluetoothLowEnergyState.UNSUPPORTED:
			Debug.Log ("This device doesn't support Bluetooth Low Energy, we should inform the user");
			break;
		case BluetoothLowEnergyState.UNKNOWN:
		case BluetoothLowEnergyState.RESETTING:
		default:
			Debug.Log ("Nothing to do at the moment");
			break;
		}
	}
	
	void OnLevelWasLoaded(int level) {
		Debug.Log ("Loaded level: " + Application.loadedLevelName);

		if (showKatjaIntroSurveillanceResponse && (this.playerState == "CAMERAS" || this.playerState == "MEDALS" || this.playerState == "SIGN")) {
			// Show Katja's response to what happened
			this.showKatjaIntroSurveillanceResponse = false;
			this.playerState = "REPORTERRESPONSE";
		}

		if (showOfficerStoryResponse && !(this.playerState.Equals ("REPORTERSTORY") || this.playerState.Equals ("SIGN"))) {
			this.showOfficerStoryResponse = false;
			this.playerState = "OFFICERRESPONSE";
//			this.changeScene = false;
			Application.LoadLevel("Officer Response");
		}

		/*
		// Display observations if there are any on this location
		Story story;
		if (stories.TryGetValue(this.playerState, out story)) {
			if (story.active) {
				// If there is something to do here. Katja calls you.
				GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
				call.name = "Katja belt";
//				this.changeScene = false;
				
				call.GetComponentInChildren<Button>().onClick.AddListener(() => {
					this.currentStory = this.playerState;
					this.playerState = "REPORTERSTORY";
					// Start the scene with Kanja and the story in it
					Application.LoadLevel("Reporter Story");

				});
			} else {
				Debug.Log ("This story now is inactive");

				GameObject chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
				chat.name = "VideoCall";
				ChatWindow cw = chat.GetComponent<ChatWindow>();
				
				// Show the correct sprite (Journalist)
				GameObject displayImage = GameObject.Find ("DisplayImage");
				Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
				displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;

				cw.AddNPCBubble("Oh je bent weer bij het bord “verboden Arnhem te betreden”.");
				cw.AddNPCBubble("Daar hebben we al iets mee gedaan. Weet je nog? Dat was dit bericht:");

				Sprite articleSprite = Resources.Load<Sprite>("Sprites/chat_artikel");
				GameObject storyBubble = cw.AddNPCBubble(this.storyText);
				storyBubble.GetComponentInChildren<Image>().sprite = articleSprite;

				cw.AddNPCBubble("Er is nu nergens meer iets te doen. Je hebt me super geholpen.");
			}
		} else if (!this.playerState.Equals("UNDERWAY") && this.locations.Keys.Contains(this.playerState)) {
			string locationString = "";
			if (this.playerState.Equals("CAMERAS")) { locationString = "de kast met camera's"; }
			else if (this.playerState.Equals("MEDALS")) { locationString = "de kast met medailles"; }


			GameObject chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
			chat.name = "VideoCall";
			ChatWindow cw = chat.GetComponent<ChatWindow>();

			// Show the correct sprite (Journalist)
			GameObject displayImage = GameObject.Find ("DisplayImage");
			Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
			displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;

			cw.AddNPCBubble("Oh je bent bij de " + locationString + "! Cool.");

			if (!storyCompleted) {
				cw.AddNPCBubble("Hier is nu niks te doen. Maar wel bij het bord “verboden Arnhem te betreden”. Ga daar eens kijken?");
			} else {
				cw.AddNPCBubble("Er is nu nergens meer iets te doen. Je hebt me super geholpen.");
			}
		}
*/
	}
	
	private void OnBeaconRangeChanged(List<Beacon> beacons) {

		foreach (Beacon b in beacons) {
			if (mybeacons.Contains(b)) {
				mybeacons[mybeacons.IndexOf(b)] = b;
			} else {
				// this beacon was not in the list before
				// this would be the place where the BeaconArrivedEvent would have been spawned in the the earlier versions
				mybeacons.Add(b);
			}
		}
		
		foreach (Beacon b in mybeacons) {
			if (b.lastSeen.AddSeconds(10) < DateTime.Now) {
				// we delete the beacon if it was last seen more than 10 seconds ago
				// this would be the place where the BeaconOutOfRangeEvent would have been spawned in the earlier versions
				mybeacons.Remove(b);
			}
		}
		
		
		bool found = false;
		foreach (Beacon b in mybeacons) {
			if (b.range == BeaconRange.NEAR || b.range == BeaconRange.IMMEDIATE) {
//			if (b.range == BeaconRange.IMMEDIATE) {
				
				foreach(KeyValuePair<string, Location> entry in locations) {
					if (entry.Value.minor == b.minor) {
						found = true;

//						if (playerLocation != entry.Value.name) {
//							NewEvent(entry.Value.name);
//						}

						if (!this.playerState.Equals (entry.Value.name)) {
							NewLocation(entry.Value.name);
						}
					}
				}
			} else if (b.range == BeaconRange.FAR) {
//			} else if (b.range == BeaconRange.FAR || b.range == BeaconRange.NEAR) {
				// We want to keep it at this location unless another one is nearer
				found = true;
			}
		}

		if (!found && playerState != "IDLE") {
//			ShowIdle ();
			NewLocation("UNDERWAY");
		}

		TakeCall ();
	}
	
	public void NewLocation(string locationKey, bool forceChange = false) {
		// Hide a map if there is one
//		if (forceChange || changeScene) {
//			this.playerState = locationKey;
//			
//			Application.LoadLevel(locations[locationKey].sceneName);
//		}

		TakeCall ();
	}

	public void TakeCall() {
		if (!this.callBusy && this.storyQueue.Count > 0) {
			string storyBit = this.storyQueue.Dequeue();

			switch (storyBit) {
			case "INTROREPORTER":
				this.callBusy = true;

//				Application.LoadLevel ("Intro Reporter");
				this.gameObject.AddComponent<IntroReporter>();

				break;
			case "INTROOFFICER":
				this.callBusy = true;

				this.gameObject.AddComponent<IntroOfficer>();
				break;

			case "REPORTERRESPONSE":
				this.callBusy = true;

				this.gameObject.AddComponent<ReporterResponse>();

				break;
			default:
			break;
			}
		}
	}

	/*
	 * Scene Management.
	 */

	public void StartGameButton() {
		Application.LoadLevel ("UNDERWAY");

		this.storyQueue.Enqueue("INTROREPORTER");
		this.storyQueue.Enqueue("INTROOFFICER");
	}
}
