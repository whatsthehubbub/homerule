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
	
	public Location(string name, string sceneName, int minor) {
		this.name = name;
		this.sceneName = sceneName;
		this.minor = minor;
	}
}

public class MuseumManager : MonoBehaviour {
	
	private Dictionary<string, Location> locations = new Dictionary<string, Location>(){
		{"HOME", new Location("HOME", "Home Scene", 22183)},
		{"OFFICE", new Location("OFFICE", "Office Scene", 57167)},
		{"SQUARE", new Location("SQUARE", "Square Scene", 17)},
		{"MARKET", new Location("MARKET", "Market Scene", 18)},
		{"STATION", new Location("STATION", "Station Scene", 22218)},
		{"UNDERWAY", new Location("UNDERWAY", "Underway", -1)}
	};
	private string[] publicLocations = {"SQUARE", "MARKET", "STATION"};
	
	
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	private GameObject fotosUI;

	public string playerLocation;
	public string officerLocation;
	
	public int observationsFound;
	public int storiesPublished;
	public List<string> observationLocations;
	
	// Use this for initialization
	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");

		CreateNewObservations();
	}
	
	void OnDestroy() {
		iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent -= OnBluetoothStateChanged;
	}
	// Update is called once per frame
	void Update () {
		// Debug code to move between Scenes
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			NewLocation(locations["HOME"]);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			NewLocation(locations["OFFICE"]);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			NewLocation(locations["SQUARE"]);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			NewLocation(locations["MARKET"]);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			NewLocation(locations["STATION"]);
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
			string[] keys = {"SQUARE", "MARKET", "STATION"};

			officerLocation = keys[UnityEngine.Random.Range(0, keys.Length)];
		}

		// Display observations if there are any on this location
		bool showObs = false;
		foreach (string obs in observationLocations) {
			if (Application.loadedLevelName.Equals(locations[obs].sceneName)) {
				showObs = true;
			}
		}
		if (showObs) {
			GameObject observation = (GameObject)Instantiate(Resources.Load ("Prefabs/Observatie UI"));
		}

		// If you're in the office, replenish your observations
		if (Application.loadedLevelName.Equals(locations["OFFICE"].sceneName)) {
			if (this.observationLocations.Count == 0) {
				CreateNewObservations();
			}
		}

		// Set the correct number of photos taken in the resources UI
		fotosUI = GameObject.Find ("fotos int");
		UpdateObservationsDisplay();
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
							NewLocation(entry.Value);
						}
					}
				}
			} else if (b.range == BeaconRange.FAR) {
				// We want to keep it at this location unless another one is nearer
				found = true;
			}
		}
		if (!found && playerLocation != "UNDERWAY") {
			NewLocation(locations["UNDERWAY"]);
		}
	}
	
	void NewLocation(Location location) {
		this.playerLocation = location.name;
		
		Application.LoadLevel(location.sceneName);
	}

	public void FoundObservation() {
		this.observationsFound += 1;
		UpdateObservationsDisplay();

		// Remove observation from the local array
		observationLocations.Remove(this.playerLocation);

	}

	public void CreateNewObservations() {
		observationLocations.Add(publicLocations[UnityEngine.Random.Range(0, publicLocations.Length)]);
	}

	public void UpdateObservationsDisplay() {
		this.fotosUI.GetComponent<Text>().text = "" + this.observationsFound;
	}
}
