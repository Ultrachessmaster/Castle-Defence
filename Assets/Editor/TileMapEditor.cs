using UnityEditor;
using UnityEngine;
using System.Collections;
[CustomEditor(typeof(TiledMapGeneration))]
public class TileMapEditor : Editor {

	public override void OnInspectorGUI () {
		//base.OnInspectorGUI ();
		DrawDefaultInspector ();

		if(GUILayout.Button("Rebuild Tile Map")) {
			TiledMapGeneration map = (TiledMapGeneration)target;
			map.BuildMesh();
		}
	}

	void OnSceneGUI () {
		/*int controlID = GUIUtility.GetControlID (FocusType.Passive);
		if (Event.current.type == EventType.mouseDown) {
			RaycastHit rh;
			TiledMapGeneration tiledMapG = (TiledMapGeneration)target;
			Collider col = tiledMapG.GetComponent <Collider> ();
			Debug.Log ("Collider");
			Event e = Event.current;

			Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
			Vector2 mouseToTile = r.origin - tiledMapG.gameObject.transform.position;
			mouseToTile = new Vector2 (Mathf.FloorToInt (mouseToTile.x/tiledMapG.tileSize), Mathf.FloorToInt (mouseToTile.y/tiledMapG.tileSize));

			Debug.Log (mouseToTile);
		}*/
	}
}
