using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;

public class MuseumKids : MonoBehaviour {

	// Class to do the API calls on http://museumkids.ijspreview.nl/api/tester/

	// API call URL example on the staging site (different domain and https not really supported):
	// http://museumkids.ijspreview.nl/api/login

	// Site on the staging site.
	// http://museumkids.ijspreview.nl/game-info/shachi/20

	public delegate void MuseumkidsHandler();
	public static event MuseumkidsHandler onMuseumkidsLoggedIn;
	public static event MuseumkidsHandler onMuseumkidsLoggedOut;


	public string email;

	public string accountstatus;
	public string authtoken;
	public string sessiontoken;

	public bool story1Shared = false;
	public bool story2Shared = false;
	public bool story3Shared = false;

	public int storyToShare;
	public string textToShare;
	public Texture2D imageToShare;


	private static MuseumKids instance;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
			return;
		}
	}

	// Use this for initialization
	void Start () {
		storyToShare = -1;

		DontDestroyOnLoad(this.gameObject);
	}

	public bool LoggedIn() {
		return !string.IsNullOrEmpty(authtoken) && !string.IsNullOrEmpty(sessiontoken);
	}

	public IEnumerator DoLogin() {
		var url = "https://www.museumkids.nl/api/login";
//		var url = "http://museumkids.ijspreview.nl/api/login";

		WWWForm form = new WWWForm();
		form.AddField("email", this.email);
		form.AddField("platform_name", "Tablet");
		// This field needs to be added because this stuff is broken
		form.AddField("gamesession_hash", "");

		Debug.Log ("Post to URL: " + url + " with email: " + this.email);

		WWW www = new WWW(url, form);

		yield return www;

		// Check whether the request succeeded
		if (www.isDone && string.IsNullOrEmpty(www.error)) {
			XmlDocument response = new XmlDocument();
			response.LoadXml(www.text);
			
			Debug.Log(www.text);
			
			try {
				XmlNode accountStatus = response.GetElementsByTagName("accountstatus")[0];
				this.accountstatus = accountStatus.InnerText;
				
				XmlNode authToken = response.GetElementsByTagName("authtoken")[0];
				this.authtoken = authToken.InnerText;
			} catch (System.NullReferenceException nre) {
				Debug.Log (nre.ToString());
				// If we typed in the wrong e-mail or something, above code throws an error
			}
		}
	}

	public IEnumerator GetSessionToken() {
		// Authtoken could be empty if the previous request failed
		// Then don't try to do this stuff
		if (!string.IsNullOrEmpty(this.authtoken)) {
			var url = "https://www.museumkids.nl/api/usersession/tikkit/Vrijevogels/" + this.authtoken;
//			var url = "http://museumkids.ijspreview.nl/api/usersession/tikkit/Vrijevogels/" + this.authtoken;
			
			Debug.Log ("Retrieve URL: " + url);
			
			// I have no idea why this is a GET request
			WWW www = new WWW(url);
			
			yield return www;

			// Check for the success of the request
			if (www.isDone && string.IsNullOrEmpty(www.error)) {
				// The response document contains all kind of stuff, like my name, e-mail and score for this game
				Debug.Log(www.text);
				
				XmlDocument response = new XmlDocument();
				response.LoadXml(www.text);
				
				XmlNode session = response.GetElementsByTagName("session")[0];
				
				Debug.Log (session.InnerText);
				
				this.sessiontoken = session.InnerText;
				
				if (onMuseumkidsLoggedIn != null) {
					onMuseumkidsLoggedIn();
				}
			}
		}
	}

	public IEnumerator DoPost(Museum m) {
		var url = "https://www.museumkids.nl/api/setItemWithUserdata";
//		var url = "http://museumkids.ijspreview.nl/api/setItemWithUserdata";

		var item_id = m.museumKidsStory1ItemId;

		if (storyToShare == 1) {
			item_id = m.museumKidsStory1ItemId;
		} else if (storyToShare == 2) {
			item_id = m.museumKidsStory2ItemId;
		} else if (storyToShare == 3) {
			item_id = m.museumKidsStory3ItemId;
		}

		if (!string.IsNullOrEmpty(this.sessiontoken)) {
			Debug.Log ("Post to URL: " + url + " with session: " + this.sessiontoken + " item: " + item_id + " and text: " + textToShare);
			
			WWWForm form = new WWWForm();
			form.AddField("gamesession_hash", this.sessiontoken);
			form.AddField("item_id", item_id);
			form.AddField("text", textToShare);
			form.AddBinaryData("file", imageToShare.EncodeToJPG(), "foto.jpg", "image/jpeg");
			
			WWW www = new WWW(url, form);
			
			yield return www;

			if (www.isDone && string.IsNullOrEmpty(www.error)) {
				if (storyToShare == 1) {
					story1Shared = true;
				} else if (storyToShare == 2) {
					story2Shared = true;
				} else if (storyToShare == 3) {
					story3Shared = true;
				}
				Debug.Log (www.text);
			}
		}
	}

	public void Logout() {
		this.email = "";
		this.sessiontoken = "";
		this.authtoken = "";

		this.story1Shared = false;
		this.story2Shared = false;
		this.story3Shared = false;

		if (onMuseumkidsLoggedOut != null) {
			onMuseumkidsLoggedOut();
		}
	}
}
