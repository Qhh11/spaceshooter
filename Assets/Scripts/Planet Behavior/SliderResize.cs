using UnityEngine;

[ExecuteInEditMode]
public class SliderResize : MonoBehaviour {

	private Canvas canvas;
	private Rect rect;

	private void Awake() {
		canvas = GetComponentInParent<Canvas>();
		var rectTransform = GetComponent<RectTransform>();
		rect = rectTransform.rect;
	}

	// Update is called once per frame
	private void Update() {
		rect.width = canvas.pixelRect.width * 0.75f;
	}

}