using System;
using UnityEngine;

public abstract class WaveSpawnable : MonoBehaviour {

	public abstract void OnSpawned(Vector3 spawnPos);

}