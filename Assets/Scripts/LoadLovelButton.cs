using UnityEngine;
using System.Collections;

public class LoadLovelButton : MonoBehaviour {

	public string level;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnMouseDown () {
		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel (level);
		}
	}
}
