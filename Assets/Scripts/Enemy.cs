using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Camera cam;
	[SerializeField]
	private float minSpeed = 1f;
	[SerializeField]
	private float maxSpeed = 10f;
	[SerializeField]
	private float minRotationSpeed = 1f;
	[SerializeField]
	private float maxRotationSpeed = 10f;
	[SerializeField]
	private float minScale = 0.2f;
	[SerializeField]
	private float maxScale = 1f;
	[SerializeField]
	private ParticleSystem explosion;
	private float enemySpeed;
	private float enemyScale;
	private float enemyRotationSpeedX;
	private float enemyRotationSpeedY;
	private float enemyRotationSpeedZ;

	// Start is called before the first frame update
	void Start() {
		cam = Camera.main;
		SetSpeedAndPosition();
	}

	// Update is called once per frame
	void Update() {
		var trans = gameObject.transform;
		trans.Translate(Vector3.down * (enemySpeed * Time.deltaTime), Space.World);
		trans.Rotate(new Vector3(enemyRotationSpeedX, enemyRotationSpeedY, enemyRotationSpeedZ));
		trans.localScale = Vector3.one * enemyScale;
	}

	private void RandomizeData() {
		enemySpeed = Random.Range(minSpeed, maxSpeed);
		enemyScale = Random.Range(minScale, maxScale);
		enemyRotationSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedY = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);
	}

	public void SetSpeedAndPosition() {
		if (cam is null || cam.IsDestroyed()) return;

		var minX = cam.ScreenToWorldPoint(Vector3.zero).x;
		var maxX = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0)).x;
		var randomX = Random.Range(minX, maxX);

		gameObject.transform.position = new Vector3(randomX, cam.transform.position.y + 50);
		RandomizeData();
	}

	public void PlayExplosion() {
		Instantiate(explosion, transform.position, Quaternion.identity);
	}

	private void OnBecameInvisible() {
		SetSpeedAndPosition();
	}

	public void ChangeSpeed(float f) {
		minSpeed += f;
		maxSpeed += f;
	}

}