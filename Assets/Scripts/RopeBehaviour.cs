using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeBehaviour : MonoBehaviour {

	public LineRenderer lineRenderer;
	public Transform point0, point1;
	public GameObject spring;
	public float maxLength = 10f;

	[Range(1, 100)]
	public int numPoints = 50;
	private Vector3[] positions;
	private SpringJoint _spring;
	private bool calculateAnchor = true;
	private Dictionary<Enemy, Vector3> closest = new();

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
		positions = new Vector3[numPoints];
		lineRenderer.positionCount = numPoints;
		_spring = spring.GetComponent<SpringJoint>();

		var cam = Camera.main;
		var viewportToWorldPoint = cam!.ViewportToWorldPoint(Vector3.zero);
		maxLength = Mathf.Abs(viewportToWorldPoint.z) * 1.5f;
	}

	private void Update() {
		DrawCurve();
		MakeSpringAnchorPoint(point0.position, point1.position);
		if (Input.GetKeyDown(KeyCode.O)) {
			StartCoroutine(Bounce());
		}

		CheckCollision();
	}

	private void DrawCurve() {
		for (int i = 1; i <= numPoints; i++) {
			float t = i / (float)numPoints;
			positions[i - 1] =
				CalculateQuadraticBezierPoint(t, point0.position, spring.transform.position, point1.position);
		}

		lineRenderer.SetPositions(positions);
	}

	private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
		// B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2 , 0 < t < 1

		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;

		Vector3 point = uu * p0;
		point += 2 * u * t * p1;
		point += tt * p2;

		return point;
	}

	private void MakeSpringAnchorPoint(Vector3 p0, Vector3 p1) {
		if (!calculateAnchor) return;
		Vector3 pos = (p0 + p1) / 2;
		var position = spring.transform.position;
		position = new Vector3(pos.x, position.y, position.z);
		spring.transform.position = position;
		pos.y -= maxLength - Vector3.Distance(p0, p1);
		if (pos.y > 0) {
			pos.y = 0;
		}

		_spring.connectedAnchor = pos;
	}

	IEnumerator Bounce() {
		calculateAnchor = false;
		var connectedAnchor = _spring.connectedAnchor;
		connectedAnchor += new Vector3(0, 10, 0);
		yield return new WaitForSeconds(.1f);
		connectedAnchor -= new Vector3(0, 10, 0);
		_spring.connectedAnchor = connectedAnchor;
		calculateAnchor = true;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;

		foreach (var entry in closest) {
			if (!GameManager.Meteors.Contains(entry.Key)) continue;
			Debug.Log($"Drawing line from {entry.Key.transform.position} to {entry.Value}");
			Gizmos.DrawLine(entry.Key.transform.position, entry.Value);
		}
	}

	private void CheckCollision() {
		var meteors = GameManager.Meteors;
		for (int i = 0; i < meteors.Count; i++) {
			var meteor = meteors[i];
			Vector3 closestPoint = Vector3.zero;
			float smallestDistance = float.MaxValue;
			foreach (var pos in positions) {
				var meteorPos = meteor.gameObject.transform.position;
				var distance = Vector3.Distance(pos, meteorPos);
				if (distance < smallestDistance) {
					smallestDistance = distance;
					closestPoint = pos;
				}

				if (distance < 1) {
					meteor.throwbackAngle = Vector3.Reflect(meteorPos, pos.normalized).normalized;
					meteor.fallingDown = false;
				}
			}

			closest[meteor] = closestPoint;
		}
	}

}