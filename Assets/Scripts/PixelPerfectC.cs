using UnityEngine;
using System.Collections;

public class PixelPerfectC : MonoBehaviour {

	public int pixelsToUnits;
	public int zoom;

	void Awake () {
		this.GetComponent <Camera> ().orthographicSize = Screen.height/(2 * (float)pixelsToUnits * (float)zoom);
	}
}
