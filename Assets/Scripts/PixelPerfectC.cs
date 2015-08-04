using UnityEngine;
using System.Collections;

public class PixelPerfectC : MonoBehaviour {

	public int pixelsToUnits;
	public int zoom;
	private bool android;

	void Awake () {
		if (android = Application.platform == RuntimePlatform.Android) {
			zoom = Mathf.FloorToInt ((float)Screen.height/320f);
			if (zoom <= 2)
				zoom = 3;
		}
		this.GetComponent <Camera> ().orthographicSize = Screen.height/(2 * (float)pixelsToUnits * (float)zoom);
	}

	void LateUpdate () {
		if (android)
			transform.Translate (-Input.touches[0].deltaPosition/8);
	}
}