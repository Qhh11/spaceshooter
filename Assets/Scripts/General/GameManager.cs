using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance;
	public readonly List<Asteroid> Meteors = new();

	public static Player Player1;
	public static Player Player2;

	[SerializeField] private Player _player1;
	[SerializeField] private Player _player2;

	#region WaveSpawnable positions

	private Vector3 leftCorner;
	private Vector3 otherLeftCorner;
	private Vector3 rightCorner;
	private Vector3 otherRightCorner;
	private Vector3 cameraCenter;

	#endregion

	/*
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
	*/
	
	public Vector3 GetLeftCorner() => leftCorner;
	public Vector3 GetOtherLeftCorner() => otherLeftCorner;
	public Vector3 GetRightCorner() => rightCorner;
	public Vector3 GetOtherRightCorner() => otherRightCorner;
	public Vector3 GetCameraCenter() => cameraCenter;

	void Start()
	{
		if (Player1 == null) Player1 = _player1;
		if (Player2 == null) Player2 = _player2;
		if (Instance == null) Instance = this;
		
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
	

	public IEnumerator HealBothPlayers()
	{
		for (int i = 0; i < 3; i++)
		{
			Player1.Heal();
			Player2.Heal();
			yield return new WaitForSeconds(0.1f);
		}
	}

	public bool IsBothDead()
	{
		return (Player1.shipState == ShipState.Destroyed && Player2.shipState == ShipState.Destroyed);
	}

}