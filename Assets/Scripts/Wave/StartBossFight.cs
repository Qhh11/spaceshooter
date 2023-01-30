using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Start Boss Fight", fileName = "BossFightDefinition", order = 1)]
public class StartBossFight : SpawnAsteroidWaveElement {

	public GameObject poofParticles;

	public override void BeforeSpawn(Action spawnTask) {
		var bfm = BossFightManager.instance;
		bfm.playBGM.Pause();
		bfm.bossBGM.time = 0;
		bfm.bossBGM.Play();
		WaveManager.instance.isTrackingTime = false;

		bfm.awaitingStart = true;
		bfm.StartBossFight(spawnTask);
	}

	public override void AfterSpawn(GameObject obj) {
		Debug.Log("Starting Boss Fight");
	}

}