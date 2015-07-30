using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TiledMapGeneration : MonoBehaviour {

	public int size_x = 100;
	public int size_y = 50;
	public float tileSize = 1.0f;

	public int sparcityOfStones;

	public Texture2D terrainTiles;
	public int tileResolution = 16;
	private int numTilesPerRow;
	private int numRows;

	public bool isNotMainLayer;
	private TiledMap tm;

	public bool isStone;

	void Awake () {
		BuildMesh ();
	}

	void BuildCollider () {
		try { 
			GameObject childColl = transform.GetChild (0).gameObject;
			DestroyImmediate (childColl);
		} catch {}
		GameObject collision = new GameObject ("Stone");
		if(isStone) {
			for (int x = 0; x < GameManager.tiledmap.width; x++) {
				for (int y = 0; y < GameManager.tiledmap.width; y++) {
					if(GameManager.tiledmap.tiles[x,y].type == Tile.castle) {
						BoxCollider2D collider =  (BoxCollider2D)collision.AddComponent <BoxCollider2D> ();

						collider.offset = new Vector2 (x * tileSize + tileSize / 2f, y * tileSize + tileSize / 2f);
						collider.size = new Vector2 (tileSize, tileSize);
					}
				}
			}
		}
		collision.transform.parent = gameObject.transform;
		collision.tag = "Stone";
		collision.layer = 8;
	}

	public void BuildMesh () {
		if (isNotMainLayer) {
			tm = new TiledMap (size_x, size_y, isStone, tileSize, sparcityOfStones);
		} else {
			tm = new TiledMap (size_x, size_y, isStone, tileSize, sparcityOfStones);
			GameManager.tiledmap = tm;
		}

		if (isStone && !isNotMainLayer) {
			BuildCollider ();
		}

		int tileCount = tm.width * tm.height;

		TileVerts[] verts = new TileVerts[tileCount];

		Vector3[] vertices = new Vector3[tileCount * 4];
		Vector3[] normals = new Vector3[tileCount * 4];
		Vector2[] uv = new Vector2[tileCount * 4];
		int[] triangles = new int[tileCount * 4 * 6];

		int x, y;

		for (x = 0; x < tm.width; x++) {
			for(y = 0; y < tm.height; y++) {
				verts[x * tm.height + y] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize, 
					tileResolution, terrainTiles.width, (int)tm[x, y].texCoor.x * tileResolution, (int)tm[x, y].texCoor.y * tileResolution); 
			}
		}

		int a = 0;
		for (int i = 0; i <  verts.Length; i++) {
			for (int j = 0; j < 4; j++) {
				vertices[a] = verts[i].verts[j];
				normals[a] = Vector3.back;
				uv[a] = verts[i].uv[j];
				a++;
			}
		}

		for (x = 0; x < tm.width; x++) {
			for (y = 0; y < tm.height; y++) {
				int squareIndex = x * tm.height + y;
				int triOffset = squareIndex * 6;
				triangles[triOffset] = (squareIndex) * 4 + 2;
				triangles[triOffset + 1] = (squareIndex) * 4 + 3;
				triangles[triOffset + 2] = (squareIndex) * 4 + 0;
				triangles[triOffset + 3] = (squareIndex) * 4 + 3;
				triangles[triOffset + 4] = (squareIndex) * 4 + 1;
				triangles[triOffset + 5] = (squareIndex) * 4 + 0;
			}
		}

		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		MeshFilter mesh_filter = GetComponent <MeshFilter> ();
		MeshRenderer mesh_renderer = GetComponent <MeshRenderer> ();
		mesh_filter.mesh = mesh;
		mesh_renderer.sharedMaterial.mainTexture = terrainTiles;
	}
}
