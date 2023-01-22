using UnityEngine;

public class SpawningWaveElement : WaveElement {

	[Tooltip("The object to get spawned when this entry gets triggered")]
	public GameObject prefab;
	[Tooltip("The tag that the object should get assigned")]
	public string assignedTag = "Untagged";

}