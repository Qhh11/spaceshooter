using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour {

	[Tooltip("Parent object used to group all asteroids under")]
	[SerializeField]
	private GameObject parentObject;
	[Tooltip("All specified waves")]
	[SerializeField]
	private Wave[] waves;
	private float trackedTime;
	private Camera cam;

	// Update is called once per frame
	void Update() {
		foreach (var wave in waves) {
			if (wave.IsReady(this)) {
				wave.Invoke(this);
			}
		}

		trackedTime += Time.deltaTime;
	}

	private void OnDrawGizmos() {
		var gm = GameManager.Instance;
		if (gm is null) return;
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(gm.GetLeftCorner(), gm.GetOtherLeftCorner());
		Gizmos.DrawLine(gm.GetOtherLeftCorner(), gm.GetRightCorner());
		Gizmos.DrawLine(gm.GetLeftCorner(), gm.GetOtherRightCorner());
		Gizmos.DrawLine(gm.GetOtherRightCorner(), gm.GetRightCorner());
	}

	public float GetTrackedTime() => trackedTime;
	public Transform GetParent() => parentObject.transform;

}

[Serializable]
public class Wave {

	[Min(0)]
	[Tooltip("Seconds after which this wave should start")]
	public float afterGameStart;
	[Tooltip("All the entries that should get spawned at some point of this wave")]
	public WaveEntry[] entries;
	private bool activated;

	public bool IsReady(WaveManager waveManager) {
		return !activated && waveManager.GetTrackedTime() <= afterGameStart;
	}

	public void Invoke(WaveManager waveManager) {
		if (!IsReady(waveManager)) return;

		foreach (var entry in entries) {
			if (entry.IsReady(waveManager, this)) {
				entry.Invoke(waveManager);
			}
		}

		if (entries.All(x => x.WasActivated())) activated = true;
	}

}

[Serializable]
public class WaveEntry {

	[Tooltip("The object to get spawned when this entry gets triggered")]
	public GameObject prefab;
	[Min(0)]
	[Tooltip("Seconds after which this object should be spawned (relative to the wave start)\n" +
			 "0 means exactly when the wave starts")]
	public float afterWaveStart;
	[Min(1)]
	[Tooltip("Amount of objects that should be spawned with this enty")]
	public int spawnedAmount = 1;
	[Header("Asteroid attributes")]
	public AsteroidAttributes attributes;
	
	private bool activated;

	public bool IsReady(WaveManager waveManager, Wave wave) {
		return !WasActivated() && waveManager.GetTrackedTime() <= wave.afterGameStart + afterWaveStart;
	}

	public void Invoke(WaveManager manager) {
		activated = true;

		#region Calculate random spawn position

		var gm = GameManager.Instance;
		var firstCorner = gm.GetLeftCorner();
		var secondCorner = gm.GetRightCorner();
		var rx = Random.Range(firstCorner.x, secondCorner.x);
		var ry = Random.Range(firstCorner.y, secondCorner.y);

		var position = new Vector3(rx, ry, 0);

		#endregion

		for (var i = 0; i < spawnedAmount; i++) {
			var obj = Object.Instantiate(prefab, position, Quaternion.identity, manager.GetParent());
			var spawnable = obj.GetComponent<WaveSpawnable>();
			spawnable.OnSpawned(position, attributes);
		}
	}

	public bool WasActivated() => activated;

}

[Serializable]
public class AsteroidAttributes {

	[Min(0.01f)]
	public float minSpeed = 1f;
	[Min(0.01f)]
	public float maxSpeed = 3f;
	[Min(0f)]
	public float minRotationSpeed = 1f;
	[Min(0f)]
	public float maxRotationSpeed = 10f;
	[Min(0.01f)]
	public float minScale = 0.2f;
	[Min(0.01f)]
	public float maxScale = 1f;

}