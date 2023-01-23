using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ShakeAnimation {

	public float shakeDurationSecs = 1;
	public float shakesPerSecond = 16;
	public float maxShakePower = 5;
	public AnimationCurve shakePowerDropOff = new(
		new Keyframe(0, 1), new Keyframe(1, 0));
	public bool playable = true;
	public bool active;
	public float rotationY;
	public float rotationStartDirection;
	private float dt;

	public void Start(float startDirection) {
		Stop(); // Kill previous run
		dt = 0;
		rotationY = 0;
		rotationStartDirection = startDirection;
		active = true;
		playable = false;
	}

	public void Stop() {
		active = false;
	}

	public void CalculateRotation() {
		var shakeInterval = 1 / shakesPerSecond;
		var shakeNr = dt / shakeInterval;
		var multiplier = maxShakePower * shakePowerDropOff.Evaluate(dt);
		rotationY = rotationStartDirection * Mathf.Cos(2 * Mathf.PI * shakeNr) * multiplier;
		
		dt += Time.deltaTime;
	}

}