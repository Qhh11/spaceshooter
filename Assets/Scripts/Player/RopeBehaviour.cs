using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class RopeBehaviour : MonoBehaviour {

	public LineRenderer lineRenderer;
	public Transform point0, point1;
	public GameObject spring;
	private float maxLength;

	[FormerlySerializedAs("springAnchor")]
	[FormerlySerializedAs("shipCenter")]
	public GameObject springAnchObj;

	[Range(1, 100)]
	public int numPoints = 50;
	[Range(0, 1)]
	public float fullyStretchedTolerance = 0.01f;
	private Vector3[] positions;
	private SpringJoint springJoint;
	private bool calculateAnchor = true;
	private Dictionary<Asteroid, Vector3> closest = new();
	private bool switchedToStretched;

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
		positions = new Vector3[numPoints];
		lineRenderer.positionCount = numPoints;
		springJoint = spring.GetComponent<SpringJoint>();

		var cam = Camera.main;
		var viewportToWorldPoint = cam!.ViewportToWorldPoint(Vector3.zero);
		maxLength = Mathf.Abs(viewportToWorldPoint.z) / 1.5f;
	}

	private void Update() {
		DrawCurve();
		MakeSpringAnchorPoint(point0.position, point1.position);
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
		Vector3 conAnchor = (p0 + p1) / 2;
		conAnchor.y -= Mathf.Max(0, maxLength - Vector3.Distance(p0, p1));
		springAnchObj.transform.position = conAnchor;

		Vector3 temp = new Vector3(conAnchor.x, spring.transform.position.y, spring.transform.position.z);
		spring.transform.position = temp;

		springJoint.damper = Mathf.Max(0, (maxLength - Vector3.Distance(p0, p1)));
	}

	IEnumerator Bounce(float meteorSize, float velocity) {
		calculateAnchor = false;
		springJoint.damper = 0;
		float bounceFactor = (meteorSize * 3 + velocity) * 0.1f;
		var newPos = springAnchObj.transform.position;

		StartCoroutine(Camera.main.GetComponent<CamShaker>()
		   .ShakeSteady(.1f, bounceFactor / 10));

		newPos -= new Vector3(0, bounceFactor, 0);
		springAnchObj.transform.position = newPos;
		yield return new WaitForSeconds(.1f);

		newPos += new Vector3(0, bounceFactor * 1.3f, 0);
		springAnchObj.transform.position = newPos;
		yield return new WaitForSeconds(.1f);

		calculateAnchor = true;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;

		foreach (var entry in closest) {
			if (!GameManager.Instance.Meteors.Contains(entry.Key)) continue;
			// Debug.Log($"Drawing line from {entry.Key.transform.position} to {entry.Value}");
			Gizmos.DrawLine(entry.Key.transform.position, entry.Value);
		}
	}

	private void CheckCollision() {
		var meteors = GameManager.Instance.Meteors;
		for (int i = 0; i < meteors.Count; i++) {
			var meteor = meteors[i];
			Vector3 closestPoint = Vector3.zero;
			float smallestDistance = float.MaxValue;
			foreach (var pos in positions) {
				if (meteor == null) break;
				var meteorPos = meteor.gameObject.transform.position;
				var distance = Vector3.Distance(pos, meteorPos);
				if (distance < smallestDistance) {
					smallestDistance = distance;
					closestPoint = pos;
				}

				if (distance < 1) {
					meteor.flyDirection = CalculateThrowbackAngle(point0.position, point1.position, Vector3.down);
					meteor.FallingDown = false;
					StartCoroutine(Bounce(meteor.GetEnemyScale(), meteor.GetEnemySpeed()));
				}
			}

			if (meteor == null) continue;
			closest[meteor] = closestPoint;
		}
	}

	private Vector3 CalculateThrowbackAngle(Vector3 ship1, Vector3 ship2, Vector3 incidentAngle) {
		Vector3 line = ship2 - ship1;
		Vector3 normal = new Vector3(-line.y, line.x).normalized;
		return Vector3.Reflect(incidentAngle, normal);
	}

	public float GetStretch() {
		if (maxLength == 0) return 0;
		return Mathf.Min(1f, Vector3.Distance(point0.position, point1.position) / maxLength);
	}

	public bool IsFullyStretched() {
		var isFullyStretched = 1 - GetStretch() < fullyStretchedTolerance;
		if (switchedToStretched && !isFullyStretched) {
			switchedToStretched = false;
		}

		return isFullyStretched;
	}

	public Vector3 GetRopeCenter() {
		return (point0.position + point1.position) / 2;
	}

	public float GetMaxLength() {
		return maxLength;
	}

}