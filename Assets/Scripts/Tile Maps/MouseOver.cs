using UnityEngine;
using System.Collections;


[RequireComponent (typeof (TiledMapGeneration))]
public class MouseOver : MonoBehaviour {

	static Vector2 pos;

	void Update () {
		Vector2 posit = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		pos = new Vector2 (Mathf.Floor (posit.x), Mathf.Floor (posit.y));
	}

	public static Vector2 mouseCoor {
		get { return pos; }
	}
}
