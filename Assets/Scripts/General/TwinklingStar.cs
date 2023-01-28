using System.Collections;
using UnityEngine;

public class TwinklingStar : MonoBehaviour {

	public Light starLight;
	// public Vector2 startTimeRange;
	// private float currentTime;
	// [Range(1, 8)]
	// public int cosineCountMax = 1;
	// private int cosineCount;
	// public Vector2 flickeringIntervalRange = new(0.5f, 4f);
	// private float flickeringInterval;
	public float animationSpeed = 4f;
	[Range(0f, 1f)]
	public float twinkleChance = 0.2f;
	public AnimationCurve twinkleCurve = new();
	private float animationLength;
	private float maxIntensity;
	private bool isTwinkling;

	// Start is called before the first frame update
	void Start() {
		// currentTime = Random.Range(startTimeRange.x, startTimeRange.y);
		// cosineCount = Random.Range(1, cosineCountMax);
		// flickeringInterval = Random.Range(flickeringIntervalRange.x, flickeringIntervalRange.y);
		animationLength = twinkleCurve[twinkleCurve.length - 1].time;
		maxIntensity = starLight.intensity;
	}

	// Update is called once per frame
	void Update() {
		if (!isTwinkling && Random.Range(0f, 1f) < twinkleChance) {
			StartCoroutine(DoTwinkle());
		}

		// starLight.intensity = maxIntensity * ProduceTwinkleValue();
		// currentTime += Time.deltaTime;
	}

	private IEnumerator DoTwinkle() {
		isTwinkling = true;
		float time = 0;
		while (time < animationLength) {
			starLight.intensity = twinkleCurve.Evaluate(time) * maxIntensity;
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime * animationSpeed;
		}

		starLight.intensity = maxIntensity;
		isTwinkling = false;
	}

	// private float ProduceTwinkleValue() {
	// 	if (cosineCount <= 0) return 1;
	// 	var val = currentTime;
	// 	for (int i = 0; i < cosineCount; i++) {
	// 		val = (1 + Mathf.Cos(flickeringInterval * val)) / 2;
	// 	}
	//
	// 	return val;
	// }

}