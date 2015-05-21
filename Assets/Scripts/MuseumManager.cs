using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public struct Location {
	public string name;
	public int minor;
	public bool shown;
	
	public Location(string name, int minor, bool shown) {
		this.name = name;
		this.minor = minor;
		this.shown = shown;
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

	private Dictionary<int, Location> locations = new Dictionary<int, Location>(){
		{48618, new Location("EPISODE1", 48618, false)},
		{22290, new Location("EPISODE2", 22290, false)},
		{48174, new Location("EPISODE3", 48174, false)},
//		{"MARKET", new Location("MARKET", "Market Scene", 53868)},
//		{"STATION", new Location("STATION", "Station Scene", 45444)},
//		{"UNDERWAY", new Location("UNDERWAY", "Underway", -1)}
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
			MovedIntoBeaconRange(48618);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			MovedIntoBeaconRange(22290);
		}		
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			MovedIntoBeaconRange(48174);
		}		
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			MovedOutOfBeaconRange();
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
				
				foreach(KeyValuePair<int, Location> entry in locations) {
					if (entry.Value.minor == b.minor) {
						found = true;

						MovedIntoBeaconRange(entry.Value.minor);

//						if (playerLocation != entry.Value.name) {
//							NewEvent(entry.Value.name);
//						}

//						if (!this.playerState.Equals (entry.Value.name)) {
//							NewLocation(entry.Value.name);
//						}
					}
				}
			} else if (b.range == BeaconRange.FAR) {
//			} else if (b.range == BeaconRange.FAR || b.range == BeaconRange.NEAR) {
				// We want to keep it at this location unless another one is nearer
				found = true;
			}
		}

		if (!found) {
			MovedOutOfBeaconRange();
//			ShowIdle ();
//			NewLocation("UNDERWAY");
		}
	}

	public void MovedIntoBeaconRange(int number) {
		// If there is an episode here that we want to go to, show that

		Location loc = this.locations[number];

		if (!loc.shown) {
			loc.shown = true;

			switch (number) {
			case 48618:
				TakeImmediateCall(1);
				break;
			case 22290:
				TakeImmediateCall(2);
				break;
			case 48174:
				TakeImmediateCall(3);
				break;
			}
		} else {
			TakeCall ();
		}

		Debug.Log ("after switch" + loc.shown);

		this.locations[number] = loc;
	}

	public void MovedOutOfBeaconRange() {
		// Check whether we have a bit of story to show and give that
		TakeCall ();
	}

	public void TakeCall() {
		if (!this.callBusy && this.storyQueue.Count > 0) {
			string storyBit = this.storyQueue.Dequeue();

			this.callBusy = true;

			switch (storyBit) {
			case "INTROREPORTER":
				this.gameObject.AddComponent<IntroReporter>();

				break;
			case "INTROOFFICER":
				this.gameObject.AddComponent<IntroOfficer>();
				break;

			case "REPORTERRESPONSE":
				this.gameObject.AddComponent<ReporterResponse>();

				break;
			}
		}
	}

	public void TakeImmediateCall(int episodeNumber) {
		if (!this.callBusy) {
			this.callBusy = true;

			switch (episodeNumber) {
			case 1:
				this.gameObject.AddComponent<ReporterStory1>();

				break;
			case 2:
				this.gameObject.AddComponent<ReporterStory2>();

				break;
			case 3:
				this.gameObject.AddComponent<ReporterStory3>();

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
