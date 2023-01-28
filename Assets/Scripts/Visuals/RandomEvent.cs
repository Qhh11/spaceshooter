using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour {

	public Transform[] positions;
	public GameObject[] prefabs;
	[Min(0)]
	public float distanceThreshold = 20f;
	[Min(0.1f)]
	public float removalThreshold = 0.5f;
	[Min(4)]
	public float timeBetweenEvents;
	private float nextRandomEvent;
	private float currentTime;

	// Start is called before the first frame update
	void Start() {
		SetNextRandomEvent();
	}

	// Update is called once per frame
	void Update() {
		if (currentTime > nextRandomEvent) {
			StartCoroutine(StartNextEvent());
			SetNextRandomEvent();
			currentTime = 0;
		}

		currentTime += Time.deltaTime;
	}

	private void SetNextRandomEvent() {
		nextRandomEvent = Random.Range(4, timeBetweenEvents);
	}

	private IEnumerator StartNextEvent() {
		if (positions.Length < 2 || prefabs.Length < 1) yield break;
		List<Transform> remainingTransforms = new(positions);
		var start = PickRandomTransform(remainingTransforms);
		var end = PickRandomTransform(remainingTransforms);
		while (remainingTransforms.Count > 0 && Vector3.Distance(start.position, end.position) < distanceThreshold) {
			end = PickRandomTransform(remainingTransforms);
		}

		var obj = Instantiate(GetRandomPrefab(), start.position, Quaternion.identity, transform);
		var trans = obj.transform;
		trans.LookAt(end);
		
		var moveDir = (end.position - trans.position).normalized;
		var visualConfig = obj.GetComponent<VisualConfiguration>();
		var moveSpeed = visualConfig != null ? Random.Range(visualConfig.moveSpeed.x, visualConfig.moveSpeed.y) : 1;
		
		while (Vector3.Distance(trans.position, end.position) > removalThreshold) {
			yield return new WaitForEndOfFrame();
			trans.position += moveDir * (Time.deltaTime * moveSpeed);
		}

		Destroy(obj);
	}

	private GameObject GetRandomPrefab() {
		return prefabs[Random.Range(0, prefabs.Length)];
	}

	private Transform PickRandomTransform(List<Transform> transforms) {
		var picked = transforms[Random.Range(0, transforms.Count)];
		transforms.Remove(picked);
		return picked;
	}

}