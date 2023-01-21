using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour {

	[SerializeField]
	private Wave[] waves;
	private float trackedTime;

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update() {
		trackedTime += Time.deltaTime;
	}

	public float GetTrackedTime() => trackedTime;

}

[Serializable]
public class Wave {

	[Min(0)]
	[Tooltip("Seconds after which this wave should start")]
	public float afterGameStart;
	[Tooltip("All the entries that should get spawned at some point of this wave")]
	public WaveEntry[] entries;
	private bool activated = false;

	public bool IsReady(WaveManager waveManager) {
		return !activated && waveManager.GetTrackedTime() <= afterGameStart;
	}

	public void Invoke(WaveManager waveManager) {
		if (!IsReady(waveManager)) return;

		var toActivate = entries.Where(x => x.IsReady(waveManager, this));
		foreach (var entry in toActivate) {
			entry.Invoke();
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
	[Tooltip("Amount of objects that should be spawned with this enty")]
	public int spawnedAmount;
	[Tooltip("Area in which the objects should get spawned")]
	public SpawnBoundingBox spawnBoundingBox;
	private bool activated = false;

	public bool IsReady(WaveManager waveManager, Wave wave) {
		return !WasActivated() && waveManager.GetTrackedTime() <= wave.afterGameStart + afterWaveStart;
	}

	public void Invoke() {
		activated = true;

		#region Calculate random spawn position

		var firstCorner = spawnBoundingBox.GetFirstCorner();
		var secondCorner = spawnBoundingBox.GetSecondCorner();
		var rx = Random.Range(firstCorner.x, secondCorner.x);
		var ry = Random.Range(firstCorner.y, secondCorner.y);

		Object.Instantiate(prefab);

		#endregion

	}

	public bool WasActivated() => activated;

}

[Serializable]
public class SpawnBoundingBox {

	[SerializeField]
	private Vector2 firstCorner;
	[SerializeField]
	private Vector2 secondCorner;

	public Vector2 GetFirstCorner() => new(
		Mathf.Min(firstCorner.x, secondCorner.x),
		Mathf.Min(firstCorner.y, secondCorner.y)
	);

	public Vector2 GetSecondCorner() => new(
		Mathf.Min(firstCorner.x, secondCorner.x),
		Mathf.Min(firstCorner.y, secondCorner.y)
	);

}