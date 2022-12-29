using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public static int Score = 0;
	public static int Health = 3;

	private Vector2 inputMovement;

	private Camera cam;
	private new Renderer renderer;
	[Header("Basic Player Data")]
	[SerializeField]
	private Vector2 respawnPoint = Vector2.zero;
	[SerializeField]
	private float playerSpeed = 0.2f;
	[SerializeField]
	private float moveRotation = 35f;
	[SerializeField]
	private ParticleSystem explosion;
	[SerializeField]
	private State state;

	[Header("UI Elements")]
	[SerializeField]
	private TMP_Text scoreText;
	[SerializeField]
	private TMP_Text healthText;
	[SerializeField]
	private Image transitionScreen;

	[Header("Transition")]
	[SerializeField]
	private float transitionDuration;
	private float currentTransitionTime;

	[Header("Projectiles")]
	[SerializeField]
	private Transform laserCannonTransform;
	[SerializeField]
	private GameObject laserPrefab;
	[SerializeField]
	private GameObject clickPlane;

	// Start is called before the first frame update
	void Start() {
		Score = 0;
		Health = 3;
		cam = Camera.main;
		renderer = GetComponent<Renderer>();
		UpdateUI();
	}

	// Update is called once per frame
	void Update() {
		HandleKeyInput();

		if (state == State.Death) {
			currentTransitionTime += Time.deltaTime;

			var color = transitionScreen.color;
			transitionScreen.color =
				new Color(color.r, color.g, color.b, (currentTransitionTime / transitionDuration) * 1.25f);
		}
	}

	// Siehe Component "Player Input"
	// onMove wird aufgerufen, wenn das zugehörige Input-Device einen Input (Joystick / WASD) liefert.
	public void onMove(InputAction.CallbackContext context)
	{
		inputMovement = context.ReadValue<Vector2>();
	}

	private void HandleKeyInput() {
		if (state != State.Playing) return;
		var trans = transform;
		var pos = trans.position;

		// var axis = Input.GetAxis("Horizontal");
		var axis = inputMovement.x;
		var rotationY = -axis * moveRotation;
		var moveAmount = axis * playerSpeed * Time.deltaTime;

		pos += Vector3.right * moveAmount;
		trans.position = pos;
		trans.rotation = Quaternion.Euler(0, rotationY, 0);

		var posOnScreen = cam.WorldToScreenPoint(pos);
		if (posOnScreen.x < 0) {
			var screenMax = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, 0));
			trans.position = new Vector3(screenMax.x, pos.y, pos.z);
		}

		if (posOnScreen.x > cam.pixelWidth) {
			var screenMax = cam.ScreenToWorldPoint(Vector3.zero);
			trans.position = new Vector3(screenMax.x, pos.y, pos.z);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(ray, out _, 150f)) return;
			Vector3 point = ray.GetPoint(50);
				
			Debug.Log($"Clicked at {Input.mousePosition} which is {point} as world point");
			var cannonPos = laserCannonTransform.position;
			var rotation = Quaternion.FromToRotation(Vector3.up, cannonPos - point);
			var obj = Instantiate(laserPrefab, cannonPos, Quaternion.identity);
			// ReSharper disable once Unity.PerformanceCriticalCodeInvocation Required to set shooter onto element
			obj.GetComponent<Projectile>().shooter = this;
			obj.transform.rotation = rotation;
		}
	}

	private void OnTriggerEnter(Collider other) {
		var enemy = other.GetComponent<Enemy>();

		if (enemy == null) return;
		enemy.PlayExplosion();
		enemy.SetSpeedAndPosition();

		Health -= 1;
		enemy.ChangeSpeed(-2f);
		UpdateUI();
		StartCoroutine(Death());
	}

	// TODO: Für 2 Schiffe entsprechend sinnvoll implementieren.
	private IEnumerator Death() {
		state = Health > 0 ? State.Respawning : State.Death;
		Instantiate(explosion, transform.position, Quaternion.identity);
		renderer.enabled = false;

		yield return new WaitForSeconds(transitionDuration);

		if (Health <= 0) {
			SceneManager.LoadScene("LoseScene");
		} else {
			transform.position = new Vector3(respawnPoint.x, respawnPoint.y, 0);
			renderer.enabled = true;
			state = State.Playing;
		}
	}

	public void UpdateUI() {
		scoreText.text = "Score: " + Score;
		healthText.text = Health + (Health != 1 ? " Lives" : " Life");
	}

}

public enum State {

	Playing,
	Respawning,
	Death

}