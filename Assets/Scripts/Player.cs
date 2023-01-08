using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public static int Score = 0;
	public static int Health = 3;

	public Vector2 InputMovement { get; private set; }

	protected Camera cam;
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
	[FormerlySerializedAs("state")]
	[SerializeField]
	private ShipState shipState;

	[Header("Rope and other Ship")]
	[SerializeField]
	private Player otherShip;
	[SerializeField]
	private RopeBehaviour rope;
	[SerializeField]
	public ShakeAnimation shakeAnimation;

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

		if (shipState == ShipState.Death) {
			currentTransitionTime += Time.deltaTime;

			var color = transitionScreen.color;
			transitionScreen.color =
				new Color(color.r, color.g, color.b, (currentTransitionTime / transitionDuration) * 1.25f);
		}
	}

	// Siehe Component "Player Input"
	// onMove wird aufgerufen, wenn das zugehörige Input-Device einen Input (Joystick / WASD) liefert.
	public void onMove(InputAction.CallbackContext context) {
		InputMovement = context.ReadValue<Vector2>();
	}

	private void HandleKeyInput() {
		if (shipState != ShipState.Playing) return;

		// var axis = Input.GetAxis("Horizontal");
		MoveShip();

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

	private void MoveShip() {
		var trans = transform;
		var pos = trans.position;

		var xAxis = InputMovement.x;
		var moveAmountX = xAxis * playerSpeed * Time.deltaTime;
		var yAxis = InputMovement.y;
		var moveAmountY = yAxis * playerSpeed * Time.deltaTime;
		var otherPos = otherShip.transform.position;
		var isLeftOfOther = pos.x < otherPos.x;
		var isBelowOther = pos.y < otherPos.y;
		var stretchStateX = GetRopeStretchState(isLeftOfOther, InputMovement.x, otherShip.InputMovement.x);
		var stretchStateY = GetRopeStretchState(isBelowOther, InputMovement.y, otherShip.InputMovement.y);

		pos += Vector3.right * moveAmountX;
		// Testing vertical movement
		pos += Vector3.up * moveAmountY;

		if (TriedToFlyOutOfView(pos)) {
			trans.rotation = Quaternion.identity;
			return;
		}

		if (rope.IsFullyStretched()) {
			if (stretchStateX == StretchState.Both || stretchStateY == StretchState.Both) {
				if (!shakeAnimation.active && shakeAnimation.playable) StartCoroutine(DoShake(isLeftOfOther));
				PerformRotation();
				pos = RestrictMovementInCircle(pos);
				trans.position = pos;
				return;
			}

			if (stretchStateX == StretchState.ByMyself || stretchStateY == StretchState.ByMyself) {
				pos -= Vector3.right * moveAmountX / 2;
				pos -= Vector3.up * moveAmountY / 2;
				shakeAnimation.playable = false;
			}

			if (stretchStateX == StretchState.ByOther || stretchStateY == StretchState.ByOther) {
				var otherMoveAmountX = otherShip.InputMovement.x * otherShip.playerSpeed * Time.deltaTime;
				var otherMoveAmountY = otherShip.InputMovement.y * otherShip.playerSpeed * Time.deltaTime;
				pos += Vector3.right * otherMoveAmountX / 2;
				pos += Vector3.up * otherMoveAmountY / 2;
				shakeAnimation.playable = false;
			}
		} else {
			shakeAnimation.playable = true;
		}

		if (stretchStateX == StretchState.Both || stretchStateY == StretchState.Both) {
			trans.position = RestrictMovementInCircle(pos);
		} else {
			trans.position = pos;
		}

		PerformRotation();
	}

	private Vector3 RestrictMovementInCircle(Vector3 pos) {
		var center = rope.GetRopeCenter();
		return center + Vector3.ClampMagnitude(pos - center, rope.GetMaxLength() / 2);
	}

	private void PerformRotation() {
		var axis = InputMovement.x;
		if (shakeAnimation.active) {
			shakeAnimation.CalculateRotation();
			transform.rotation = Quaternion.Euler(0, shakeAnimation.rotationY, 0);
		} else if (!shakeAnimation.playable) {
			transform.rotation = Quaternion.identity;
		} else {
			var rotationY = -axis * moveRotation;
			transform.rotation = Quaternion.Euler(0, rotationY, 0);
		}
	}

	private IEnumerator DoShake(bool isLeftOf) {
		shakeAnimation.Start(isLeftOf ? -1 : 1);
		yield return new WaitForSeconds(shakeAnimation.shakeDurationSecs);
		shakeAnimation.Stop();
	}

	private bool TriedToFlyOutOfView(Vector3 pos) {
		var posOnScreen = cam.WorldToScreenPoint(pos);
		return posOnScreen.x <= 40 || posOnScreen.x > cam.pixelWidth - 40 ||
			   posOnScreen.y <= 10 || posOnScreen.y > cam.pixelHeight - 10;
	}

	private StretchState GetRopeStretchState(bool negativeToOther, float ownMovement, float otherMovement) {
		// ReSharper disable twice ConvertIfStatementToSwitchStatement
		// Switch statements make it more unreadable

		if (negativeToOther) {
			if (ownMovement < 0 && otherMovement > 0) return StretchState.Both;
			if (ownMovement == 0 && otherMovement > 0) return StretchState.ByOther;
			if (ownMovement < 0 && otherMovement == 0) return StretchState.ByMyself;
		} else {
			if (ownMovement > 0 && otherMovement < 0) return StretchState.Both;
			if (ownMovement == 0 && otherMovement < 0) return StretchState.ByOther;
			if (ownMovement > 0 && otherMovement == 0) return StretchState.ByMyself;
		}

		return StretchState.NoOne;
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
		shipState = Health > 0 ? ShipState.Respawning : ShipState.Death;
		Instantiate(explosion, transform.position, Quaternion.identity);
		StartCoroutine(cam.GetComponent<CamShaker>().ShakeFadeout(2, .3f));
		renderer.enabled = false;

		yield return new WaitForSeconds(transitionDuration);

		if (Health <= 0) {
			SceneManager.LoadScene("LoseScene");
		} else {
			transform.position = new Vector3(respawnPoint.x, respawnPoint.y, 0);
			renderer.enabled = true;
			shipState = ShipState.Playing;
		}
	}

	public void UpdateUI() {
		scoreText.text = "Score: " + Score;
		healthText.text = Health + (Health != 1 ? " Lives" : " Life");
	}

}

public enum ShipState {

	Playing,
	Respawning,
	Death

}

public enum StretchState {

	NoOne,    // No one is currently stretching the rope
	ByMyself, // This ship is currently stretching the rope
	ByOther,  // The other ship is currently stretching the rope
	Both      // Both ships are stretching the rope

}