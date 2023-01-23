using System.Collections.Generic;
using UnityEngine;

public class GameManager {

	public static GameManager Instance = new();
	public readonly List<Asteroid> Meteors = new();

	#region WaveSpawnable positions

	private readonly Vector3 leftCorner;
	private readonly Vector3 otherLeftCorner;
	private readonly Vector3 rightCorner;
	private readonly Vector3 otherRightCorner;
	private readonly Vector3 cameraCenter;

	#endregion

	public GameManager() {
		var cam = Camera.main;
		var camPos = cam!.transform.position;

		#region Setting WaveSpawnable positions

		var playingFieldZ = 0 - camPos.z;
		leftCorner = cam!.ViewportToWorldPoint(new Vector3(0, 3, playingFieldZ));
		rightCorner = cam!.ViewportToWorldPoint(new Vector3(1, 2, playingFieldZ));
		otherLeftCorner = new Vector3(rightCorner.x, leftCorner.y);
		otherRightCorner = new Vector3(leftCorner.x, rightCorner.y);
		cameraCenter = cam!.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, playingFieldZ));

		#endregion
	}

	public Vector3 GetLeftCorner() => leftCorner;
	public Vector3 GetOtherLeftCorner() => otherLeftCorner;
	public Vector3 GetRightCorner() => rightCorner;
	public Vector3 GetOtherRightCorner() => otherRightCorner;
	public Vector3 GetCameraCenter() => cameraCenter;

}