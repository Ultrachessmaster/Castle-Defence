using UnityEngine;
using System.Collections;
public class TileVerts {
	public readonly Vector3[] verts = new Vector3[4];
	public readonly Vector2[] uv = new Vector2[4];

	public TileVerts (Vector2 position, float tileSize, int tileTexSize, int texSize, int uvx, int uvy) {
		verts[0] = new Vector3 (position.x, position.y, -(position.x + position.y)/500f);
		verts[1] = new Vector3 (position.x + tileSize, position.y, -(position.x + position.y)/500f);
		verts[2] = new Vector3 (position.x, position.y + tileSize, -(position.x + position.y)/500f);
		verts[3] = new Vector3 (position.x + tileSize, position.y + tileSize, -(position.x + position.y)/500f);

		uv[0] = new Vector2 ((float)uvx / (float)(texSize), (float)uvy / (float)(texSize));
		uv[1] = new Vector2 ((float)(uvx + tileTexSize) / (float)texSize, (float)uvy / (float)texSize);
		uv[2] = new Vector2 ((float)uvx / (float)texSize, (float)(uvy + tileTexSize) / (float)texSize);
		uv[3] = new Vector2 ((float)(uvx + tileTexSize) / (float)texSize, (float)(uvy + tileTexSize) / (float)texSize);
	}
}
