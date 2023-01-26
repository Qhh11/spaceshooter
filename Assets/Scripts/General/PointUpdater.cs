using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointUpdater : MonoBehaviour {

	private TMP_Text text;

	// Start is called before the first frame update
	void Start() {
		text = GetComponent<TMP_Text>();
	}

	// Update is called once per frame
	void Update() {
		var score = GameManager.Instance.score;
		text.SetText($"{score} Point{(score == 1 ? "" : "s")}");
	}

}