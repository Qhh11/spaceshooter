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
	public WaveEvent[] events;
	[Tooltip("All the entries that should get spawned at some point of this wave but are bound to a specific chance")]
	public ProbabilisticWaveEvent[] probabilisticEvents;
	private bool activated;

	public bool IsReady(WaveManager waveManager) {
		return !activated && waveManager.GetTrackedTime() <= afterGameStart;
	}

	public void Invoke(WaveManager waveManager) {
		if (!IsReady(waveManager)) return;

		foreach (var ev in events) {
			if (ev.IsReady(waveManager, this)) ev.Invoke(waveManager);
		}

		foreach (var ev in probabilisticEvents) {
			if (ev.IsReady(waveManager, this)) ev.Invoke(waveManager);
		}

		if (events.All(x => x.WasActivated())) activated = true;
	}

}

[Serializable]
public class WaveEvent {

	[Min(0)]
	[Tooltip("Seconds after which this object should be spawned (relative to the wave start)\n" +
			 "0 means exactly when the wave starts")]
	public float afterWaveStart;
	public WaveElement waveElement;
	protected bool activated;

	public bool IsReady(WaveManager waveManager, Wave wave) {
		return !WasActivated() && waveManager.GetTrackedTime() <= wave.afterGameStart + afterWaveStart;
	}

	public virtual void Invoke(WaveManager manager) {
		activated = true;

		#region Calculate random spawn position

		var gm = GameManager.Instance;
		var firstCorner = gm.GetLeftCorner();
		var secondCorner = gm.GetRightCorner();
		var rx = Random.Range(firstCorner.x, secondCorner.x);
		var ry = Random.Range(firstCorner.y, secondCorner.y);

		var position = new Vector3(rx, ry, 0);

		#endregion

		for (var i = 0; i < waveElement.spawnAmount; i++) {
			var obj = Object.Instantiate(waveElement.prefab, position, Quaternion.identity, manager.GetParent());
			var spawnable = obj.GetComponent<WaveSpawnable>();
			spawnable.OnSpawned(position, waveElement);
		}
	}

	public bool WasActivated() => activated;

}

[Serializable]
public class ProbabilisticWaveEvent : WaveEvent {

	[Tooltip("Chance of this wave event occurring\n" +
			 "0 means this event will never happen\n" +
			 "1 means this event will always happen\n" +
			 "Any value in between will represent the chance of occurence")]
	[Range(0, 1)]
	public float chance = 0.5f;

	public override void Invoke(WaveManager waveManager) {
		activated = true;
		var random = Random.Range(0f, 1f);
		Debug.Log($"random value for probabilistic event: {random}");
		if (chance == 0 || random > chance) return;
		base.Invoke(waveManager);
	}

}