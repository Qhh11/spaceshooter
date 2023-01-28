using UnityEngine;

public class StarScatter : MonoBehaviour {

	public Camera cam;
	[Min(0)]
	public int spawnedAmount;
	public GameObject starPrefab;
	public Vector2 zArea;
	[HideInInspector]
	public Vector3 firstCorner;
	[HideInInspector]
	public Vector3 secondCorner;

	// Start is called before the first frame update
	void Start() {
		firstCorner = cam.ViewportToWorldPoint(new Vector3(-0.5f, 1.5f, zArea.x));
		secondCorner = cam.ViewportToWorldPoint(new Vector3(1.5f, -0.5f, zArea.y));

		for (var i = 0; i < spawnedAmount; i++) {
			var pos = new Vector3(Random.Range(firstCorner.x, secondCorner.x),
				Random.Range(firstCorner.y, secondCorner.y), Random.Range(firstCorner.z, secondCorner.z));
			Instantiate(starPrefab, pos, starPrefab.gameObject.transform.rotation, transform);
		}
	}

}