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

		stories.Clear();
		stories.Add ("SIGN", new Story("Story about the sign"));
	}
	
	void OnDestroy() {
		iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent -= OnBluetoothStateChanged;
	}

	void Update () {
		// Debug code to move between Scenes
//		if (Input.GetKeyDown(KeyCode.Alpha1)) {
//			ShowIdle();
//		}
//		if (Input.GetKeyDown(KeyCode.Alpha2)) {
//			StartStory("HOME");
//		}

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

		// Display observations if there are any on this location
		Story story;
		if (stories.TryGetValue(this.playerState, out story)) {
			if (story.active) {
				GameObject observation = (GameObject)Instantiate(Resources.Load ("Prefabs/Observeer UI"));
				observation.name = "Observeer UI";

				Text storytext = observation.GetComponentInChildren<Text>();
				storytext.text = story.name;
			}
		} else if (!this.playerState.Equals("UNDERWAY") && this.locations.Keys.Contains(this.playerState)) {
			// There is no story here
			GameObject nostory = (GameObject)Instantiate(Resources.Load ("Prefabs/No story"));
			nostory.name = "No story";

			// Point the player to a place where there is a story
			List<string> activeLocations = stories.Where(kvp => kvp.Value.active).Select(kvp => kvp.Key).ToList();
			Text nostorytext = nostory.GetComponentInChildren<Text>();

			if (activeLocations.Count > 0) {
				nostorytext.text = "Er is hier geen verhaal maar in " + activeLocations[0] + " wel.";
			} else {
				nostorytext.text = "De verhalen zijn op.";
			}

		}
//
//		// If you're in the office
//		if (Application.loadedLevelName.Equals(locations["OFFICE"].sceneName)) {
//			//  replenish your observations if they are gone
//			if (this.observationLocations.Count == 0) {
//				CreateNewObservations();
//			}
//		}

//		if (Application.loadedLevelName.Equals ("Idle")) {
//			GameObject.Find("FreedomIndexDisplay").GetComponent<Text>().text = "" + this.freedomIndex;
//		}

		// Set the correct number of photos taken in the resources UI
//		UpdateObservationsDisplay();
//		UpdatePublicationDisplay();
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
//						StartStory(entry.Value.name);
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
		Application.LoadLevel ("Intro Reporter");
	}

	public void IntroReporterDone() {
		Application.LoadLevel ("Intro Officer");
	}

	public void IntroOfficerDone() {
		changeScene = true;
		NewLocation ("UNDERWAY");
	}

	public void ShowIntro() {
		Application.LoadLevel("Intro Scene");
	}
	
	public void ShowEventSituation() {
		Application.LoadLevel("EventSituation");
	}

	public void ShowEventCallToAction() {
		Application.LoadLevel ("EventCallAction");
	}

	public void ShowArticleStart() {
		Application.LoadLevel ("ArticleStart");
	}

	/*
	 * Game logic
	 */

	public void ArticleSubmitted(bool positive) {
//		if (positive) {
//			this.freedomIndex -= 1;
//		} else {
//			this.freedomIndex += 1;
//		}
//
//		this.storiesPublished += 1;
	}

	public void StartStory() {
		// Do stuff when we start a story

		this.playerState = "STORY-" + this.playerState;;

		this.changeScene = false;

		Application.LoadLevel("Story Communication");
	}


	/*
	 * Previous version stuff.
	 */
	
//	public void FoundObservation() {
//		if (this.officerLocation == this.playerLocation) {
//			// You got caught son
//			CaughtByOfficer();
//		} 
//		else {
//			this.observationsFound += 1;
//
//			Animator fotosAnim = GameObject.Find("fotos").GetComponent<Animator>();
//			fotosAnim.Play("icon bounce");
//
//			UpdateObservationsDisplay();
//
//			// Remove observation from the local array
//			observationLocations.Remove(this.playerLocation);
//		}
//	}

//	public void CreateNewObservations() {
//		observationLocations.Add(publicLocations[UnityEngine.Random.Range(0, publicLocations.Length)]);
//	}
//
//	public void ConvertObservationIntoPublication() {
//		if (this.observationsFound > 0) {
//			this.observationsFound -= 1;
//			this.storiesPublished += 1;
//
//			Animator fotosAnim = GameObject.Find("fotos").GetComponent<Animator>();
//			fotosAnim.Play("icon bounce");
//			Animator artikelenAnim = GameObject.Find("artikelen").GetComponent<Animator>();
//			artikelenAnim.Play("icon bounce");
//
//			UpdateObservationsDisplay();
//			UpdatePublicationDisplay();
//		}
//	}

//	public void UpdatePublicationDisplay() {
//		GameObject publicationsUI = GameObject.Find ("artikelen int");
//
//		if (publicationsUI != null) {
//			publicationsUI.GetComponent<Text>().text = "" + this.storiesPublished;
//		}
//	}
//
//	public void UpdateObservationsDisplay() {
//		GameObject fotosUI = GameObject.Find ("fotos int");
//
//		if (fotosUI != null) {
//			fotosUI.GetComponent<Text>().text = "" + this.observationsFound;
//		}
//	}

//	public void CaughtByOfficer() {
//		Debug.Log ("Got caught by officer");
//
//		changeScene = false;
//
//		GameObject officerOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent UI"));
//		officerOverlay.name = "Agent UI";
//
//		// Remove observation at this location if there still were any
//		observationLocations.Remove (this.playerLocation);
//
//		// Remove the observation UI as well
//		Destroy(GameObject.Find ("Observeer UI"));
//
//		// Remove observations on you
//		this.observationsFound = 0;
//		UpdateObservationsDisplay();
//	}

//	public void OfficerDone() {
//		changeScene = true;
//
//		// Move the officer
//		MoveOfficer(new string[] {this.playerLocation});
//
//		// Remove officer overlay
//		Destroy(GameObject.Find ("Agent UI"));
//		Destroy (GameObject.Find ("Officer(Clone)"));
//	}

//	public void MoveOfficer(string[] exclude = null) {
//		exclude = exclude ?? new string[0];
//
//		List<string> keys = new List<string>(new string[] {"SQUARE", "MARKET", "STATION"});
//
//		foreach (var toExclude in exclude) {
//			keys.Remove(toExclude);
//		}
//		
////		officerLocation = keys[UnityEngine.Random.Range(0, keys.Count)];
//	}

//	public void HintClicked() {
//		Text hintText = GameObject.Find ("Hint Text").GetComponent<Text>();
//		hintText.text = "BLablablba";
//	}
}
