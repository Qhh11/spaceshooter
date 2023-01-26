using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Spawn Asteroid", fileName = "AsteroidWaveElement", order = 1)]
public class SpawnAsteroidWaveElement : SpawningWaveElement {

	[Header("Asteroid Data")]
	[Min(0.01f)]
	public float minSpeed = 0.01f;
	[Min(0.01f)]
	public float maxSpeed = 0.01f;
	[Min(0f)]
	public float minRotationSpeed;
	[Min(0f)]
	public float maxRotationSpeed;
	[Min(0.01f)]
	public float minScale = 0.01f;
	[Min(0.01f)]
	public float maxScale = 0.01f;
	[Min(0)]
	public int score;

}