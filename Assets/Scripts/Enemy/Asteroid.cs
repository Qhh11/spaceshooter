using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : WaveSpawnable {

	protected Camera cam;
	[SerializeField]
	protected float minSpeed = 1f;
	[SerializeField]
	protected float maxSpeed = 3f;
	[SerializeField]
	protected float minRotationSpeed = 1f;
	[SerializeField]
	protected float maxRotationSpeed = 10f;
	[SerializeField]
	protected float minScale = 0.2f;
	[SerializeField]
	protected float maxScale = 1f;
	[SerializeField]
	protected ParticleSystem explosion;
	protected Vector3 spawnPosition;
	[SerializeField]
	protected float enemySpeed;
	protected float enemyScale;
	protected float enemyRotationSpeedX;
	protected float enemyRotationSpeedY;
	protected float enemyRotationSpeedZ;
	public Vector3 throwbackAngle = Vector3.zero;
	public bool fallingDownVal = true;

	public bool FallingDown {
		set {
			fallingDownVal = value;
			var meteors = GameManager.Instance.Meteors;
			if (!fallingDownVal) meteors.Remove(this);
			else meteors.Add(this);
		}
	}

	// Start is called before the first frame update
	public void Start() {
		cam = Camera.main;
		OnRespawnRandomize();
	}

	// Update is called once per frame
	void Update() {
		var trans = gameObject.transform;
		var enemySpeedNormalized = enemySpeed * Time.deltaTime;
		var translation = (fallingDownVal ? Vector3.down : Vector3.up) * enemySpeedNormalized;
		translation += throwbackAngle * enemySpeedNormalized;
		translation.z = 0;

		trans.Translate(translation, Space.World);
		trans.Rotate(new Vector3(enemyRotationSpeedX, enemyRotationSpeedY, enemyRotationSpeedZ));
		trans.localScale = Vector3.one * enemyScale;
	}

	public override void OnSpawned(Vector3 spawnPos, WaveElement waveElement) {
		spawnPosition = spawnPos;

		if (waveElement is AsteroidWaveElement awe) {
			minSpeed = awe.minSpeed;
			maxSpeed = awe.maxSpeed;
			minRotationSpeed = awe.minRotationSpeed;
			maxRotationSpeed = awe.maxRotationSpeed;
			minScale = awe.minScale;
			maxScale = awe.maxScale;
		}
	}

	public virtual void OnRespawnRandomize() {
		if (cam is null || cam.IsDestroyed()) return;
		var gm = GameManager.Instance;

		#region Randomizing Respawn Position

		var minX = gm.GetLeftCorner().x - spawnPosition.x;
		var maxX = gm.GetRightCorner().x - spawnPosition.x;
		var randomX = Random.Range(minX, maxX);
		var rx = Mathf.Max(gm.GetLeftCorner().x - 10, Mathf.Min(gm.GetRightCorner().x + 10, randomX));
		var minY = gm.GetRightCorner().y - 2;
		var maxY = gm.GetLeftCorner().y + 2;
		var randomY = Random.Range(minY, maxY);
		var ry = Mathf.Max(gm.GetRightCorner().y - 10, Mathf.Min(gm.GetLeftCorner().y + 10, randomY));
		var randomSpawnPos = new Vector3(rx, ry);

		gameObject.transform.position = randomSpawnPos;

		#endregion

		#region Move towards randomized offset of camera center

		throwbackAngle = (GameManager.Instance.GetCameraCenter() - randomSpawnPos).normalized;
		Debug.Log("Set throwback angle to " + throwbackAngle);

		#endregion

		#region Randomize other data

		enemySpeed = Random.Range(minSpeed, maxSpeed);
		enemyScale = Random.Range(minScale, maxScale);
		enemyRotationSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedY = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);

		#endregion
	}

	public void PlayExplosion() {
		Instantiate(explosion, transform.position, Quaternion.identity);
	}

	private void OnBecameVisible() {
		var meteors = GameManager.Instance.Meteors;
		if (!meteors.Contains(this)) meteors.Add(this);
	}

	private void OnBecameInvisible() {
		OnRespawnRandomize();
		FallingDown = true;
	}

	public void ChangeSpeed(float f) {
		minSpeed += f;
		maxSpeed += f;
	}

	public float GetEnemyScale() => enemyScale;

	public float GetEnemySpeed() => enemySpeed;

}