using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Start Boss Fight", fileName = "BossFightDefinition", order = 1)]
public class StartBossFight : SpawnAsteroidWaveElement {

	public GameObject poofParticles;
	private BossFightManager bossFightManager; 
	public override void BeforeSpawn(Action spawnTask) {
		bossFightManager.playBGM.Pause();
		bossFightManager.bossBGM.Play();
		var dialogTrigger = FindObjectOfType<DialogTrigger>();
		dialogTrigger.gameObject.transform.position += new Vector3(0, 100, 0);
		WaveManager.instance.isTrackingTime = false;

		var gm = GameManager.Instance;
		gm.isMovingAsteroids = false;
		gm.Meteors.ForEach(a => {
			Instantiate(poofParticles, a.transform.position, Quaternion.identity);
			a.Hide();
		});

		void OnPanOutFinished() {
			HeartPowerUpManager.instance.UpdateSpawnPosition();
			gm.SetWaveSpawnablePositions();
			gm.Meteors.ForEach(a => {
				a.Show();
				a.OnRespawnRandomize();
				gm.isMovingAsteroids = true;
				gm.playersInvincible = false;
			});
			GameManager.Instance.playersCanMove = true;
			spawnTask.Invoke();
		}

		dialogTrigger.AddCallback(() => {
			BossFightManager.instance.PanCameraOut(OnPanOutFinished);
		});
		gm.playersInvincible = true;
		GameManager.Instance.playersCanMove = false;
		dialogTrigger.StartDialogue();
	}

	public override void AfterSpawn(GameObject obj) {
		Debug.Log("Starting Boss Fight");
	}

}