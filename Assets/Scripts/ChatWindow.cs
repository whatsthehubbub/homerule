using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatWindow : MonoBehaviour {

	private ChatWindow archivalChat;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetArchivalChat(ChatWindow cw) {
		this.archivalChat = cw;
	}

	public GameObject AddBubble(string text, string party) {
		string prefabName = "";

		switch (party) {
			case "PLAYER":
				prefabName = "Prefabs/PlayerChatbubble";
				break;
			case "NPC":
				prefabName = "Prefabs/NPCChatbubble";
				break;
			default:
				break;
		}

		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
		
		GameObject bubble = (GameObject)Instantiate(Resources.Load (prefabName));
		bubble.name = party + "Button";
		
		Text chatText = bubble.GetComponentInChildren<Text>();
		chatText.text = text;

		if (this.archivalChat != null) {
			this.archivalChat.AddBubble(text, party);
		}
		
		bubble.transform.SetParent (scrollContent.transform, false);
		
		return bubble;
	}

	public GameObject AddPlayerBubble(string text) {
		return this.AddBubble(text, "PLAYER");
	}

	public GameObject AddNPCBubble(string text) {
		return this.AddBubble(text, "NPC");
	}

	public GameObject AddImageBubble(string party) {
		string prefabName = "";
		
		switch (party) {
		case "PLAYER":
			prefabName = "Prefabs/PlayerImageBubble";
			break;
		case "NPC":
			prefabName = "Prefabs/NPCImageBubble";
			break;
		default:
			break;
		}
	
		GameObject imageBubble = (GameObject)Instantiate(Resources.Load (prefabName));
		imageBubble.name = party + " ImageBubble";

		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
		imageBubble.transform.SetParent (scrollContent.transform, false);
		
		return imageBubble;
	}

	public GameObject AddPlayerImageBubble() {
		return this.AddImageBubble("PLAYER");
	}

	public GameObject AddNPCImageBubble() {
		return this.AddImageBubble ("NPC");
	}

	public GameObject AddButton(string text) {
		GameObject buttonArea = this.gameObject.transform.Find("ButtonArea").gameObject;

		GameObject button = (GameObject)Instantiate (Resources.Load ("Prefabs/PlayerChatButton"));
		button.name = "PlayerChatButton";

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
