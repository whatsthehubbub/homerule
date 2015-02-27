using UnityEngine;
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
		{"UNDERWAY", new Location("UNDERWAY", "Underway Scene", -1)}
	};
	
	
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;
	
	public string playerLocation;
	public string officerLocation;
	
	public int observationsFound;
	public int storiesPublished;
	public string[] observationLocations;
	
	// Use this for initialization
	void Start () {
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.BluetoothStateChangedEvent += OnBluetoothStateChanged;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log ("Listening for beacons");
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
	}
	
	private void OnBeaconRangeChanged(List<Beacon> beacons) { // 
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
						NewLocation(entry.Value);
					}
				}
			}
		}
		if (!found) {
			NewLocation(locations["UNDERWAY"]);
		}
	}
	
	void NewLocation(Location location) {
		this.playerLocation = location.name;
		
		Application.LoadLevel(location.sceneName);
		
		//		if (location == Location.HOME) {
		//			Application.LoadLevel("Home Scene");
		//		} else if (location == Location.OFFICE) {
		//			Application.LoadLevel ("Office Scene");
		//		} else if (location == Location.SQUARE) {
		//			Application.LoadLevel ("Square Scene");
		//		} else if (location == Location.MARKET) {
		//			Application.LoadLevel ("Market Scene");
		//		} else if (location == Location.STATION) {
		//			Application.LoadLevel ("Station Scene");
		//		} else if (location == Location.UNDERWAY) {
		//			Application.LoadLevel("Underway");
		//		}
		
		//		GameObject rocket = GameObject.Find("RocketSprite");
		
		//		rocket.GetComponent<RocketManager>().NewBeacon(location);
	}
	
	public void ButtonPressed() {
		NewLocation (locations["OFFICE"]);
	}
	
	void OnGUI() {
		GUIStyle labelStyle = GUI.skin.GetStyle("Label");
		
		labelStyle.fontSize = 25;
		
		string locationText = "";
		if (this.playerLocation == "HOME") {
			locationText = "Je bent thuis.";
		} else if (this.playerLocation == "OFFICE") {
			locationText = "Je bent op kantoor.";
		}
		
		float currenty = 10;
		float labelHeight = labelStyle.CalcHeight(new GUIContent(locationText), Screen.width-20);
		GUI.Label(new Rect(currenty, 10, Screen.width-20, labelHeight), locationText);
		
		//		currenty += labelHeight;
		//		scrolldistance = GUI.BeginScrollView(new Rect(10, currenty,Screen.width -20, Screen.height - currenty - 10), scrolldistance, new Rect(0, 0, Screen.width - 20, mybeacons.Count*100));
		//		GUILayout.BeginVertical("box", GUILayout.Width(Screen.width-20), GUILayout.Height(50));
		//		foreach (Beacon b in mybeacons) {
		//			GUILayout.Label("UUID: " + b.UUID);
		//			GUILayout.Label("Major: " + b.major);
		//			GUILayout.Label("Minor: " + b.minor);
		//			GUILayout.Label("Range: " + b.range.ToString());
		//		}
		//		GUILayout.EndVertical();
		//		GUI.EndScrollView();
	}
}
