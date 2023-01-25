using UnityEngine;

public class BossAsteroid : Asteroid {

	public override void OnSpawned(Vector3 spawnPos, WaveElement waveElement) {
		spawnPosition = GameManager.Instance.GetTopCenter();

		if (waveElement is not SpawnAsteroidWaveElement awe) return;

		minSpeed = awe.minSpeed;
		maxSpeed = awe.maxSpeed;
		minRotationSpeed = awe.minRotationSpeed;
		maxRotationSpeed = awe.maxRotationSpeed;
		minScale = awe.minScale;
		maxScale = awe.maxScale;
	}

}