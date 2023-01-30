using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Image actorImage;
	public TextMeshProUGUI actorName;
	public TextMeshProUGUI messageText;
	public RectTransform backgroundBox;

	public String textToDisplay;

	private Message[] currentMessages;
	private Actor[] currentActors;
	private int activeMessage = 0;
	public static bool isActive = false;
	public static bool isDone = false;

	[SerializeField] private AudioSource _audioSource;
	//[SerializeField] private AudioClip nya;

	private Message messageToDisplay;
	private Actor actorToDisplay;

	private Animator _anim;

	public Action callbacks = () => {};
	private DialogTrigger dialogTrigger;

	// Start is called before the first frame update

	public void OpenDialogue(DialogTrigger trigger, Message[] messages, Actor[] actors) {
		if (isActive) return;
		dialogTrigger = trigger;
		Debug.Log("Conversation started!");
		GameManager.Instance.playersCanMove = false;
		WaveManager.instance.isTrackingTime = false;
		GameManager.Instance.isMovingAsteroids = false;
		currentMessages = messages;
		currentActors = actors;
		activeMessage = 0;
		isActive = true;
		_anim.SetTrigger("OpenPanel");
		DisplayMessage();
	}

	void Start() {
		_anim = GetComponent<Animator>();
		//_audioSource = GetComponent<AudioSource>();
	}

	void DisplayMessage() {
		isDone = false;
		messageText.text = "";
		messageToDisplay = currentMessages[activeMessage];

		actorToDisplay = currentActors[messageToDisplay.actorId];
		actorImage.sprite = actorToDisplay.sprite;
		actorName.text = actorToDisplay.name;

		StartCoroutine(AppendText(messageToDisplay.message));
	}

	public void NextMessage() {
		if (++activeMessage < currentMessages.Length) {
			DisplayMessage();
		} else {
			CloseDialogue();
		}
	}

	private void PlaySound()
	{
		_audioSource.pitch = UnityEngine.Random.Range(1.3f,1.7f);
		_audioSource.PlayOneShot(_audioSource.clip);
	}

	IEnumerator AppendText(String msg) {
		// dialogTrigger.typingSound.Play();
		textToDisplay = "";
		foreach (Char c in msg) {
			textToDisplay += c;
			messageText.text = textToDisplay;
			if (c != ' ') PlaySound();
				yield return new WaitForSeconds(WaitTime.getWaitTime(c));
			if (isDone) break;
		}

		messageText.text = msg;
		isDone = true;
	}

	void CloseDialogue() {
		isActive = false;
		GameManager.Instance.playersCanMove = true;
		WaveManager.instance.isTrackingTime = true;
		GameManager.Instance.isMovingAsteroids = true;
		_anim.SetTrigger("ClosePanel");
		callbacks.Invoke();
		callbacks = () => { };
		Debug.Log("Conversation ended!");
		// dialogTrigger.typingSound.Pause();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) && isActive == true) {
			if (!isDone) {
				isDone = true;
				return;
			}

			NextMessage();
		}
	}

}

public static class WaitTime {

	public const float Standard = 0.05f;
	public const float Extended = 0.2f;
	public const float SemiExtended = 0.1f;
	public const float Cutscene = 2f;

	public static float getWaitTime(Char character) {
		switch (character) {
			case '.' or '!':
				return Extended;
			case ',':
				return SemiExtended;
			default:
				return Standard;
		}
	}

}