using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Start Boss Fight", fileName = "BossFightDefinition", order = 1)]
public class StartBossFight : SpawnAsteroidWaveElement {

	public GameObject poofParticles;

	public override void BeforeSpawn(Action spawnTask) {
		var dialogTrigger = FindObjectOfType<DialogTrigger>();
		dialogTrigger.gameObject.transform.position += new Vector3(0, 100, 0);
		WaveManager.instance.isTrackingTime = false;
		
		var gm = GameManager.Instance;
		gm.isMovingAsteroids = false;
		gm.Meteors.ForEach(a => {
			Instantiate(poofParticles, a.transform.position, Quaternion.identity);
			a.OnRespawnRandomize();
		});
		dialogTrigger.AddCallback(() => {
			gm.isMovingAsteroids = true;
			BossFightManager.instance.PanCameraOut(spawnTask);
		});
		dialogTrigger.StartDialogue();
	}

	public override void AfterSpawn(GameObject obj) {
		Debug.Log("Starting Boss Fight");
	}

}