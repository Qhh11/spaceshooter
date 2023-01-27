using UnityEditor;
using UnityEngine;

public class ExitGame : MonoBehaviour {

	void DoExitGame() {
		#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		EditorApplication.isPlaying = false;
		#else
         Application.Quit();
		#endif
	}

}