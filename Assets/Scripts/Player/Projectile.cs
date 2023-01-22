using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField]
	private float laserSpeed = 10;
	[SerializeField]
	private GameObject clickPlane;
	public Player shooter;

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update() {
		transform.Translate(Vector3.down * (laserSpeed * Time.deltaTime), Space.Self);

		if (transform.position.y > clickPlane.transform.position.y) {
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other) {
		var enemy = other.GetComponent<Asteroid>();

		if (enemy == null) return;
		enemy.PlayExplosion();
		enemy.ChangeSpeed(0.1f);
		enemy.OnRespawnRandomize();
		Destroy(gameObject);
	}

}