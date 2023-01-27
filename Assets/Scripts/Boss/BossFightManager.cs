using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class BossFightManager : MonoBehaviour {

	public static BossFightManager instance;
	[Header("Boss fight data")]
	[SerializeField]
	private GameObject bossDestroyParticles;
	public List<BossAsteroid> activeBosses = new();
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

	public float GetTargetPanTime() => targetPanTime;
	public bool IsPanning() => isPanning;

	public void PanCameraOut(Action taskAfterwards = null) {
		if (isPanning) return;
		StartCoroutine(PanCamera(true, taskAfterwards));
	}

	public void PanCameraIn(Action taskAfterwards = null) {
		if (isPanning) return;
		StartCoroutine(PanCamera(false, taskAfterwards, 2));
	}

	private IEnumerator PanCamera(bool panOut, Action taskAfterwards = null, float panSpeed = 1f) {
		isPanning = true;
		targetPanTime = panOut ? movementCurve.keys.Max(k => k.time) : 0;
		while (Math.Abs(panTime - targetPanTime) > 0.0001f) {
			yield return new WaitForEndOfFrame();
			var delta = Time.deltaTime * panSpeed;

			if (panOut) {
				panTime += delta;
				if (panTime > targetPanTime) panTime = targetPanTime;
			} else {
				panTime -= delta;
				if (panTime < targetPanTime) panTime = targetPanTime;
			}
		}

		isPanning = false;
		taskAfterwards?.Invoke();
	}

	public void OnBossFightEnd() {
		var gm = GameManager.Instance;
		PanCameraIn(() => {
			WaveManager.instance.isTrackingTime = true;

			gm.SetWaveSpawnablePositions();
			gm.Meteors.ForEach(a => {
				a.OnRespawnRandomize();
				a.Show();
			});
			gm.isMovingAsteroids = true;
			HeartPowerUpManager.instance.UpdateSpawnPosition();
		});

		var player1 = GameManager.Player1;
		var player2 = GameManager.Player2;

		player1.gameObject.transform.position = player1.GetStartPosition();
		player2.gameObject.transform.position = player2.GetStartPosition();

		gm.isMovingAsteroids = false;
		gm.Meteors.ForEach(a => {
			a.Hide();
			Instantiate(bossDestroyParticles, a.transform.position, Quaternion.identity);
		});
	}

	public bool IsCurrentlyEnding() {
		return GetTargetPanTime() == 0 && IsPanning();
	}

}