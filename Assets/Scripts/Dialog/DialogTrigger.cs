using System;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

	public Message[] messages;
	public Actor[] actors;
	private DialogueManager manager;

	public void StartDialogue() {
		manager ??= FindObjectOfType<DialogueManager>();
		manager.OpenDialogue(messages, actors);
	}

	public void AddCallback(Action action) {
		manager ??= FindObjectOfType<DialogueManager>();
		manager.callbacks += action;
	}

}

[Serializable]
public class Message {

	public int actorId;
	public string message;

}

[Serializable]
public class Actor {

	public string name;
	public Sprite sprite;

}