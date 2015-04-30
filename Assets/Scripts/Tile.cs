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
	public int[] graphicsCoordinate;
	public bool isWalkable;

	public Tile (string type) {
		this.type = type;
		if(type == "Grass") {
			graphicsCoordinate = new int[2] {0,0};
			isWalkable = true;
		}
			

		if(type == "Castle") {
			graphicsCoordinate = new int[2] {1,0};
			isWalkable = false;
		}

		if(type == "Castlebackground") {
			graphicsCoordinate = new int[2] {2,0};
			isWalkable = true;
		}

		if(type == "Fence1") {
			graphicsCoordinate = new int[2] {0,1};
			isWalkable = false;
		}

		if(type == "Fence2") {
			graphicsCoordinate = new int[2] {1,1};
			isWalkable = false;
		}

		if(type == "Blank") {
			graphicsCoordinate = new int[2] {4,0};
			isWalkable = false;
		}
	}
}
