using UnityEngine;

public abstract class DestroyingWaveElement : WaveElement {

	public SpawningWaveElement template;

	public abstract bool IsValid(GameObject obj);

	public virtual void PerformDestruction(GameObject obj) {
		Destroy(obj);
	}

	public void Invoke(int amount, Transform parent) {
		int val = amount;
		foreach (var child in parent) {
			if (child is not Transform t) continue;
			var obj = t.gameObject;
			if (!IsValid(obj)) continue;

			PerformDestruction(obj);
			
			val--;
			if (val <= 0) break;
		}
	}

}