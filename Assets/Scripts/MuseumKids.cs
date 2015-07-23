using UnityEngine;
using System.Collections;
using System.Xml;

public class MuseumKids : MonoBehaviour {

	public string email;

	public string authtoken;

	public string sessiontoken;

	public int score;

	// Use this for initialization
	void Start () {
		this.email = "alper@hubbub.eu";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoginButtonPressed() {
		StartCoroutine(DoLogin());
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

		XmlNode accountStatus = response.GetElementsByTagName("accountstatus")[0];
		XmlNode authToken = response.GetElementsByTagName("authtoken")[0];

		this.authtoken = authToken.InnerText;
	}

	public void SessionButtonPressed() {
		Debug.Log ("Session button pressed");
		
		StartCoroutine(GetSessionToken());
	}
	
	public IEnumerator GetSessionToken() {
		var url = "http://museumkids.ijspreview.nl/api/usersession/tikkit/Shachi/" + this.authtoken;
		
		Debug.Log ("Retrieve URL: " + url);

		// I have no idea why this is a GET request
		WWW www = new WWW(url);
		
		yield return www;

		// The response document contains all kind of stuff, like my name, e-mail and score for this game

		XmlDocument response = new XmlDocument();
		response.LoadXml(www.text);
		
		XmlNode session = response.GetElementsByTagName("session")[0];
		
		Debug.Log (session.InnerText);
		
		this.sessiontoken = session.InnerText;

	}

	public void PostButtonPressed() {
		Debug.Log ("Post button pressed");

		StartCoroutine(DoPost());
	}

	public IEnumerator DoPost() {
		var url = "http://museumkids.ijspreview.nl/api/setScore";

		WWWForm form = new WWWForm();
		form.AddField("gamesession_hash", this.sessiontoken);
		form.AddField("score", this.score);

		WWW www = new WWW(url, form);

		Debug.Log ("Posting to url: " + url + " with session " + this.sessiontoken + " and score " + this.score);

		yield return www;

		Debug.Log (www.text);
	}

	/*

Hierbij een overzicht van de (voor jullie meest relevante) calls van museumkids:
usersession - starten van een sessie per game waarmee je scores en resultaten kan opslaan
identify - simpele login aan de hand van je e-mailadres (voordat er een sessie is aangemaakt)
login - uitgebreide login om sessie aan een gebruiker te koppelen
register - naam en leeftijd om een echt account aan te maken
setScore - koppelt een score aan een sessie
setItem - koppelt een item(id) aan een sessie
getItems - geeft een lijst aan items, gekoppeld aan een game
getItem - geeft info over een bepaald item
Een uitgebreidere beschrijving kun je vinden op http://museumkids.ijspreview.nl/api/tester/


Hierbij even uitgeschreven de flow hoe die in de huidige API zou passen:

Je start een usersessie en krijgt een sessie-token terug.
http://museumkids.ijspreview.nl/api/usersession/{platform_name}/{game_name}/{token}

Of

we kunnen beginnen met een inlog met alleen een e-mailadres
http://museumkids.ijspreview.nl/api/login
Deze geeft ook een sessie-token terug en of dit een bestaande of een nieuwe gebruiker is.

————
 
Met deze sessie token kun je een item inschieten:
http://museumkids.ijspreview.nl/api/setItem/
(met een itemId, dit id is van een item wat voorgedefinieerd is in de database en gekoppeld aan een museum)

Je krijgt dan een Id terug van een userItem (de koppeling tussen een item en een user)
Deze Id kunnen we meesturen met een nog te ontwikkelen call die een afbeelding en tekst koppelt aan dit item.

————

Als er nog niet ingelogd is moeten we deze sessie koppelen aan een gebruiker.
Dat kan opnieuw met http://museumkids.ijspreview.nl/api/login
waarin we de sessie-token meegeven om alles te koppelen.

Bij een nieuwe account kunnen we doorverwijzen voor verdere registratie op de site (of registeren in de app maar dat valt misschien buiten de scope)


	*/
}
