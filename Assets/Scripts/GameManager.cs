using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public bool isPlayerTurn = true;

	public readonly bool playerTurn;

	public GameObject enemy;
	public GameObject speedyEnemy;
	public GameObject potion;

	public GameObject[] enemies;
	public int amountOfEnemies;
	public float enemyTurnLength;
	private float enemyTimer;
	public int level = 1;

	public int seed = 0;

	public static bool playerWon { get; private set; }

	public static bool isGameOver { get; private set; }

	public static TiledMap tiledmap;

	public static GameManager instance;

	void Awake () {
		if (seed != 0)
			Random.seed = seed;
	}

	void Start () {
		instance = this;
		if (PlayerPrefs.GetInt ("Level") == 0)
			PlayerPrefs.SetInt ("Level", 1);

		level = PlayerPrefs.GetInt ("Level");
		PlayerPrefs.Save ();
		Debug.Log ("Level " + PlayerPrefs.GetInt ("Level"));
		amountOfEnemies = Mathf.FloorToInt(level/2) + 3;
		int enemyHealth = (Mathf.FloorToInt (Mathf.Log (level, 4))) + 2;
		enemies = new GameObject[amountOfEnemies];

		int[] placesTakenX = new int[amountOfEnemies];
		int[] placesTakenY = new int[amountOfEnemies];
		for (int i = 0; i < amountOfEnemies; i++) {
			bool placeFound = false;
			for (int j = 0; j < 10 && !placeFound; j++) {
				int x = Random.Range (1, tiledmap.width - 2);
				int y = Random.Range (1, tiledmap.width - 12);
				Tile tile = tiledmap[x,y];
				bool placeIsTaken = false;
				foreach (int enemyx in placesTakenX) {
					foreach (int enemyy in placesTakenY) {
						if (x == enemyx && y == enemyy)
							placeIsTaken = true;
					}
				}
				if (tile.isWalkable && !placeIsTaken) {
					if (Random.Range (0, 9) == 0) {
						enemies[i] = (GameObject)Instantiate (speedyEnemy, new Vector3 (x, y, -4), Quaternion.identity);
					} else {
						enemies[i] = (GameObject)Instantiate (enemy, new Vector3 (x, y, -4), Quaternion.identity);
						enemies[i].GetComponent <Enemy> ().health = enemyHealth;
					}
					placesTakenX[i] = x;
					placesTakenY[i] = y;
					placeFound = true;
				}
			}
			if (!placeFound) {
				Debug.Log ("Place for Enemy " + i + " not found. Giving up.");
			}
		}
		bool placeForPotionFound = false;
		while (!placeForPotionFound) {
			int potionX = Random.Range (2, tiledmap.width - 2);
			int potionY = Random.Range (2, tiledmap.height - 5);
			if (tiledmap.tiles [potionX, potionY].type == Tile.grass) {
				Instantiate (potion, new Vector3 (potionX, potionY, 0), Quaternion.identity);
				placeForPotionFound = true;
			}
		}

		

		GameObject.Find ("Level").GetComponent <Text> ().text = "Level " + level.ToString ();
	}

	void Update () {
		if (!isPlayerTurn) {
			bool enemiesAreDone = true;
			foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
				if (!g.GetComponent <Enemy> ().turnDone) {
					enemiesAreDone = false;
					enemyTimer += Time.deltaTime;
				}
			}
			if (enemyTimer >= enemyTurnLength) {
				enemiesAreDone = true;
			}
			if (enemiesAreDone) {
				isPlayerTurn = true;
				enemyTimer = 0;
				foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
					g.GetComponent <Enemy> ().resetTurn ();
				}
			}
		}
		if (Input.GetKey (KeyCode.R)) {
			PlayerPrefs.SetInt ("Level", 1);
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	public static bool areAllEnemiesDead () {
		bool enemiesaredead = true;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
			if (!g.GetComponent <Enemy> ().dead) enemiesaredead = false;
		}
		playerWon = enemiesaredead;
		if (playerWon)
			GameManager.instance.win ();
		return enemiesaredead;
	}

	public static bool checkIfEnemyIsAt (Vector2 place, GameObject enemyToIgnore) {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
			if (g.Equals (enemyToIgnore)) continue;
			if (g.GetComponent <Enemy> ().dead) continue;
			if (Vector2.Distance (place, g.transform.position) < 0.1f) {
				return true;
			}
		}
		return false;
	}

	public void gameOver () {
		isGameOver = true;
		StartCoroutine (loadLevel (2, "Game Over"));
	}

	private void win () {
		PlayerPrefs.SetInt ("Level", PlayerPrefs.GetInt ("Level") + 1);
		PlayerPrefs.Save ();
		level++;
		GameObject.Find ("Level").GetComponent <Text> ().text = "Level " + level.ToString ();
		StartCoroutine (loadLevel (2, "Game"));
	}
	 
	IEnumerator loadLevel (float time, string level) {
		yield return new WaitForSeconds (time);
		Application.LoadLevel (level);
	}
}
