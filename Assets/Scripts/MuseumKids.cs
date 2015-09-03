using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;

public class MuseumKids : MonoBehaviour {

	// Class to do the API calls on http://museumkids.ijspreview.nl/api/tester/

	// Site on the staging site.
	// http://museumkids.ijspreview.nl/game-info/shachi/20

	public string email;

	public string authtoken;
	public string sessiontoken;

	public int storyToShare;
	public string textToShare;
	public Texture2D imageToShare;

	// Use this for initialization
	void Start () {
		storyToShare = -1;
	}

	public IEnumerator DoLogin() {
		var url = "http://museumkids.ijspreview.nl/api/login";

		WWWForm form = new WWWForm();
		form.AddField("email", this.email);
		form.AddField("platform_name", "Tablet");
		// This field needs to be added because this stuff is broken
		form.AddField("gamesession_hash", "");

		Debug.Log ("Post to URL: " + url + " with email: " + this.email);

		WWW www = new WWW(url, form);

		yield return www;

		XmlDocument response = new XmlDocument();
		response.LoadXml(www.text);

		Debug.Log(www.text);

//		XmlNode accountStatus = response.GetElementsByTagName("accountstatus")[0];
		XmlNode authToken = response.GetElementsByTagName("authtoken")[0];

		this.authtoken = authToken.InnerText;
	}

	public IEnumerator GetSessionToken() {
		var url = "http://museumkids.ijspreview.nl/api/usersession/tikkit/Shachi/" + this.authtoken;
		
		Debug.Log ("Retrieve URL: " + url);

		// I have no idea why this is a GET request
		WWW www = new WWW(url);
		
		yield return www;

		// The response document contains all kind of stuff, like my name, e-mail and score for this game

		Debug.Log(www.text);

		XmlDocument response = new XmlDocument();
		response.LoadXml(www.text);
		
		XmlNode session = response.GetElementsByTagName("session")[0];
		
		Debug.Log (session.InnerText);
		
		this.sessiontoken = session.InnerText;

	}

	public IEnumerator DoPost() {
		var url = "http://museumkids.ijspreview.nl/api/setItemWithUserdata";

		var item_id = 90; // Behang

		if (storyToShare == 2) {
			item_id = 91; // Bord
		} else if (storyToShare == 3) {
			item_id = 92; // Wilhelmina
		}

		Debug.Log ("Post to URL: " + url + " with session: " + this.sessiontoken + " item: " + item_id + " and text: " + textToShare);

		WWWForm form = new WWWForm();
		form.AddField("gamesession_hash", this.sessiontoken);
		form.AddField("item_id", item_id);
		form.AddField("text", textToShare);
		form.AddBinaryData("file", imageToShare.EncodeToJPG(), "foto.jpg", "image/jpeg");

		WWW www = new WWW(url, form);

		yield return www;

		Debug.Log (www.text);
	}
}
