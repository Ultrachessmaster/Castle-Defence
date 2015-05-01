using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TiledMapGeneration : MonoBehaviour {

	public int size_x = 100;
	public int size_y = 50;
	public float tileSize = 1.0f;

	public Texture2D terrainTiles;
	public int tileResolution = 16;
	private int numTilesPerRow;
	private int numRows;

	public bool isNotMainLayer;
	private TiledMap tm;

	public bool isStone;

	Color[][] ChopUpTiles () {

		Color[][] tiles = new Color[numTilesPerRow * numRows][];

		for (int y = 0; y < numRows; y++) {
			for (int x = 0; x < numTilesPerRow; x++) {
				tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution);
			}

		}
		return tiles;
	}

	void Awake () {
		BuildMesh ();
	}

	void BuildTexture () {
	
		numTilesPerRow = terrainTiles.width / tileResolution;
		numRows = terrainTiles.height / tileResolution;

		int texWidth = size_x * tileResolution;
		int textHeight = size_y * tileResolution;

		Texture2D texture = new Texture2D (texWidth, textHeight);

		Color[][] tiles = ChopUpTiles ();
		if (isNotMainLayer) {
			for (int y = 0; y < size_y; y++) {
				for (int x = 0; x < size_x; x++) {
					Color[] p = tiles[tm.getTileTexCoordinatesAt (x, y)[1] * numTilesPerRow + tm.getTileTexCoordinatesAt (x, y)[0]];
					texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
				}
			}
		} else {
			for (int y = 0; y < size_y; y++) {
				for (int x = 0; x < size_x; x++) {
					Color[] p = tiles[GameManager.tiledmap.getTileTexCoordinatesAt (x, y)[1] * numTilesPerRow + GameManager.tiledmap.getTileTexCoordinatesAt (x, y)[0]];
					texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
				}
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.Apply ();

		MeshRenderer mesh_renderer = GetComponent <MeshRenderer> ();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
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
			tm = new TiledMap (size_x, size_y, isStone, tileSize);
		} else {
			GameManager.tiledmap = new TiledMap (size_x, size_y, isStone, tileSize);
		}

		if (isStone && !isNotMainLayer) {
			BuildCollider ();
		}

		int numTiles = size_x * size_y;
		int numTris = numTiles * 2;

		//int vsize_x = size_x * 2;
		//int vsize_y = size_y * 2;

		int numVerts = numTiles * 4;

		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3];

		int x, y;

		TileVerts[] verts= new TileVerts[numTiles]; 

		for (y = 0; y < size_y; y++) {
			for(x = 0; x < size_x; x++) {
				verts[y * size_x + x] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize, size_x, size_y); 
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
			
		/*for (y = 0; y < vsize_y; y++) {
			for(x = 0; x < vsize_x; x++) {
				vertices[y * vsize_x + x] = new Vector3 (x * tileSize, y * tileSize);
				normals[y * vsize_x + x] = Vector3.back;
				uv[y * vsize_x + x] = new Vector2 ((float) x / size_x, (float) y / size_y);
			}
		}*/
		
		for (y = 0; y < size_y; y++) {
			for (x = 0; x < size_x; x++) {
				int squareIndex = y * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset] = (squareIndex) * 4 + 0;
				triangles[triOffset + 1] = (squareIndex) * 4 + 3;
				triangles[triOffset + 2] = (squareIndex) * 4 + 2;
				triangles[triOffset + 3] = (squareIndex) * 4 + 0;
				triangles[triOffset + 4] = (squareIndex) * 4 + 1;
				triangles[triOffset + 5] = (squareIndex) * 4 + 3;
			}
		}


		Mesh mesh = new Mesh ();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		mesh.name = isStone.ToString ();

		MeshFilter mesh_filter = GetComponent <MeshFilter> ();
		mesh_filter.mesh = mesh;

		BuildTexture ();
	}
}
