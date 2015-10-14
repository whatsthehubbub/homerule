using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public enum Story1OpinionAnswer {
	CLEAN,
	LEAVE,
	DISPLAY
}

public enum Story1OpinionDescription {
	VANDAL,
	CITIZEN,
	ARTIST
}

public enum OfficerResponse1Answer {
	NEVER_SEEN,
	WHY,
	YES
}

public enum Story2OpinionAnswer {
	SAD,
	GOOD,
	WRONG
}

public enum OfficerResponse2Opinion {
	AGREE,
	NEUTRAL,
	DISAGREE
}

public enum Artist1Answer {
	SCARED,
	DUNNO,
	CONFIRM
}

public enum Reporter2Source {
	FRANK,
	SELF,
	ANONYMOUS
}

public enum Story3Attribution {
	FRANK,
	KATJA,
	ANONYMOUS
}

public enum Officer3Response {
	DISAGREE,
	UNDERSTAND
}


public class MuseumManager : MonoBehaviour {
	/* In order these are:
	 * - the gun
	 * - the wallpaper
	 * - the sign
	 * - wilhelmina
	 */
	private List<int> locations = new List<int>(new int[] {53868, 48618, 22290, 48174});

	public Dictionary<string, float> callDelays = new Dictionary<string, float> {
		{"OFFICERRESPONSE1", 120.0f},
		{"REPORTERRESPONSE1", 10.0f},
		{"OFFICERRESPONSE2", 30.0f},
		{"ARTISTRESPONSE1", 10.0f},
		{"REPORTERRESPONSE2", 30.0f},
		{"OFFICERRESPONSE3", 10.0f}
	};

	public bool forceCalls = false;

	/*
	 * The stories will be processed in this order (some of them depending on a beacon being present.
	 * ReporterStory0 - Introductie van Katja
	 * ReporterStory1 - Het gedoe met de graffiti
	 * OfficerResponse1
	 * ReporterResponse1
	 * ReporterStory2 - De herrie rond de huizen
	 * OfficerResponse2
	 * ArtistResponse1
	 * ReporterResponse2
	 * ReporterStory3 - Het vraagstuk over vrijheid
	 * OfficerResponse3
	 * ReporterStory4 - Afscheid van Katja
	 */
	public Queue<string> storyQueue = new Queue<string>();
	public bool waitingForCall = false;
	public bool callBusy = false;

	public Museum museum;
	public Goal goal;

	[Header("Interface objects")]
	public GameObject canvas;

	public GameObject reporterButton;
	public GameObject officerButton;
	public GameObject artistButton;

	public GameObject reporterChatHistory;
	public GameObject officerChatHistory;
	public GameObject artistChatHistory;

	[Header("Story data")]
	public bool story0Done = false;

	public Story1OpinionAnswer story1Opinion;
	public int story1FactAnswer;
	public Story1OpinionDescription story1OpinionDescription;
	public string story1Text = "";
	public bool story1Done = false;
	public Texture2D story1Image;

	public OfficerResponse1Answer officer1Answer;

	public Story2OpinionAnswer story2Opinion;
	public int story2FactAnswer;
	public Story2OpinionAnswer story2FinalOpinion;
	public string story2Text = "";
	public bool story2Done = false;
	[System.NonSerialized] public Texture2D story2Image;

	public OfficerResponse2Opinion officer2Opinion;

	public Artist1Answer artist1Answer;

	public Reporter2Source reporter2Source;

	public int story3FactAnswer;
	public Story3Attribution story3Attribution;
	public string story3Text = "";
	public bool story3Done = false;
	[System.NonSerialized] public Texture2D story3Image;

	public bool story4Done = false;

	public Officer3Response officer3Response;

	[Header("Beacon stuff")]

	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	[Header("Google Analytics")]

	public GoogleAnalyticsV4 googleAnalytics;

	/**
	 * MuseumManager general beacon housekeeping.
	 */

	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");

		this.callBusy = true;

		this.museum = new AirborneMuseum();
//		this.museum = new DummyMuseum();

		this.goal = museum.GetStartGoal();


		// Initialization
		this.story0Done = false;
		this.story1Done = false;
		this.story2Done = false;
		this.story3Done = false;
		this.story4Done = false;

