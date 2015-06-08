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

public enum Story1OpinionAnswer {
	CLEAN,
	LEAVE,
	DISPLAY
}

public enum Story1FactAnswer {
	COUNT,
	FEAR,
	VANDALISM
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

public enum Story2FactAnswer {
	FIGHTING,
	HELPING,
	STEALING
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

public enum Story3FactAnswer {
	QUEEN,
	HITLER,
	ORGANIZATION
}

public enum Story3Attribution {
	FRANK,
	KATJA,
	ANONYMOUS
}



public class MuseumManager : MonoBehaviour {

	private Dictionary<int, Location> locations = new Dictionary<int, Location>(){
		{53868, new Location("EPISODE0", 53868, false)},
		{48618, new Location("EPISODE1", 48618, false)},
		{22290, new Location("EPISODE2", 22290, false)},
		{48174, new Location("EPISODE3", 48174, false)},
//		{"MARKET", new Location("MARKET", "Market Scene", 53868)},
//		{"STATION", new Location("STATION", "Station Scene", 45444)},
//		{"UNDERWAY", new Location("UNDERWAY", "Underway", -1)}
	};

	public bool forceCalls = false;

	public Queue<string> storyQueue = new Queue<string>();
	public bool callBusy = false;

	public GameObject reporterChatHistory;
	public GameObject officerChatHistory;
	public GameObject artistChatHistory;

	public string targetText = "";

	public bool story0Done = false;

	public Story1OpinionAnswer story1Opinion;
	public Story1FactAnswer story1Fact;
	public Story1OpinionDescription story1OpinionDescription;
	public string story1Text = "";
	public bool story1Done = false;

	public OfficerResponse1Answer officer1Answer;

	public Story2OpinionAnswer story2Opinion;
	public Story2FactAnswer story2Fact;
	public Story2OpinionAnswer story2FinalOpinion;
	public string story2Text = "";
	public bool story2Done = false;

	public OfficerResponse2Opinion officer2Opinion;

	public Artist1Answer artist1Answer;

	public Reporter2Source reporter2Source;

	public Story3FactAnswer story3Fact;
	public Story3Attribution story3Attribution;
	public string story3Text = "";

	public bool story3Done = false;


	public Texture2D storyImage;
	
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	/**
	 * MuseumManager general beacon housekeeping.
	 */

	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");

		this.callBusy = true;

		this.targetText = "Ga naar het begin";

		// Create the chat windows to keep the history in (and make sure they don't get destroyed on scene change)
		reporterChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		reporterChatHistory.name = "ReporterChatHistory";
		UnityEngine.Object.DontDestroyOnLoad(reporterChatHistory);
//		ChatWindow reporterChatWindow = reporterChatHistory.GetComponent<ChatWindow>();
//		reporterChatWindow.SetNPCAvatar("katja");
		reporterChatHistory.transform.Find("topbar/Title").GetComponent<Text>().text = "Katja";
		reporterChatHistory.SetActive(false);

		officerChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		officerChatHistory.name = "OfficerChatHistory";
		UnityEngine.Object.DontDestroyOnLoad(officerChatHistory);
//		ChatWindow officerChatWindow = officerChatHistory.GetComponent<ChatWindow>();
//		officerChatWindow.SetNPCAvatar("agent");
		officerChatHistory.transform.Find("topbar/Title").GetComponent<Text>().text = "Agent";
		officerChatHistory.SetActive(false);

		artistChatHistory = (GameObject)Instantiate(Resources.Load ("Prefabs/Chat"));
		artistChatHistory.name = "ArtistChatHistory";
		UnityEngine.Object.DontDestroyOnLoad(artistChatHistory);
		artistChatHistory.transform.Find("topbar/Title").GetComponent<Text>().text = "Frank";
		artistChatHistory.SetActive(false);
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
		UpdateTargetText();
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

//		Debug.Log (string.Join(", ", Array.ConvertAll(this.storyQueue.ToArray(), i => i.ToString())));

		Location loc = this.locations[number];

		bool showedLocation = false;

		switch (number) {
			case 53868:
				if (!loc.shown) {
					showedLocation = true;
					TakeImmediateCall(0);
				}
				break;
			case 48618:
				if (!loc.shown && this.story0Done) {
					showedLocation = true;
					TakeImmediateCall(1);
				}
				break;
			case 22290:
				if (!loc.shown && this.story1Done) {
					showedLocation = true;
					TakeImmediateCall(2);
				}
				break;
			case 48174:
				if (!loc.shown && this.story2Done) {
					showedLocation = true;
					TakeImmediateCall(3);
				}
				break;
		}

		if (showedLocation) {
			loc.shown = true;
		} else {
			TakeCall ();
		}

		this.locations[number] = loc;
	}

	public void MovedOutOfBeaconRange() {

//		Debug.Log (string.Join(", ", Array.ConvertAll(this.storyQueue.ToArray(), i => i.ToString())));

		// Check whether we have a bit of story to show and give that
		TakeCall ();
	}

	public void UpdateTargetText() {
		// Put the text in the right place
		GameObject.Find ("GoalCanvas").GetComponentInChildren<Text>().text = this.targetText;
	}

	public void TakeCall() {
		if (!this.callBusy && this.storyQueue.Count > 0) {
			string storyBit = this.storyQueue.Dequeue();

			this.callBusy = true;

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
			case "ARTISTRESPONSE2":
				this.gameObject.AddComponent<ArtistResponse2>();
				
				break;
			}
		}
	}

	public void TakeImmediateCall(int episodeNumber) {
		if (!this.callBusy) {
			// Only advance the story if we don't have any outstanding response calls
			if (this.storyQueue.Count == 0) {
				this.callBusy = true;

				switch (episodeNumber) {
				case 0:
					this.gameObject.AddComponent<ReporterStory0>();
					break;
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
			} else {
				TakeCall();
			}
		}
	}

	public void StartGameButton() {
		Application.LoadLevel ("UNDERWAY");

		this.callBusy = false;
	}
}
