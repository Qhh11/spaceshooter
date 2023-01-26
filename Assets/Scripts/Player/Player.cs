using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	private Vector2 InputMovement { get; set; }
	private Camera cam;

	[Header("Player Data")]
	[SerializeField]
	private int health;
	[SerializeField]
	private float playerSpeed = 0.2f;
	[SerializeField]
	private float moveRotation = 35f;
	[FormerlySerializedAs("state")]
	[SerializeField]
	public ShipState shipState;

	[Header("Parts of convoy")]
	[SerializeField]
	private Light shipLight;
	[SerializeField]
	private GameObject smokingShipParticles;
	private GameObject smokingShipParticlesInstantiated;
	[SerializeField]
	private Player otherShip;
	[SerializeField]
	private RopeBehaviour rope;
	[SerializeField]
	public ShakeAnimation shakeAnimation;

	[Header("UI Elements")]
	[SerializeField]
	private PlayerHealthUI healthUI;

	[SerializeField] private AudioSource audioSource;
	
	// TODO Use when losing
	// [SerializeField]
	// private Image transitionScreen;

	
	// [Header("Transition")]
	// [SerializeField]
	// private float transitionDuration;
	// private float currentTransitionTime;

	// Start is called before the first frame update
	void Start() {
		cam = Camera.main;
		GetComponent<Renderer>();
		healthUI.UpdateUI();
	}

	// Update is called once per frame
	void Update() {
		HandleKeyInput();
		HandleShipState();

		// TODO: Should get handled by GameManager in the future
		// if (shipState == ShipState.Destroyed) {
		// currentTransitionTime += Time.deltaTime;
		//
		// var color = transitionScreen.color;
		// transitionScreen.color =
		// 	new Color(color.r, color.g, color.b, (currentTransitionTime / transitionDuration) * 1.25f);
		// }
	}

	private void HandleShipState() {
		switch (shipState) {
			case ShipState.Alive: {
				if (!shipLight.enabled) shipLight.enabled = true;
				if (smokingShipParticlesInstantiated is not null) {
					Destroy(smokingShipParticlesInstantiated);
					smokingShipParticlesInstantiated = null;
				}

				break;
			}
			case ShipState.Destroyed: {
				if (shipLight.enabled) shipLight.enabled = false;
				if (smokingShipParticlesInstantiated is null) {
					var obj = Instantiate(smokingShipParticles, transform.position,
						Quaternion.Euler(-90, 0, 0), transform);
					smokingShipParticlesInstantiated = obj;
				}

				break;
			}
		}
	}

	// Siehe Component "Player Input"
	// onMove wird aufgerufen, wenn das zugehörige Input-Device einen Input (Joystick / WASD) liefert.
	public void onMove(InputAction.CallbackContext context) {
		if (shipState != ShipState.Alive) return;
		InputMovement = context.ReadValue<Vector2>();
	}

	private void HandleKeyInput() {
		MoveShip();
	}

	#region Movement Management

	private void MoveShip() {
		var trans = transform;
		var pos = trans.position;

		var xAxis = InputMovement.x;
		var moveAmountX = xAxis * playerSpeed * GetMoveSpeedModifier() * Time.deltaTime;
		var yAxis = InputMovement.y;
		var moveAmountY = yAxis * playerSpeed * GetMoveSpeedModifier() * Time.deltaTime;
		var otherPos = otherShip.transform.position;
		var isLeftOfOther = pos.x < otherPos.x;
		var isBelowOther = pos.y < otherPos.y;
		var stretchStateX = GetRopeStretchState(isLeftOfOther, InputMovement.x, otherShip.InputMovement.x);
		var stretchStateY = GetRopeStretchState(isBelowOther, InputMovement.y, otherShip.InputMovement.y);

		pos += Vector3.right * moveAmountX;
		// Testing vertical movement
		pos += Vector3.up * moveAmountY;

		if (TriedToFlyOutOfView(pos)) {
			trans.localRotation = Quaternion.identity;
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

				otherShip.transform.position = RestrictMovementInCircle(otherShip.transform.position);
			} else if (stretchStateX == StretchState.NoOne || stretchStateY == StretchState.NoOne) {
				pos = RestrictMovementInCircle(pos);
			}

			// if (stretchStateX == StretchState.ByOther || stretchStateY == StretchState.ByOther) {
			// 	var otherMoveAmountX = otherShip.InputMovement.x * otherShip.playerSpeed
			// 													 * otherShip.GetMoveSpeedModifier() * Time.deltaTime;
			// 	var otherMoveAmountY = otherShip.InputMovement.y * otherShip.playerSpeed
			// 													 * otherShip.GetMoveSpeedModifier() * Time.deltaTime;
			//
			// 	pos += Vector3.right * otherMoveAmountX / 2;
			// 	pos += Vector3.up * otherMoveAmountY / 2;
			// 	Debug.Log($"Getting pulled by {otherShip.gameObject.name} to {pos}");
			// 	shakeAnimation.playable = false;
			// }
		} else {
			pos = RestrictMovementInCircle(pos);
			shakeAnimation.playable = true;
		}

		trans.position = pos;

		PerformRotation();
	}

	private void PerformRotation() {
		var axis = InputMovement.x;
		if (shakeAnimation.active) {
			shakeAnimation.CalculateRotation();
			transform.localRotation = Quaternion.Euler(0, shakeAnimation.rotationY, 0);
		} else if (!shakeAnimation.playable || health == 0) {
			transform.localRotation = Quaternion.identity;
		} else {
			var rotationY = -axis * moveRotation;
			transform.localRotation = Quaternion.Euler(0, rotationY, 0);
		}

		healthUI.GetCanvas().transform.rotation = Quaternion.identity;
	}

	private Vector3 RestrictMovementInCircle(Vector3 pos) {
		var center = rope.GetRopeCenter();
		return center + Vector3.ClampMagnitude(pos - center, rope.GetMaxLength() / 2);
	}

	private bool TriedToFlyOutOfView(Vector3 pos) {
		var posOnScreen = cam.WorldToScreenPoint(pos);
		return posOnScreen.x <= 40 || posOnScreen.x > cam.pixelWidth - 40 ||
			   posOnScreen.y <= 10 || posOnScreen.y > cam.pixelHeight - 10;
	}

	private IEnumerator DoShake(bool isLeftOf) {
		shakeAnimation.Start(isLeftOf ? -1 : 1);
		yield return new WaitForSeconds(shakeAnimation.shakeDurationSecs);
		shakeAnimation.Stop();
	}

	#endregion

	#region Health Management

	public int GetHealth() => health;

	public void Heal(int amount = 1) {
		if (health >= 3) return;
		health += Math.Min(3, amount);
		healthUI.UpdateUI();

		if (health <= 0) {
			shipState = ShipState.Alive;
		}
	}

	public void TakeDamage(int amount = 1) {
		if (health <= 0) return;
		health -= Math.Max(0, amount);
		healthUI.UpdateUI();

		if (health <= 0) {
			shipState = ShipState.Destroyed;
		}
	}

	public float GetMoveSpeedModifier() => health / 3f;

	#endregion

	private StretchState GetRopeStretchState(bool negativeToOther, float ownMovement, float otherMovement) {
		// ReSharper disable twice ConvertIfStatementToSwitchStatement
		// Switch statements make it more unreadable

		if (otherShip.shipState == ShipState.Destroyed) {
			return StretchState.ByMyself;
		}

		if (shipState == ShipState.Destroyed) {
			return StretchState.ByOther;
		}

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
		var enemy = other.GetComponent<Asteroid>();

		if (enemy == null || health <= 0) return;
		enemy.PlayExplosion();
		enemy.OnRespawnRandomize();
		audioSource.Play();
		TakeDamage();
	}

	// TODO: Könnte man für die Lose Condition (Planet wird zerstört) verwenden
	// private IEnumerator Death() {
	// 	shipState = Health > 0 ? ShipState.Respawning : ShipState.Death;
	// 	Instantiate(explosion, transform.position, Quaternion.identity);
	// 	StartCoroutine(cam.GetComponent<CamShaker>().ShakeFadeout(2, .3f));
	// 	renderer.enabled = false;
	//
	// 	yield return new WaitForSeconds(transitionDuration);
	//
	// 	if (Health <= 0) {
	// 		SceneManager.LoadScene("LoseScene");
	// 	} else {
	// 		transform.position = new Vector3(respawnPoint.x, respawnPoint.y, 0);
	// 		renderer.enabled = true;
	// 		shipState = ShipState.Playing;
	// 	}
	// }

}

[Serializable]
public class PlayerHealthUI {

	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private Image[] hpImages = new Image[3];
	[SerializeField]
	private Sprite emptyIcon;
	[SerializeField]
	private Sprite filledIcon;
	[SerializeField]
	private Player player;

	public void UpdateUI() {
		var health = player.GetHealth();
		
		canvas.enabled = health != 3;
		
		for (var i = 1; i <= 3; i++) {
			var image = hpImages[i - 1];
			image.sprite = health >= i ? filledIcon : emptyIcon;
		}
	}

	public Canvas GetCanvas() {
		return canvas;
	}

}


public enum ShipState {

	Alive,
	Destroyed

}

public enum StretchState {

	NoOne,    // No one is currently stretching the rope
	ByMyself, // This ship is currently stretching the rope
	ByOther,  // The other ship is currently stretching the rope
	Both      // Both ships are stretching the rope

}