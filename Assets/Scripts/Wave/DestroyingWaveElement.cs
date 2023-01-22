using UnityEngine;

public abstract class DestroyingWaveElement : WaveElement {

	public SpawningWaveElement template;

	public abstract bool IsValid(GameObject obj);

}