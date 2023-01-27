using UnityEngine;

public class HeartPowerUpManager : MonoBehaviour {

	public static HeartPowerUpManager instance;
	[SerializeField]
	private AudioSource pickupSound;
	[SerializeField]
	private GameObject heartPrefab;
	public float heartCooldown;
	public GameObject heart;
	[SerializeField]
	private Camera cam;
	private Vector3 spawnPosition;

	private void Start() {
		instance = this;
		cam = Camera.main;
		UpdateSpawnPosition();
	}

	private void Update() {
		if (heartCooldown > 0 && heart is null) {
			heartCooldown -= Mathf.Min(heartCooldown, Time.deltaTime);
		}

		var hasTakenDamage = GameManager.Player1.HasTakenDamage() || GameManager.Player2.HasTakenDamage();
		if (hasTakenDamage && heartCooldown == 0 && heart == null && !BossFightManager.instance.IsPanning()) {
			heart = Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
			var comp = heart.GetComponent<heart>();
			comp.sound = pickupSound;
		}
	}

	public void UpdateSpawnPosition() {
		spawnPosition = cam!.ViewportToWorldPoint(new Vector3(-0.1f, 0.8f, -cam.transform.position.z));
	}

}