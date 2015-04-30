using UnityEngine;
using System.Collections;

public class Square {

	public Square parent;

	public Square[] children;

	public float f;

	public float g;

	public readonly float h;

	public readonly bool isWalkable;

	public bool isOnClosedList;

	public Square (bool isWalkable) {
		this.isWalkable = isWalkable;
	}

	public void calculateH (Vector2 goal) {



	}



}
