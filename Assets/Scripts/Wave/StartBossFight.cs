
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Start Boss Fight", fileName = "BossFightDefinition", order = 1)]
public class StartBossFight : SpawnAsteroidWaveElement {

	public override void OnSpawned(GameObject obj) {
		Debug.Log("Starting Boss Fight");
		BossFightManager.instance.PanCameraOut();
	}

}