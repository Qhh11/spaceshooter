using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListener : MonoBehaviour {

	[SerializeField]
	private string switchToScene;
	
	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update() {
		if (!Input.anyKey) return;
		
		SceneManager.LoadScene(switchToScene);
	}

}