using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

//
//public enum Location {
//	HOME = 22183,
//	OFFICE = 57167,
//	SQUARE = 17,
//	MARKET = 18,
//	STATION = 22218,
//	UNDERWAY = -1
//}

public struct Location {
	public string name;
	public string sceneName;
	public int minor;
	public string interfaceString;
	
	public Location(string name, string sceneName, int minor, string interfaceString) {
		this.name = name;
		this.sceneName = sceneName;
		this.minor = minor;

		this.interfaceString = interfaceString;
	}
}

public class MuseumManager : MonoBehaviour {
	
	private Dictionary<string, Location> locations = new Dictionary<string, Location>(){
		{"HOME", new Location("HOME", "Home Scene", 48618, "")},
		{"OFFICE", new Location("OFFICE", "Office Scene", 22290, "")},
		{"SQUARE", new Location("SQUARE", "Square Scene", 48174, "het plein")},
		{"MARKET", new Location("MARKET", "Market Scene", 53868, "de markt")},
		{"STATION", new Location("STATION", "Station Scene", 45444, "het station")},
		{"UNDERWAY", new Location("UNDERWAY", "Underway", -1, "")}
	};
	private string[] publicLocations = {"SQUARE", "MARKET", "STATION"};

	public bool changeScene = true;
	
	
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	public string playerLocation;
	public string officerLocation;
	
	public int observationsFound;
	public int storiesPublished;
	public List<string> observationLocations;

	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");

		CreateNewObservations();
		UpdatePublicationDisplay();

		changeScene = false;
	}
	
	void OnDestroy() {
		iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent -= OnBluetoothStateChanged;
	}

	void Update () {
		// Debug code to move between Scenes
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			NewLocation("HOME");
		}		
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			NewLocation("OFFICE");
		}		
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			NewLocation("SQUARE");
		}		
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			NewLocation("MARKET");
		}		
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			NewLocation("STATION");
		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			NewLocation ("UNDERWAY");
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

		// Display the officer if he is at this location
		if (!"".Equals(officerLocation) && Application.loadedLevelName.Equals(locations[officerLocation].sceneName)) {
			GameObject officer = (GameObject)Instantiate(Resources.Load("Officer"));
		} else {
			if (playerLocation != "UNDERWAY") {
				MoveOfficer();
			}
		}

		// Display observations if there are any on this location
		bool showObs = false;
		foreach (string obs in observationLocations) {
			if (Application.loadedLevelName.Equals(locations[obs].sceneName)) {
				showObs = true;
			}
		}
		if (showObs) {
			GameObject observation = (GameObject)Instantiate(Resources.Load ("Prefabs/Observeer UI"));
			observation.name = "Observeer UI";
		}

		// If you're in the office
		if (Application.loadedLevelName.Equals(locations["OFFICE"].sceneName)) {
			//  replenish your observations if they are gone
			if (this.observationLocations.Count == 0) {
				CreateNewObservations();
			}
		}

		// Set the correct number of photos taken in the resources UI
		UpdateObservationsDisplay();
		UpdatePublicationDisplay();
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
				
				foreach(KeyValuePair<string, Location> entry in locations) {
					if (entry.Value.minor == b.minor) {
						found = true;

						if (playerLocation != entry.Value.name) {
							NewLocation(entry.Value.name);
						}
					}
				}
			} else if (b.range == BeaconRange.FAR) {
				// We want to keep it at this location unless another one is nearer
				found = true;
			}
		}
		if (!found && playerLocation != "UNDERWAY") {
			NewLocation("UNDERWAY");
		}
	}
	
	public void NewLocation(string locationKey) {
		// Hide a map if there is one
		if (changeScene) {
			this.playerLocation = locationKey;
			
			Application.LoadLevel(locations[locationKey].sceneName);
		}
	}

	public void FoundObservation() {
		if (this.officerLocation == this.playerLocation) {
			// You got caught son
			CaughtByOfficer();
		} 
		else {
			this.observationsFound += 1;

			Animator fotosAnim = GameObject.Find("fotos").GetComponent<Animator>();
			fotosAnim.Play("icon bounce");

			UpdateObservationsDisplay();

			// Remove observation from the local array
			observationLocations.Remove(this.playerLocation);
		}
	}

	public void CreateNewObservations() {
		observationLocations.Add(publicLocations[UnityEngine.Random.Range(0, publicLocations.Length)]);
	}

	public void ConvertObservationIntoPublication() {
		if (this.observationsFound > 0) {
			this.observationsFound -= 1;
			this.storiesPublished += 1;

			Animator fotosAnim = GameObject.Find("fotos").GetComponent<Animator>();
			fotosAnim.Play("icon bounce");
			Animator artikelenAnim = GameObject.Find("artikelen").GetComponent<Animator>();
			artikelenAnim.Play("icon bounce");

			UpdateObservationsDisplay();
			UpdatePublicationDisplay();
		}
	}

	public void UpdatePublicationDisplay() {
		GameObject publicationsUI = GameObject.Find ("artikelen int");

		if (publicationsUI != null) {
			publicationsUI.GetComponent<Text>().text = "" + this.storiesPublished;
		}
	}

	public void UpdateObservationsDisplay() {
		GameObject fotosUI = GameObject.Find ("fotos int");

		if (fotosUI != null) {
			fotosUI.GetComponent<Text>().text = "" + this.observationsFound;
		}
	}

	public void CaughtByOfficer() {
		Debug.Log ("Got caught by officer");

		changeScene = false;

		GameObject officerOverlay = (GameObject)Instantiate(Resources.Load ("Prefabs/Agent UI"));
		officerOverlay.name = "Agent UI";

		// Remove observation at this location if there still were any
		observationLocations.Remove (this.playerLocation);

		// Remove the observation UI as well
		Destroy(GameObject.Find ("Observeer UI"));

		// Remove observations on you
		this.observationsFound = 0;
		UpdateObservationsDisplay();

		Invoke ("OfficerDone", 5.0f);
	}

	public void OfficerDone() {
		changeScene = true;

		// Move the officer
		MoveOfficer(new string[] {this.playerLocation});

		// Remove officer overlay
		Destroy(GameObject.Find ("Agent UI"));
		Destroy (GameObject.Find ("Officer(Clone)"));
	}

	public void MoveOfficer(string[] exclude = null) {
		exclude = exclude ?? new string[0];

		List<string> keys = new List<string>(new string[] {"SQUARE", "MARKET", "STATION"});

		foreach (var toExclude in exclude) {
			keys.Remove(toExclude);
		}
		
		officerLocation = keys[UnityEngine.Random.Range(0, keys.Count)];
	}

	public void HintClicked() {
		Text hintText = GameObject.Find ("Hint Text").GetComponent<Text>();
		hintText.text = "BLablablba";
	}

	public void StartGame() {
		changeScene = true;
		NewLocation("UNDERWAY");
	}

	public string getLocationInterfaceString(string locationKey) {
		return this.locations[locationKey].interfaceString;
	}
}
