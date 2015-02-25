using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum PlayerLocation {
	HOME = 15,
	OFFICE = 16,
	SQUARE = 17,
	MARKET = 18,
	STATION = 19,
	UNDERWAY = -1
}

public class MuseumReceiver : MonoBehaviour {

	private Vector2 scrolldistance;
	private List<Beacon> mybeacons = new List<Beacon>();
	private bool scanning = true;

	private PlayerLocation location;

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
				if (b.minor == (int)PlayerLocation.HOME) {
					NewLocation(PlayerLocation.HOME);
					found = true;
				} else if (b.minor == (int)PlayerLocation.OFFICE) {
					NewLocation(PlayerLocation.OFFICE);
					found = true;
				}
			}
		}
		if (!found) {
			NewLocation(PlayerLocation.UNDERWAY);
		}
	}

	void NewLocation(PlayerLocation location) {
		location = location;

		if (location == PlayerLocation.HOME) {
			Application.LoadLevel("Home Scene");
		} else if (location == PlayerLocation.OFFICE) {
			Application.LoadLevel ("Office Scene");
		} else if (location == PlayerLocation.UNDERWAY) {
			Application.LoadLevel("Underway");
		}

//		GameObject rocket = GameObject.Find("RocketSprite");

//		rocket.GetComponent<RocketManager>().NewBeacon(location);
	}

	public void ButtonPressed() {
		NewLocation (PlayerLocation.OFFICE);
	}
	
	void OnGUI() {
		GUIStyle labelStyle = GUI.skin.GetStyle("Label");

		labelStyle.fontSize = 25;

		string locationText = "";
		if (this.location == PlayerLocation.HOME) {
			locationText = "Je bent thuis.";
		} else if (this.location == PlayerLocation.OFFICE) {
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
