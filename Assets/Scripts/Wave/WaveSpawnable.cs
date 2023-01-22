using System;
using UnityEngine;

public abstract class WaveSpawnable : MonoBehaviour {

	public string objectTag;
	
	public abstract void OnSpawned(Vector3 spawnPos, WaveElement asteroidAttributes);

}