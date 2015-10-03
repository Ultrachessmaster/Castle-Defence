using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public TiledMap tm;

	public Vector2 goal;
	public float speed;
	public int blocksToTravel;
	public float accuracy;
	public int health = 2;
	public int maxHealth = 2;
	public bool dead { get; private set;}
	public bool speedy;
	public int movementPriority;
	bool isPathCalculated;
	private int tilesTraveled = 0;
	public AudioClip enemyHit;
	public AudioClip enemyDie;

	public GameObject bloodParticles;

	private bool playerIsNear = false;
	private Pathfinding2D pf2;

	public bool turnDone {
		get; private set;
	}

	private bool isTurnCalculated;

	void Start () {
		pf2 = GetComponent <Pathfinding2D> ();
		dead = false;
		turnDone = false;
		isPathCalculated = false;
	}

	void FixedUpdate () {
		if (!isPathCalculated) {
			Debug.Log ("Path Calculated");
			pf2.FindPath (transform.position, goal);
			isPathCalculated = true;
		}

		if(!GameManager.instance.isPlayerTurn && !dead && !turnDone) {

			if (!isTurnCalculated) {
				playerIsNear = isPlayerNear ();
				GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
				for (int i = 0; i < enemies.Length; i++) {
					if (enemies[i].GetComponent <Enemy> () == null) {
						Debug.Log ("Broken Enemy Component");
						continue;
					}
					if (enemies[i].GetComponent <Enemy> ().dead) continue;
					if (enemies[i].Equals (gameObject)) continue;
					if (enemies[i].GetComponent <Pathfinding2D> ().Path[1].Equals (pf2.Path[1]) && enemies[i].GetComponent <Enemy> ().movementPriority > movementPriority)
						turnDone = true;
					if (Vector2.Distance (enemies[i].transform.position, pf2.Path[1]) < 0.1)
						turnDone = true;
				}
				isTurnCalculated = true;
			}
			if (pf2.Path.Count > 0 && tilesTraveled < blocksToTravel && !playerIsNear && !turnDone) {
				move ();
			} else {
				if (tilesTraveled == blocksToTravel || pf2.Path.Count == 0)
					turnDone = true;
				GetComponent <Rigidbody2D> ().velocity = Vector2.zero;
			}
			if (Vector2.Distance ((Vector2)transform.position, goal) < accuracy)
				GameManager.instance.gameOver ();
			if (playerIsNear && !turnDone) {
				GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerMovement> ().loseHealth (1);
				turnDone = true;
			}
		}
		else {
			if (dead) turnDone = true;
			if (tilesTraveled != blocksToTravel && !playerIsNear && turnDone) transform.position = new Vector3 (pf2.Path[0].x, pf2.Path[0].y, -4);
		}
	}

	bool isPlayerNear () {
		playerIsNear = false;
		if (Vector2.Distance (GameObject.FindGameObjectWithTag ("Player").transform.position, transform.position) <= 1.2)
			playerIsNear = true;
		return playerIsNear;
	}

	void move()
	{
		Vector3 target = pf2.Path[1];
		target = new Vector3 (Mathf.Floor (target.x), Mathf.Floor (target.y), -4);
		Vector3 previousPos = pf2.Path[0];
		previousPos = new Vector3 (Mathf.Floor (previousPos.x), Mathf.Floor (previousPos.y), -4);
		if (Vector2.Distance(transform.position, target) < accuracy) {
			if (speedy)
				Debug.Log ("Made it to target");
			GetComponent <Rigidbody2D> ().velocity = Vector2.zero;
			GetComponent <Rigidbody2D> ().MovePosition (target);
			pf2.Path.RemoveAt(0);
			tilesTraveled++;
		} else {
			GetComponent <Rigidbody2D> ().velocity = (target - previousPos) * speed;
		}
	}

	public void lowerHealth (int healthLost) {
		health -= healthLost;
		if (health <= 0) {
			dead = true;
			GetComponent <Animator> ().SetBool ("isDead", true);
			GetComponent <AudioSource> ().PlayOneShot (enemyDie);
			GetComponent <Collider2D> ().enabled = false;
			GetComponent <SpriteRenderer> ().sortingOrder = 0;
			GameManager.areAllEnemiesDead ();
			Destroy (gameObject, 5);
		} else {
			GetComponent <AudioSource> ().PlayOneShot (enemyHit);
		}
		GameObject partcles = (GameObject)Instantiate (bloodParticles, new Vector3 (transform.position.x + 0.5f, transform.position.y + 0.5f, -5), Quaternion.identity);
		Destroy (partcles, 5);
		transform.GetChild (0).GetChild (0).GetComponent <Image> ().fillAmount = (float)health/maxHealth;
	}

	public void resetTurn () {
		turnDone = false;
		isTurnCalculated = false;
		tilesTraveled = 0;
	}
}
