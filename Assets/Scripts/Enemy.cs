using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

	protected Camera cam;
	[SerializeField] protected float minSpeed = 1f;
	[SerializeField] protected float maxSpeed = 3f;
	[SerializeField] protected float minRotationSpeed = 1f;
	[SerializeField] protected float maxRotationSpeed = 10f;
	[SerializeField] protected float minScale = 0.2f;
	[SerializeField] protected float maxScale = 1f;
	[SerializeField] protected ParticleSystem explosion;
	protected float enemySpeed;
	protected float enemyScale;
	protected float enemyRotationSpeedX;
	protected float enemyRotationSpeedY;
	protected float enemyRotationSpeedZ;
	public Vector3 throwbackAngle = Vector3.zero;
	protected bool fallingDownVal = true;

	public Vector3 fallDirection = Vector3.down;
	public Vector3 throwbackDirection = Vector3.up;

	public bool fallingDown
	{
		set
		{
			fallingDownVal = value;
			if (!fallingDownVal)
			{
				GameManager.Meteors.Remove(this);
			}
			else
			{
				GameManager.Meteors.Add(this);
			}
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		cam = UnityEngine.Camera.main;
		SetSpeedAndPosition();
	}

	// Update is called once per frame
	void Update()
	{
		var trans = gameObject.transform;
		var enemySpeedNormalized = enemySpeed * Time.deltaTime;
		var translation = (fallingDownVal ? Vector3.down : Vector3.up) * enemySpeedNormalized;
		translation += throwbackAngle * enemySpeedNormalized;

		trans.Translate(translation, Space.World);
		trans.Rotate(new Vector3(enemyRotationSpeedX, enemyRotationSpeedY, enemyRotationSpeedZ));
		trans.localScale = Vector3.one * enemyScale;
	}

	protected void RandomizeData()
	{
		enemySpeed = Random.Range(minSpeed, maxSpeed);
		enemyScale = Random.Range(minScale, maxScale);
		enemyRotationSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedY = Random.Range(minRotationSpeed, maxRotationSpeed);
		enemyRotationSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);
	}

	public void SetSpeedAndPosition()
	{
		if (cam is null || cam.IsDestroyed()) return;

		var minX = cam.ScreenToWorldPoint(Vector3.zero).x;
		var maxX = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0)).x;
		var randomX = Random.Range(minX, maxX);

		gameObject.transform.position = new Vector3(randomX, cam.transform.position.y + 50);
		throwbackAngle = Vector3.zero;
		RandomizeData();
	}

	public void PlayExplosion()
	{
		Instantiate(explosion, transform.position, Quaternion.identity);
	}

	private void OnBecameVisible()
	{
		if (!GameManager.Meteors.Contains(this))
		{
			GameManager.Meteors.Add(this);
		}
	}

	private void OnBecameInvisible()
	{
		SetSpeedAndPosition();
		fallingDown = true;
	}

	public void ChangeSpeed(float f)
	{
		minSpeed += f;
		maxSpeed += f;
	}

	public float getEnemyScale()
	{
		return enemyScale;
	}

	public float getEnemySpeed()
	{
		return enemySpeed; 
	}

}