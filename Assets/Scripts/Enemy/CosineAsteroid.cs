using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosineAsteroid : Asteroid {

	[Header("Cosine specific")]
	[SerializeField]
	private float _amplitude = .75f;
	[SerializeField]
	private float _frequency = 15f;

	void Start() {
		cam = Camera.main;
		OnRespawnRandomize();
	}

	new void Update() {
		if (!GameManager.Instance.isMovingAsteroids) return;
		
		var trans = gameObject.transform;
		var enemySpeedNormalized = enemySpeed * Time.deltaTime;
		var translation = (fallingDown ? cosMovement(Vector3.down) : Vector3.up) * enemySpeedNormalized;
		translation += flyDirection * enemySpeedNormalized;

		trans.Translate(translation, Space.World);
		trans.Rotate(new Vector3(enemyRotationSpeedX, enemyRotationSpeedY, enemyRotationSpeedZ));
		trans.localScale = Vector3.one * enemyScale;
	}

	Vector3 cosMovement(Vector3 pos) {
		pos.x += Mathf.Cos(Time.time * _frequency) * _amplitude;
		return pos;
	}

}