using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatWindow : MonoBehaviour {

	public ChatWindow archivalChat;

	public string lastMessageDisplay;

	public AudioClip bubbleSound;
	public AudioClip buttonSound;
	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		bubbleSound = Resources.Load<AudioClip>("Audio/bubble");
		buttonSound = Resources.Load<AudioClip>("Audio/choice");

		this.audioSource = GameObject.Find ("Main").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
//	void Update () {
//	}

	public void SetArchivalChat(ChatWindow cw) {
		this.archivalChat = cw;
	}

	public void SetLastMessageDisplay(string whose) {
		this.lastMessageDisplay = whose;
	}

	public GameObject AddBubble(string text, string party) {
		// The audio source isn't created as quickly as we would like
		if (this.audioSource != null) {
			audioSource.PlayOneShot(bubbleSound);
		}

		string prefabName = "";

		switch (party) {
			case "PLAYER":
				prefabName = "Prefabs/PlayerChatbubble";
				break;
			case "NPC":
				prefabName = "Prefabs/NPCChatbubble";
				break;
			case "ARTICLE":
				prefabName = "Prefabs/ArticleChatbubble";
				break;
			default:
				break;
		}

		ScrollToBottom();

		GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
		
		GameObject bubble = (GameObject)Instantiate(Resources.Load (prefabName));
		bubble.name = party + "Bubble";
		
		Text chatText = bubble.GetComponentInChildren<Text>();
		chatText.text = text;

		if (this.archivalChat != null) {
			this.archivalChat.AddBubble(text, party);
		}

		if (party.Equals("NPC") && this.lastMessageDisplay != null && !this.lastMessageDisplay.Equals("")) {
			// Show the last message by the NPC in the last message display
			GameObject chatButton = GameObject.Find (this.lastMessageDisplay + "ChatsButton");

			if (chatButton != null) {
				GameObject lastMessageText = chatButton.transform.Find ("SenderLastMessageText").gameObject;
				
				lastMessageText.GetComponentInChildren<Text>().text = text;
			}
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

	public GameObject AddArticleBubble(string text) {
		return this.AddBubble (text, "ARTICLE");
	}

	/*
	 * Note: These are properly archived because they are only ever called on chat windows which are the archive
	 * windows as well. If they would be called during VideoCalls they would not be looped through to the archive
	 * because of how image bubbles work.
	 */
	public GameObject AddImageBubble(string party) {
		if (this.audioSource != null) {
			audioSource.PlayOneShot(bubbleSound);
		}

		string prefabName = "";
		
		switch (party) {
		case "PLAYER":
			prefabName = "Prefabs/PlayerImageBubble";
			break;
		case "NPC":
			prefabName = "Prefabs/NPCImageBubble";
			break;
		case "ARTICLE":
			prefabName = "Prefabs/ArticleImageBubble";
			break;
		default:
			break;
		}

		ScrollToBottom();
	
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

	public GameObject AddArticleImageBubble() {
		return this.AddImageBubble ("ARTICLE");
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

	public void ScrollToBottom() {
		// Scroll the view all the way to the bottom
		GameObject scrollView = this.gameObject.transform.Find("ScrollView").gameObject;
		ScrollRect rect = scrollView.GetComponent<ScrollRect>();

		rect.verticalNormalizedPosition = 0.0f;
	}

	/*
	 * Removes all the chat bubbles in the chat window.
	 */
	public void FlushChildren() {
		GameObject scrollContent = this.gameObject.transform.Find ("ScrollView/ScrollContent").gameObject;

		List<GameObject> children = new List<GameObject>();

		foreach (Transform child in scrollContent.transform) {
			children.Add(child.gameObject);
		}

		children.ForEach(child => Destroy(child));
	}

	public GameObject AddDivider() {
		if (this.archivalChat != null) {
			this.archivalChat.AddDivider();
		} else if (!this.gameObject.name.Equals ("VideoCall")) {
			// Don't add the new messages prefab to videocalls
			GameObject divider = (GameObject)Instantiate(Resources.Load("Prefabs/NewMessages"));
			divider.name = "Divider";
			
			GameObject scrollContent = this.gameObject.transform.Find("ScrollView/ScrollContent").gameObject;
			divider.transform.SetParent (scrollContent.transform, false);
			
			return divider;
		}

		return null;
	}

	public void SetNPCAvatar(string avatar_suffix) {
		GameObject avatar = this.gameObject.transform.Find("TopBar/avatar npc").gameObject;
		Image avatarImage = avatar.GetComponentInChildren<Image>();

		Sprite sprite = Resources.Load<Sprite>("Sprites/avatar_" + avatar_suffix);

		avatarImage.sprite = sprite;
	}

	public void ClearButtons() {
		if (this.audioSource != null) {
			audioSource.PlayOneShot(buttonSound);
		}

		GameObject buttonArea = this.gameObject.transform.Find("ButtonArea").gameObject;

		foreach (Transform child in buttonArea.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void DisableBack() {
		// Disable the button
		this.gameObject.transform.Find ("TopBar/Back").GetComponent<Button>().interactable = false;

		// Change its scale to 0 to hide it
		this.gameObject.transform.Find ("TopBar/Back").transform.localScale = new Vector3(0, 0, 0);
	}

	public void EnableBack() {
		// Enable the button
		this.gameObject.transform.Find ("TopBar/Back").GetComponent<Button>().interactable = true;

		// Change its scale back to 1
		this.gameObject.transform.Find ("TopBar/Back").transform.localScale = new Vector3(1, 1, 1);
	}

}
