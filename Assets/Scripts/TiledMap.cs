using UnityEngine;
using System.Collections;

public class TiledMap {

	public int width;

	public int height;

	public Tile[,] tiles;

	public TiledMap (int width, int height, bool isStone, float tileSize) {
		this.width = width;
		this.height = height;

		tiles = new Tile[width, height];

		if (isStone) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					tiles[x,y] = new Tile (Tile.grass);
				}
			}

			generateStones ();
		}
		else {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					tiles[x,y] = new Tile (Tile.blank);
				}
			}

			generateFence ();
		}

	}

	void generateStones () {
		for (int x = 1; x < width - 1; x++) {
			for (int y = 0; y < height; y++) {
				if (Random.Range (0, 8) == 1) {
					tiles[x,y] = new Tile (Tile.castle);
				}
			}
		}
		tiles[3, 19] = new Tile (Tile.castle);
		tiles[3, 18] = new Tile (Tile.castle);
		tiles[3, 17] = new Tile (Tile.castle);
		for (int i = 3; i < 17; i++) {
			tiles[i,17] = new Tile (Tile.castle);
		}
		tiles[16, 19] = new Tile (Tile.castle);
		tiles[16, 18] = new Tile (Tile.castle);
		tiles[16, 17] = new Tile (Tile.castle);
		for (int x = 4; x < 16; x++) {
			for (int y = 19; y > 17; y--) {
				tiles[x,y] = new Tile (Tile.castlebackground);
			}
		}
		tiles[9, 17] = new Tile (Tile.castlebackground);
		tiles[10, 17] = new Tile (Tile.castlebackground);

		for (int x = 8; x < 12; x++) {
			for (int y = 16; y > 14; y--) {
				tiles[x, y] = new Tile (Tile.grass);
			}
		}
	}

	void generateFence () {
		for (int y = 0; y < height; y++) {
			int random = Random.Range (0, 2);
			if (random == 1 || random == 2) {
				tiles[0,y] = new Tile (Tile.fence1);
			}
			random = Random.Range (0, 2);
			if (random == 1 || random == 2) {
				tiles[width - 1, y] = new Tile (Tile.fence2);
			}
		}
			
		tiles[0, 0] = new Tile (Tile.fence1);
		tiles[0, 1] = new Tile (Tile.fence1);
		tiles[0, 2] = new Tile (Tile.fence1);
		tiles[0, 3] = new Tile (Tile.fence1);
		tiles[0, height - 1] = new Tile (Tile.fence1);
		tiles[0, height - 2] = new Tile (Tile.fence1);
		tiles[0, height - 3] = new Tile (Tile.fence1);
		tiles[0, height - 4] = new Tile (Tile.fence1);
		tiles[width - 1, 0] = new Tile (Tile.fence2);
		tiles[width - 1, 1] = new Tile (Tile.fence2);
		tiles[width - 1, 2] = new Tile (Tile.fence2);
		tiles[width - 1, 3] = new Tile (Tile.fence2);
		tiles[width - 1, height - 1] = new Tile (Tile.fence2);
		tiles[width - 1, height - 2] = new Tile (Tile.fence2);
		tiles[width - 1, height - 3] = new Tile (Tile.fence2);
		tiles[width - 1, height - 4] = new Tile (Tile.fence2);
	}

	public int[] getTileTexCoordinatesAt (int x, int y) {
		return tiles[x,y].graphicsCoordinate;
	}

}
