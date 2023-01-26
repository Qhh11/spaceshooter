using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossAsteroid : Asteroid {

	private int respawnAmount = 1;
	[Header("Data")]
	[SerializeField]
	private int health = 5;
	[SerializeField]
	private GameObject smokeParticles;
	[SerializeField]
	private GameObject deathExplosionParticles;
	[Header("UI Elements")]
	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private Image[] hearts;
	[SerializeField]
	private Sprite emptyHeart;
	[SerializeField]
	private Sprite filledHeart;

	protected void Start() {
		UpdateUI();
		BossFightManager.instance.activeBosses.Add(this);
	}

	protected new void Update() {
		base.Update();

		var canvasTransform = canvas.transform;
		canvasTransform.LookAt(cam!.transform);
	}

	public override void OnRespawnRandomize() {
		if (cam is null || cam.IsDestroyed() || respawnAmount == 0) return;
		respawnAmount--;
		var trans = gameObject.transform;

		var playingFieldZ = -cam!.transform.position.z;
		var topCenter = cam!.ViewportToWorldPoint(new Vector3(0.5f, 1, playingFieldZ));
		var bottomCenter = cam!.ViewportToWorldPoint(new Vector3(0.5f, 0, playingFieldZ));

		var capsCol = GetComponent<CapsuleCollider>();
		topCenter += new Vector3(0, capsCol.height * (trans.localScale.y + 0.5f), 0);

		trans.position = topCenter;
		flyDirection = (bottomCenter - topCenter).normalized;

		// enemyRotationSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed) / 4;
		// enemyRotationSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);

		#region Randomize other data

		enemySpeed = Random.Range(minSpeed, maxSpeed);
		enemyScale = Random.Range(minScale, maxScale);
		enemyRotationSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedY = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);

		#endregion
	}

	private void OnTriggerEnter(Collider other) {
		var enemy = other.GetComponent<Asteroid>();

		if (enemy == null || enemy.fallingDown) return;
		enemy.PlayExplosion();
		enemy.OnRespawnRandomize();

		if (TakeDamage()) OnDeath();
	}

	/// <summary>
	/// Makes the boss take damage, i.e. reduces its health by 1 and updates the UI
	/// </summary>
	/// <returns>true if the boss has died, false if it is still alive afterwards</returns>
	private bool TakeDamage() {
		if (health <= 1) return true;
		health--;
		UpdateUI();

		var rx = Random.Range(-0.25f, 0.25f) * enemyScale;
		var ry = Random.Range(-0.4f, 0.4f) * enemyScale;
		var smoke = Instantiate(smokeParticles, Vector3.zero, Quaternion.identity);
		var trans = smoke.transform;
		var ownTrans = transform;
		var ownRot = ownTrans.rotation;
		trans.SetParent(ownTrans);
		trans.localPosition = new Vector3(rx, ry, 1);
		trans.localScale *= enemyScale * 1.5f;
		trans.localRotation = Quaternion.Euler(ownRot.x - 90, ownRot.y, ownRot.z);
		return false;
	}

	private void OnDeath() {
		Instantiate(deathExplosionParticles, transform.position, Quaternion.identity);
		Destroy(gameObject);

		var gm = BossFightManager.instance;
		gm.activeBosses.Remove(this);
		if (gm.activeBosses.Count == 0) {
			gm.PanCameraIn();
		}
	}

	private void UpdateUI() {
		for (var i = 0; i < hearts.Length; i++) {
			hearts[i].sprite = health > i ? filledHeart : emptyHeart;
		}
	}

}