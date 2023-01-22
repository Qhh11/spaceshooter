using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Blank", fileName = "BlankWaveElement", order = 1)]
public class WaveElement : ScriptableObject {

	[Tooltip("The object to get spawned when this entry gets triggered")]
	public GameObject prefab;
	[Min(1)]
	[Tooltip("Amount of objects that should be spawned with this entry")]
	public int spawnAmount = 1;

}