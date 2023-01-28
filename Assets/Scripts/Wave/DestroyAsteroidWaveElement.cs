using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Destroy Asteroid", fileName = "AsteroidWaveElement", order = 2)]
public class DestroyAsteroidWaveElement : DestroyingWaveElement {

	public override bool IsValid(GameObject obj) {
		var ws = obj.GetComponent<WaveSpawnable>();
		return ws != null && ws.objectTag == template.assignedTag;
	}

	public override void PerformDestruction(GameObject obj) {
		var asteroid = obj.GetComponent<Asteroid>();
		if (asteroid == null) return;
		
		asteroid.MarkForRemoval();
	}

}