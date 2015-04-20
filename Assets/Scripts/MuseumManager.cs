﻿using UnityEngine;
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

public class MuseumManager : MonoBehaviour {

	private Dictionary<string, Location> locations = new Dictionary<string, Location>(){
		{"CAMERAS", new Location("CAMERAS", "Home Scene", 48618)},
		{"MEDALS", new Location("MEDALS", "Office Scene", 22290)},
		{"SIGN", new Location("SIGN", "Square Scene", 48174)},
//		{"MARKET", new Location("MARKET", "Market Scene", 53868)},
//		{"STATION", new Location("STATION", "Station Scene", 45444)},
		{"UNDERWAY", new Location("UNDERWAY", "Underway", -1)}
	};
	
	public Dictionary<string, Story> stories = new Dictionary<string, Story>(){
	};

	public bool changeScene = true;

	public bool showKatjaIntroSurveillanceResponse = false;

	public bool storyCompleted = false;
	
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	public string playerState;

	/**
	 * MuseumManager general beacon housekeeping.
	 */

	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");

//		CreateNewObservations();
//		UpdatePublicationDisplay();

		changeScene = false;
		showKatjaIntroSurveillanceResponse = false;
		storyCompleted = false;

		stories.Clear();
		stories.Add ("SIGN", new Story("Story about the sign"));
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

//		// Display the officer if he is at this location
//		if (!"".Equals(officerLocation) && Application.loadedLevelName.Equals(locations[officerLocation].sceneName)) {
//			GameObject officer = (GameObject)Instantiate(Resources.Load("Prefabs/Officer"));
//		} else {
//			if (playerLocation != "UNDERWAY") {
//				MoveOfficer(new string[] {playerLocation});
//			}
//		}

		if (showKatjaIntroSurveillanceResponse && (this.playerState == "CAMERAS" || this.playerState == "MEDALS" || this.playerState == "SIGN")) {
			// Show Katja's response to what happened
			this.playerState = "REPORTERRESPONSE";
			Application.LoadLevel ("Reporter Response");
		}

		// Display observations if there are any on this location
		Story story;
		if (stories.TryGetValue(this.playerState, out story)) {
			if (story.active) {
				// If there is something to do here. Katja calls you.
				GameObject call = (GameObject)Instantiate(Resources.Load ("Prefabs/Katja belt"));
				call.name = "Katja belt";
				
				call.GetComponentInChildren<Button>().onClick.AddListener(() => {
					this.changeScene = false;

					this.playerState = "REPORTERSTORY";
					// Start the scene with Kanja and the story in it
					Application.LoadLevel("Reporter Story");

				});
			}
		} else if (!this.playerState.Equals("UNDERWAY") && this.locations.Keys.Contains(this.playerState)) {
			// There is no story here
			GameObject nostory = (GameObject)Instantiate(Resources.Load ("Prefabs/No story"));
			nostory.name = "No story";

			// Point the player to a place where there is a story
			List<string> activeLocations = stories.Where(kvp => kvp.Value.active).Select(kvp => kvp.Key).ToList();
			Text nostorytext = nostory.GetComponentInChildren<Text>();

			string locationString = "";
			if (this.playerState.Equals("CAMERAS")) { locationString = "de kast met camera's"; }
			else if (this.playerState.Equals("MEDALS")) { locationString = "de kast met medailles"; }


			GameObject chat = (GameObject)Instantiate(Resources.Load ("Prefabs/VideoCall"));
			chat.name = "VideoCall";

			// Show the correct sprite (Journalist)
			GameObject displayImage = GameObject.Find ("DisplayImage");
			Sprite katjaSprite = Resources.Load<Sprite>("Sprites/journalist video");
			displayImage.GetComponentInChildren<Image>().sprite = katjaSprite;
			
			ChatWindow cw = chat.GetComponent<ChatWindow>();

			cw.AddNPCBubble("Oh je bent bij de " + locationString + "! Cool.");

			cw.AddNPCBubble("Hier is nu niks te doen. Maar wel bij het bord “verboden Arnhem te betreden”. Ga daar eens kijken?");
		}
//
//		// If you're in the office
//		if (Application.loadedLevelName.Equals(locations["OFFICE"].sceneName)) {
//			//  replenish your observations if they are gone
//			if (this.observationLocations.Count == 0) {
//				CreateNewObservations();
//			}
//		}
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
//			if (b.range == BeaconRange.NEAR || b.range == BeaconRange.IMMEDIATE) {
			if (b.range == BeaconRange.IMMEDIATE) {
				
				foreach(KeyValuePair<string, Location> entry in locations) {
					if (entry.Value.minor == b.minor) {
						found = true;

//						if (playerLocation != entry.Value.name) {
//							NewEvent(entry.Value.name);
//						}
						NewLocation(entry.Value.name);
					}
				}
//			} else if (b.range == BeaconRange.FAR) {
			} else if (b.range == BeaconRange.FAR || b.range == BeaconRange.NEAR) {
				// We want to keep it at this location unless another one is nearer
				found = true;
			}
		}
		if (!found && playerState != "IDLE") {
//			ShowIdle ();
			NewLocation("UNDERWAY");
		}
	}
	
	public void NewLocation(string locationKey, bool forceChange = false) {
		// Hide a map if there is one
		if (forceChange || changeScene) {
			this.playerState = locationKey;	
			
			Application.LoadLevel(locations[locationKey].sceneName);
		}
	}

	/*
	 * Scene Management.
	 */

	public void StartGameButton() {
		this.playerState = "INTROREPORTER";

		Application.LoadLevel ("Intro Reporter");
	}

	public void IntroReporterDone() {
		this.playerState = "INTROOFFICER";

		Application.LoadLevel ("Intro Officer");
	}

	public void IntroOfficerDone() {
		changeScene = true;

		showKatjaIntroSurveillanceResponse = true;

		NewLocation ("UNDERWAY");
	}
}
