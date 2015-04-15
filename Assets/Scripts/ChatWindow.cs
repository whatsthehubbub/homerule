using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatWindow : MonoBehaviour {

	public float startY = 600;
	public float bubblePadding;

	public GameObject lastAdded;
	public bool heightDirty = false;

	// Use this for initialization
	void Start () {

		this.bubblePadding = 30;
	}
	
	// Update is called once per frame
	void Update () {
		// Really weird but we have to wait for the value to be calculated and that may take some time

		// TODO this will go wrong if too much stuff is added at once
		Rect rect = ((RectTransform)lastAdded.transform).rect;

		if (heightDirty && rect.height > 0.0f) {
			Debug.Log (((RectTransform)lastAdded.transform).rect);
			
			this.startY -= rect.height;

			// Also now that we know the height, move it down a bit more to correct for very high bubbles
			Vector2 position = lastAdded.transform.localPosition;
			position.y -= (rect.height / 2);
			lastAdded.transform.localPosition = position;

			this.startY -= bubblePadding;

			Debug.Log ("New start Y " + this.startY);
			
			heightDirty = false;
		}
	}


	public GameObject AddPlayerBubble(string text) {
		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;

		GameObject playerBubble = (GameObject)Instantiate(Resources.Load ("Prefabs/PlayerChatbubble"));
		playerBubble.name = "New Button";

		Text chatText = playerBubble.GetComponentInChildren<Text>();
		chatText.text = text;

		playerBubble.transform.parent = scrollContent.transform;
		playerBubble.transform.localPosition = new Vector2(70.0f, startY);

		this.lastAdded = playerBubble;
		this.heightDirty = true;

		return playerBubble;
	}

	// TODO code is duplicate
	public GameObject AddNPCBubble(string text) {
		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
		
		GameObject playerBubble = (GameObject)Instantiate(Resources.Load ("Prefabs/NPCChatbubble"));
		playerBubble.name = "New Button";
		
		Text chatText = playerBubble.GetComponentInChildren<Text>();
		chatText.text = text;
		
		playerBubble.transform.parent = scrollContent.transform;
		playerBubble.transform.localPosition = new Vector2(-70.0f, startY);
		
		this.lastAdded = playerBubble;
		this.heightDirty = true;
		
		return playerBubble;
	}

}
