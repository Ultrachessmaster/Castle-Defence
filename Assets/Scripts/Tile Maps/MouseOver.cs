using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent (typeof (TiledMapGeneration))]
public class MouseOver : MonoBehaviour {
	private bool android;
	private EventSystem eS;
	static Vector2 pos;
	static bool overTile = true;
	void Awake () {
		android = Application.platform == RuntimePlatform.Android;
		eS = GameObject.Find ("EventSystem").GetComponent <EventSystem> ();
	}
	void Update () {
		if (!android) {
			Vector2 posit = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos = new Vector2 (Mathf.Floor (posit.x), Mathf.Floor (posit.y));
			overTile = eS.currentSelectedGameObject == null;
		} else {
			if (Input.touchCount > 0) {
				Vector2 posit = (Vector2)Camera.main.ScreenPointToRay (Input.touches[0].position).origin;
				pos = new Vector2 (Mathf.Floor (posit.x), Mathf.Floor (posit.y));
				overTile = (!(Physics2D.Raycast ((Vector2)Camera.main.ScreenPointToRay (Input.touches[0].position).origin, Vector3.forward).collider.CompareTag ("Gui")));
			}
		}
	}

	public static Vector2 mouseCoor {
		get { return pos; }
	}
	public static bool isOverTile {
		get { return overTile; }
	}
}
