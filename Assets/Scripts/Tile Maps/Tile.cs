using UnityEngine;
using System.Collections;

public class Tile {

	public const string grass = "Grass";
	public const string castle = "Castle";
	public const string castlebackground = "Castlebackground";
	public const string fence1 = "Fence1";
	public const string fence2 = "Fence2";
	public const string blank = "Blank";

	public string type;
	public Vector2 texCoor;
	public bool isWalkable;

	public Tile (string type) {
		this.type = type;
		if(type == "Grass") {
			texCoor = new Vector2 (0, 0);
			isWalkable = true;
		}
			

		if(type == "Castle") {
			texCoor = new Vector2 (1, 0);
			isWalkable = false;
		}

		if(type == "Castlebackground") {
			texCoor = new Vector2 (2, 0);
			isWalkable = true;
		}

		if(type == "Fence1") {
			texCoor = new Vector2 (0, 1);
			isWalkable = false;
		}

		if(type == "Fence2") {
			texCoor = new Vector2 (1, 1);
			isWalkable = false;
		}

		if(type == "Blank") {
			texCoor = new Vector2 (4, 1);
			isWalkable = false;
		}
	}
}
