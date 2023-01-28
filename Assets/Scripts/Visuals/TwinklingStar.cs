using System.Collections;
using UnityEngine;

public class TwinklingStar : MonoBehaviour {

	public Light starLight;
	public float animationSpeed = 4f;
	[Range(0f, 1f)]
	public float twinkleChance = 0.2f;
	public AnimationCurve twinkleCurve = new();
	private float animationLength;
	private float maxIntensity;
	private bool isTwinkling;

	// Start is called before the first frame update
	void Start() {
		animationLength = twinkleCurve[twinkleCurve.length - 1].time;
		maxIntensity = starLight.intensity;
	}

	// Update is called once per frame
	void Update() {
		if (!isTwinkling && Random.Range(0f, 1f) < twinkleChance) {
			StartCoroutine(DoTwinkle());
		}
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

}