		// On desktop we want to initialize all the images with something otherwise we get a null if we jump randomly through the story
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
			story1Image = new Texture2D(2, 2, TextureFormat.ARGB32, false);
			story2Image = new Texture2D(2, 2, TextureFormat.ARGB32, false);
			story3Image = new Texture2D(2, 2, TextureFormat.ARGB32, false);
		}
	}
	
	void OnDestroy() {
		iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent -= OnBluetoothStateChanged;
	}

	void Update () {
		// Debug code to move between Scenes

		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			if (!forceCalls) {
				MovedIntoBeaconRange(53868);
			} else {
				TakeImmediateCall(0);
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			if (!forceCalls) {
				MovedIntoBeaconRange(48618);
			} else { 
				TakeImmediateCall(1);
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			if (!forceCalls) {
				MovedIntoBeaconRange(22290);
			} else {
				TakeImmediateCall(2);
			}
		}		
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			if (!forceCalls) {
				MovedIntoBeaconRange(48174);
			} else {
				TakeImmediateCall(3);
			}

		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			MovedOutOfBeaconRange();
		}

		if (Application.loadedLevelName.Equals("Underway")) {
			UpdateTargetText();
		}

		TakeCall ();
	}
	
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
		DontDestroyOnLoad(GameObject.Find ("GAv4"));
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
		if (Application.loadedLevelName.Equals("Underway")) {
			this.canvas = GameObject.Find ("Canvas");

			// Create the chat windows to keep the history in (and make sure they don't get destroyed on scene change)
			reporterChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
			reporterChatHistory.transform.SetParent(canvas.transform, false);
			reporterChatHistory.name = "ReporterChatHistory";
			UnityEngine.Object.DontDestroyOnLoad(reporterChatHistory);
			
			ChatWindow reporterChatWindow = reporterChatHistory.GetComponent<ChatWindow>();
			reporterChatWindow.SetNPCAvatar("katja");
			reporterChatWindow.SetLastMessageDisplay("Reporter");
			
			reporterChatHistory.transform.Find("TopBar/Title").GetComponent<Text>().text = "Katja";
			reporterChatHistory.SetActive(false);
			
			
			officerChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
			officerChatHistory.transform.SetParent(canvas.transform, false);
			officerChatHistory.name = "OfficerChatHistory";
			UnityEngine.Object.DontDestroyOnLoad(officerChatHistory);
			
			ChatWindow officerChatWindow = officerChatHistory.GetComponent<ChatWindow>();
			officerChatWindow.SetNPCAvatar("agent");
			officerChatWindow.SetLastMessageDisplay("Officer");
			
			officerChatHistory.transform.Find("TopBar/Title").GetComponent<Text>().text = "Agent";
			officerChatHistory.SetActive(false);
			
			
			artistChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
			artistChatHistory.transform.SetParent(canvas.transform, false);
			artistChatHistory.name = "ArtistChatHistory";
			UnityEngine.Object.DontDestroyOnLoad(artistChatHistory);
			
			ChatWindow artistChatWindow = artistChatHistory.GetComponent<ChatWindow>();
			artistChatWindow.SetNPCAvatar("kunstenaar");
			artistChatWindow.SetLastMessageDisplay("Artist");
			
			artistChatHistory.transform.Find("TopBar/Title").GetComponent<Text>().text = "Frank";
			artistChatHistory.SetActive(false);

			// Hook up the buttons
			reporterButton = GameObject.Find("ReporterChatsButton");
			officerButton = GameObject.Find("OfficerChatsButton");
			artistButton = GameObject.Find("ArtistChatsButton");

			// Hide the chat history buttons of people we haven't talked to yet
			reporterButton.SetActive(false);
			officerButton.SetActive(false);
			artistButton.SetActive(false);
		}
	}
	
	private void OnBeaconRangeChanged(List<Beacon> beacons) {

		// Print out all the beacons there are
		Debug.Log ("============= Beacon range changed");
		var orderMap = new Dictionary<BeaconRange, int>() {
			{ BeaconRange.IMMEDIATE, 0},
			{ BeaconRange.NEAR, 1},
			{ BeaconRange.FAR, 2},
			{ BeaconRange.UNKNOWN, 3}
		};
		List<Beacon> sortedBeacons = beacons.OrderBy(b => orderMap[b.range]).ToList();
		foreach (Beacon b in sortedBeacons) {
			Debug.Log ("Minor: " + b.minor + " Range: " + b.range);
		}


		foreach (Beacon b in beacons) {
			if (mybeacons.Contains(b)) {
				mybeacons[mybeacons.IndexOf(b)] = b;
			} else {
				// this beacon was not in the list before
				// this would be the place where the BeaconArrivedEvent would have been spawned in the the earlier versions
				mybeacons.Add(b);
			}
		}
		
		foreach (Beacon b in mybeacons.ToList()) {
			if (b.lastSeen.AddSeconds(10) < DateTime.Now) {
				// we delete the beacon if it was last seen more than 10 seconds ago
				// this would be the place where the BeaconOutOfRangeEvent would have been spawned in the earlier versions
				mybeacons.Remove(b);
			}
		}
		
		bool found = false;
		foreach (Beacon b in mybeacons) {
			if (b.range == BeaconRange.NEAR || b.range == BeaconRange.IMMEDIATE || b.range == BeaconRange.FAR) {
//			if (b.range == BeaconRange.IMMEDIATE) {

				if (locations.IndexOf(b.minor) != -1) {
					found = true;

//					Debug.Log ("Moved into beacon range " + b.minor);

					MovedIntoBeaconRange(b.minor);
				}
				
//				foreach(KeyValuePair<int, Location> entry in locations) {
//					if (entry.Value.minor == b.minor) {
//						found = true;
//
//						MovedIntoBeaconRange(entry.Value.minor);
//
////						if (playerLocation != entry.Value.name) {
////							NewEvent(entry.Value.name);
////						}
//
////						if (!this.playerState.Equals (entry.Value.name)) {
////							NewLocation(entry.Value.name);
////						}
//					}
//				}
			} else if (b.range == BeaconRange.FAR) {
//			} else if (b.range == BeaconRange.FAR || b.range == BeaconRange.NEAR) {
				// We want to keep it at this location unless another one is nearer
				found = true;
			}
		}

		if (!found) {
			Debug.Log ("Moved out of beacon range");
			MovedOutOfBeaconRange();
//			ShowIdle ();
//			NewLocation("UNDERWAY");
		}
	}

	public void MovedIntoBeaconRange(int number) {
		// If there is an episode here that we want to go to, show that
		bool showedLocation = false;

		switch (number) {
			case 53868:
				if (!this.story0Done) {
					showedLocation = true;
					TakeImmediateCall(0);
				} else if (this.story3Done && !this.story4Done) {
					showedLocation = true;
					TakeImmediateCall(4);
				}
				break;
			case 48618:
				if (this.story0Done && !this.story1Done) {
					showedLocation = true;
					TakeImmediateCall(1);
				}
				break;
			case 22290:
				if (this.story1Done && !this.story2Done) {
					showedLocation = true;
					TakeImmediateCall(2);
				}
				break;
			case 48174:
				if (this.story2Done && !this.story3Done) {
					showedLocation = true;
					TakeImmediateCall(3);
				}
				break;
		}

		if (!showedLocation) {
			TakeCall ();
		}

	}

	public void MovedOutOfBeaconRange() {
		// Check whether we have a bit of story to show and give that
		TakeCall ();
	}
	
	public void UpdateTargetText() {
		// Put the text in the right place
		GameObject.Find ("GoalBar").GetComponentInChildren<Text>().text = this.goal.GetGoalText();
	}

	public void PreCallCleanUp() {
		// Destroy the overlay if there is one
		GameObject goal = GameObject.Find("GoalOverlay");

		if (goal != null) {
			Destroy (goal);
		}

		// Switch back to the Calls Tab because of some weird Unity stuff
		GameObject camera = GameObject.Find ("Main Camera");
		camera.GetComponentInChildren<Underway>().ShowChatHistory();

		// Close the chat histories
		reporterChatHistory.SetActive(false);
		officerChatHistory.SetActive(false);
		artistChatHistory.SetActive(false);
	}

	public void TakeCall() {
		// Do the check and if we have a call to show, then block and invoke that in number of seconds equal to the delay set for that call
		if (!this.callBusy && !this.waitingForCall && this.storyQueue.Count > 0) {
			this.waitingForCall = true;

			float delay = callDelays[this.storyQueue.Peek ()];

//			if (Application.platform == RuntimePlatform.OSXEditor) {
//				// Remove this for testing on desktop
//				delay = 2.0f;
//			}

			Debug.Log ("Going to take a call in: " + delay);

			Invoke("TakeCallDelayed", delay);
		}
	}

	public void TakeCallDelayed() {
		// We can become busy between TakeCall and TakeCallDelayed (like for instance when sharing)
		// Don't take a call then but call this method again in a bit
		if (this.callBusy) {
			Debug.Log ("Busy and can't take a delayed call. Trying later.");

			Invoke ("TakeCallDelayed", 10.0f);
			return;
		}

		this.waitingForCall = false;
		this.callBusy = true;

		string storyBit = this.storyQueue.Dequeue();

		PreCallCleanUp();

		switch (storyBit) {

		// New style labels for the various calls you can get
		case "OFFICERRESPONSE1":
			this.gameObject.AddComponent<OfficerResponse1>();
			
			break;
		case "REPORTERRESPONSE1":
			this.gameObject.AddComponent<ReporterResponse1>();
			
			break;
		case "OFFICERRESPONSE2":
			this.gameObject.AddComponent<OfficerResponse2>();
			
			break;
		case "ARTISTRESPONSE1":
			this.gameObject.AddComponent<ArtistResponse1>();
			
			break;
		case "REPORTERRESPONSE2":
			this.gameObject.AddComponent<ReporterResponse2>();
			
			break;
		case "OFFICERRESPONSE3":
			this.gameObject.AddComponent<OfficerResponse3>();
			
			break;
		}
	}

	public void TakeImmediateCall(int episodeNumber) {
		if (!this.callBusy) {
			// Only advance the story if we don't have any outstanding response calls
			if (this.storyQueue.Count == 0) {
				this.callBusy = true;

				PreCallCleanUp();

				switch (episodeNumber) {
				case 0:
					this.gameObject.AddComponent<ReporterStory0>();

					googleAnalytics.LogEvent("Call", "Reporter Story 0", "Reporter story 0 started", 0);
					break;
				case 1:
					this.gameObject.AddComponent<ReporterStory1>();

					googleAnalytics.LogEvent("Call", "Reporter Story 1", "Reporter story 1 started", 0);

					break;
				case 2:
					this.gameObject.AddComponent<ReporterStory2>();

					googleAnalytics.LogEvent("Call", "Reporter Story 2", "Reporter story 2 started", 0);

					break;
				case 3:
					this.gameObject.AddComponent<ReporterStory3>();

					googleAnalytics.LogEvent("Call", "Reporter Story 3", "Reporter story 3 started", 0);

					break;
				case 4:
					this.gameObject.AddComponent<ReporterStory4>();

					googleAnalytics.LogEvent("Call", "Reporter Story 4", "Reporter story 4 started", 0);

					break;
				}
			} else {
				TakeCall();
			}
		}
	}

	public void StartGameButton() {
		Application.LoadLevel ("Underway");

		this.callBusy = false;

		googleAnalytics.LogEvent("Main", "Game Started", "Start game button pressed", 0);
	}

	public void QuitGame() {
		// Reset the state in the MuseumkidsHolder
		MuseumKids m = GameObject.Find ("MuseumkidsHolder").GetComponent<MuseumKids>();
		m.story1Shared = false;
		m.story2Shared = false;
		m.story3Shared = false;

		m.storyToShare = -1;
		m.textToShare = "";
		m.imageToShare = null;



		Destroy (this.reporterChatHistory);
		Destroy (this.officerChatHistory);
		Destroy (this.artistChatHistory);

		Destroy (this.story1Image);
		Destroy (this.story2Image);
		Destroy (this.story3Image);

		Destroy (this.gameObject);

		Destroy (GameObject.Find ("IBeaconReceiver"));

		Destroy (this);


		googleAnalytics.LogEvent("Main", "Game Quit", "Player ended game", 0);
		Destroy (GameObject.Find ("GAv4"));

		Application.LoadLevel ("Start Scene");
	}
}
