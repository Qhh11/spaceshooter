using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class BossFightManager : MonoBehaviour {

	public static BossFightManager instance;
	[Header("UI transformation components")]
	[SerializeField]
	private CinemachineVirtualCamera virtualCam;
	private CinemachineTrackedDolly dolly;
	[SerializeField]
	private AnimationCurve movementCurve = new();
	[SerializeField]
	private float panTime;
	[SerializeField]
	private float targetPanTime;
	[SerializeField]
	private bool isPanning;

	private void Awake() {
		instance = this;
		dolly = virtualCam.GetCinemachineComponent<CinemachineTrackedDolly>();
		dolly.m_PositionUnits = CinemachinePathBase.PositionUnits.Normalized;
	}

	private void Update() {
		var position = movementCurve.Evaluate(panTime);
		position = Mathf.Min(1, Mathf.Max(0, position));
		dolly.m_PathPosition = position;
	}

	public void PanCameraOut() {
		if (isPanning) return;
		StartCoroutine(PanCamera(true));
	}

	public void PanCameraIn() {
		if (isPanning) return;
		StartCoroutine(PanCamera(false));
	}

	private IEnumerator PanCamera(bool panOut) {
		isPanning = true;
		targetPanTime = panOut ? movementCurve.keys.Max(k => k.time) : 0;
		while (Math.Abs(panTime - targetPanTime) > 0.0001f) {
			yield return new WaitForEndOfFrame();
			var delta = Time.deltaTime;

			if (panOut) {
				panTime += delta;
				if (panTime > targetPanTime) panTime = targetPanTime;
			} else {
				panTime -= delta;
				if (panTime < targetPanTime) panTime = targetPanTime;
			}
		}

		isPanning = false;
	}

}