using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum Location {
	HOME = 22183,
	OFFICE = 57167,
	SQUARE = 17,
	MARKET = 18,
	STATION = 22218,
	UNDERWAY = -1
}

public class MuseumManager : MonoBehaviour {
	
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	public Location playerLocation;
	public Location officerLocation;

	public int observationsFound;
	public int storiesPublished;
	public Location[] observationLocations;

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
			NewLocation(Location.HOME);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			NewLocation(Location.OFFICE);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			NewLocation(Location.SQUARE);
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			NewLocation(Location.MARKET);
		}

		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			NewLocation(Location.STATION);
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
				if (b.minor == (int)Location.HOME) {
					NewLocation(Location.HOME);
					found = true;
				} else if (b.minor == (int)Location.OFFICE) {
					NewLocation(Location.OFFICE);
					found = true;
				} else if (b.minor == (int)Location.SQUARE) {
					NewLocation(Location.SQUARE);
					found = true;
				} else if (b.minor == (int)Location.MARKET) {
					NewLocation(Location.MARKET);
					found = true;
				} else if (b.minor == (int)Location.STATION) {
					NewLocation(Location.STATION);
					found = true;
				}
			}
		}
		if (!found) {
			NewLocation(Location.UNDERWAY);
		}
	}

	void NewLocation(Location location) {
		this.playerLocation = location;

		if (location == Location.HOME) {
			Application.LoadLevel("Home Scene");
		} else if (location == Location.OFFICE) {
			Application.LoadLevel ("Office Scene");
		} else if (location == Location.SQUARE) {
			Application.LoadLevel ("Square Scene");
		} else if (location == Location.MARKET) {
			Application.LoadLevel ("Market Scene");
		} else if (location == Location.STATION) {
			Application.LoadLevel ("Station Scene");
		} else if (location == Location.UNDERWAY) {
			Application.LoadLevel("Underway");
		}

//		GameObject rocket = GameObject.Find("RocketSprite");

//		rocket.GetComponent<RocketManager>().NewBeacon(location);
	}

	public void ButtonPressed() {
		NewLocation (Location.OFFICE);
	}
	
	void OnGUI() {
		GUIStyle labelStyle = GUI.skin.GetStyle("Label");

		labelStyle.fontSize = 25;

		string locationText = "";
		if (this.playerLocation == Location.HOME) {
			locationText = "Je bent thuis.";
		} else if (this.playerLocation == Location.OFFICE) {
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
