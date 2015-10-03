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
		instance = this;
		if (PlayerPrefs.GetInt ("Level") == 0)
			PlayerPrefs.SetInt ("Level", 1);
		if (PlayerPrefs.GetInt ("health") == 0)
			PlayerPrefs.SetInt ("health", 2);

		level = PlayerPrefs.GetInt ("Level");
		PlayerPrefs.Save ();
	}

	void Start () {
		int amountOfEnemies = Mathf.FloorToInt(level/2) + 3;
		int enemyHealth = (Mathf.FloorToInt (Mathf.Log (level, 4))) + 2;

		enemies = new GameObject[amountOfEnemies];
		List <Vector2> places = new List<Vector2> ();
		for (int x = 1; x < tiledmap.width - 2; x++) {
			for (int y = 1; y < tiledmap.height - 12; y++) {
				if (tiledmap[x, y].isWalkable)
					places.Add (new Vector2 (x, y));
			}
		}
		for (int i = 0; i < amountOfEnemies; i++) {
			int j = i + Random.Range (0, places.Count - i);
			Vector2 tmp = places[i];
			places[i] = places[j];
			places[j] = tmp;
			if (Random.Range (0, 9) == 0) {
				enemies[i] = (GameObject)Instantiate (speedyEnemy, places[i], Quaternion.identity);
				enemies[i].GetComponent <Enemy> ().maxHealth = 1;
				enemies[i].GetComponent <Enemy> ().movementPriority = (i + 1)*amountOfEnemies;

				enemies[i].transform.GetChild (0).GetComponent <Canvas> ().worldCamera = Camera.main;
			} else {
				enemies[i] = (GameObject)Instantiate (enemy, places[i], Quaternion.identity);
				enemies[i].GetComponent <Enemy> ().health = enemyHealth;
				enemies[i].GetComponent <Enemy> ().maxHealth = enemyHealth;
				enemies[i].GetComponent <Enemy> ().movementPriority = i;
				enemies[i].transform.GetChild (0).GetComponent <Canvas> ().worldCamera = Camera.main;
			}
		}
		Vector2 place = places[amountOfEnemies + Random.Range (0, places.Count - amountOfEnemies)];
		Instantiate (potion, place, Quaternion.identity);
		GameObject.Find ("Zombie Health Count").GetComponent <Text> ().text = ": " + enemyHealth.ToString ();
		GameObject.Find ("Level Number").GetComponent <Text> ().text = "Level " + level.ToString ();
		GameObject.Find ("Health Count").GetComponent <Text> ().text = "x " + PlayerPrefs.GetInt ("health").ToString ();
	}

	void Update () {
		if (!isPlayerTurn) {
			bool enemiesAreDone = true;
			foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
				if (g.GetComponent <Enemy> () == null) {
					Debug.Log ("Broken Enemy Component");
					continue;
				}
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
					if (g.GetComponent <Enemy> () == null) {
						Debug.Log ("Broken Enemy Component");
						continue;
					}
					g.GetComponent <Enemy> ().resetTurn ();
				}
			}
		}
		if (Input.GetKey (KeyCode.R)) {
			PlayerPrefs.DeleteAll ();
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public static bool areAllEnemiesDead () {
		bool enemiesaredead = true;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
			if (g.GetComponent <Enemy> () == null) {
				Debug.Log ("Broken Enemy Component");
				continue;
			}
			if (!g.GetComponent <Enemy> ().dead) enemiesaredead = false;
		}
		playerWon = enemiesaredead;
		if (playerWon)
			GameManager.instance.win ();
		return enemiesaredead;
	}

	public static GameObject checkEnemy (Vector2 place) {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
			if (g == null) continue;
			if (g.GetComponent <Enemy> ().dead) continue;
			if (Vector2.Distance (place, g.transform.position) < 0.1f) {
				return g;
			}
		}
		return null;
	}

	public void gameOver () {
		isGameOver = true;
		GetComponent <Shop> ().coins += 5;
		PlayerPrefs.SetInt ("coins", GetComponent <Shop> ().coins);
		StartCoroutine (loadLevel (2, "Game Over"));
	}

	private void win () {
		PlayerPrefs.SetInt ("Level", PlayerPrefs.GetInt ("Level") + 1);
		PlayerPrefs.Save ();
		level++;
		GameObject.Find ("Level Number").GetComponent <Text> ().text = "Level " + level.ToString ();
		GetComponent<Shop> ().showShop ();
	}

	public void nextLevel () {
		Application.LoadLevel ("Game");
	}
	 
	IEnumerator loadLevel (float time, string level) {
		yield return new WaitForSeconds (time);
		Application.LoadLevel (level);
	}
}
