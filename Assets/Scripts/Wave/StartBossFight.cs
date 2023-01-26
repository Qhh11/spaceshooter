
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Start Boss Fight", fileName = "BossFightDefinition", order = 1)]
public class StartBossFight : SpawnAsteroidWaveElement {

	public override void BeforeSpawn(Action spawnTask) {
		BossFightManager.instance.PanCameraOut(spawnTask);
	}

	public override void AfterSpawn(GameObject obj) {
		Debug.Log("Starting Boss Fight");
	}

}