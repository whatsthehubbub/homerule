using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatWindow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}


	public GameObject AddPlayerBubble(string text) {
		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;

		GameObject playerBubble = (GameObject)Instantiate(Resources.Load ("Prefabs/PlayerChatbubble"));
		playerBubble.name = "New Button";

		Text chatText = playerBubble.GetComponentInChildren<Text>();
		chatText.text = text;

		playerBubble.transform.SetParent (scrollContent.transform, false);

		return playerBubble;
	}

	public GameObject AddPlayerImageBubble() {
		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
		
		GameObject playerImageBubble = (GameObject)Instantiate(Resources.Load ("Prefabs/PlayerImageBubble"));
		playerImageBubble.name = "PlayerImageBubble";
		
		playerImageBubble.transform.SetParent (scrollContent.transform, false);
		
		return playerImageBubble;
	}

	public GameObject AddNPCImageBubble() {
		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
		
		GameObject npcImageBubble = (GameObject)Instantiate(Resources.Load ("Prefabs/NPCImageBubble"));
		npcImageBubble.name = "PlayerImageBubble";
		
		npcImageBubble.transform.SetParent (scrollContent.transform, false);
		
		return npcImageBubble;
	}
	
	public GameObject AddNPCBubble(string text) {
		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
		
		GameObject playerBubble = (GameObject)Instantiate(Resources.Load ("Prefabs/NPCChatbubble"));
		playerBubble.name = "NPC Button";
		
		Text chatText = playerBubble.GetComponentInChildren<Text>();
		chatText.text = text;

		playerBubble.transform.SetParent (scrollContent.transform, false);
		
		return playerBubble;
	}

	public GameObject AddButton(string text) {
		GameObject buttonArea = this.gameObject.transform.Find("ButtonArea").gameObject;

		GameObject button = (GameObject)Instantiate (Resources.Load ("Prefabs/PlayerChatbubble"));
		button.name = "Player button";

		Text chatText = button.GetComponentInChildren<Text>();
		chatText.text = text;

		button.transform.SetParent(buttonArea.transform, false);

		return button;
	}

	public void SetNPCAvatar(string avatar_suffix) {
		GameObject avatar = this.gameObject.transform.Find("topbar/avatar npc").gameObject;
		Image avatarImage = avatar.GetComponentInChildren<Image>();

		Sprite sprite = Resources.Load<Sprite>("Sprites/avatar_" + avatar_suffix);

		avatarImage.sprite = sprite;
	}

	public void ClearButtons() {
		GameObject buttonArea = this.gameObject.transform.Find("ButtonArea").gameObject;

		foreach (Transform child in buttonArea.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

}
