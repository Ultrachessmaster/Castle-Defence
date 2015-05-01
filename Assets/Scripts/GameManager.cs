using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public bool isPlayerTurn = true;

	public readonly bool playerTurn;
	public GameObject enemy;
	public GameObject[] enemies;
	public int amountOfEnemies;

	public static TiledMap tiledmap;

	public static GameManager instance;

	void Awake () {

	}

	void Start () {
		instance = this;
		enemies = new GameObject[amountOfEnemies];
		for (int i = 0; i < amountOfEnemies; i++) {

			bool placeFound = false;
			int timesTried = 0;
			for (int j = 0; j < 10 && !placeFound; j++) {
				int x = Random.Range (0,tiledmap.width - 1);
				int y = Random.Range (0,tiledmap.width - 1);
				Tile tile = tiledmap.tiles[x,y];
				if (tile.isWalkable) {
					enemies[i] = (GameObject)Instantiate (enemy, new Vector3 (x, y, -4), Quaternion.identity);
					placeFound = true;
				}
			}
			if (!placeFound) {
				Debug.Log ("Place for Enemy " + i + " not found. Giving up.");
			}
		}
	}

	void Update () {
		if (!isPlayerTurn) {
			bool enemiesAreDone = true;
			foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
				if (!g.GetComponent <Enemy> ().turnDone) {
					enemiesAreDone = false;
				}
			}
			if (enemiesAreDone) {
				isPlayerTurn = true;
				foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
					g.GetComponent <Enemy> ().resetTurn ();
				}
			}
		}
	}
}
