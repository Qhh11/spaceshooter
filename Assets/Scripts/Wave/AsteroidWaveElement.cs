using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Asteroid", fileName = "AsteroidWaveElement", order = 2)]
public class AsteroidWaveElement : WaveElement {

	[Header("Asteroid Data")]
	[Min(0.01f)]
	public float minSpeed = 0.01f;
	[Min(0.01f)]
	public float maxSpeed = 0.01f;
	[Min(0f)]
	public float minRotationSpeed = 0;
	[Min(0f)]
	public float maxRotationSpeed = 0;
	[Min(0.01f)]
	public float minScale = 0.01f;
	[Min(0.01f)]
	public float maxScale = 0.01f;

}