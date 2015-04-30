using UnityEngine;
using System.Collections;

public class TileVerts {

	public readonly Vector3[] verts = new Vector3[4];

	public readonly Vector2[] uv = new Vector2[4];

	public TileVerts (Vector2 position, float tileSize, float size_x, float size_y) {
		verts[0] = position;
		verts[1] = new Vector2 (position.x + tileSize, position.y);
		verts[2] = new Vector2 (position.x, position.y + tileSize);
		verts[3] = new Vector2 (position.x + tileSize, position.y + tileSize);

		uv[0] = new Vector2 (position.x / (size_x * tileSize), position.y / (size_y * tileSize));
		uv[1] = new Vector2 ((position.x + tileSize) / (size_x * tileSize), position.y / (size_y * tileSize));
		uv[2] = new Vector2 (position.x / (size_x * tileSize), (position.y + tileSize) / (size_y * tileSize));
		uv[3] = new Vector2 ((position.x + tileSize)  / (size_x * tileSize), (position.y + tileSize) / (size_y * tileSize));
	}
}
