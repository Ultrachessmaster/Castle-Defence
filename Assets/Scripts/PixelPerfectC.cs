using UnityEngine;
using System.Collections;

public class PixelPerfectC : MonoBehaviour {

	public int pixelsToUnits;
	public int zoom;

	void Awake () {
		if (Screen.height < 640) {
			zoom = 1;
		}
		this.GetComponent <Camera> ().orthographicSize = Screen.height/(2 * (float)pixelsToUnits * (float)zoom);
	}
}
