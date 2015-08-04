using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public TiledMap tm;

	public Vector2 goal;
	public float speed;
	public int blocksToTravel;
	public float accuracy;
	public int health = 2;
	public bool dead { get; private set;}

	private int tilesTraveled = 0;

	public GameObject bloodParticles;

	private bool playerIsNear;

	private Pathfinding2D pf2;
	private Pathfinder2D pfdr2;
	private bool pathNotFound = true;

	public bool turnDone {
		get; private set;
	}

	private bool isTurnCalculated;

	void Start () { pf2 = GetComponent <Pathfinding2D> ();
					dead = false;							}

	void FixedUpdate () {
		if (pathNotFound) {
			pf2.FindPath (transform.position, goal);
			pathNotFound = false;
		}
		if(!GameManager.instance.isPlayerTurn && !dead && !turnDone) {
			
			if (!isTurnCalculated) {
				playerIsNear = isPlayerNear ();
				isTurnCalculated = true;
			}

			if (GameManager.checkIfEnemyIsAt ((Vector2)transform.position + Vector2.right, null) || GameManager.checkIfEnemyIsAt ((Vector2)transform.position + Vector2.right, null) ||
				GameManager.checkIfEnemyIsAt ((Vector2)transform.position + Vector2.right + Vector2.up, null) || GameManager.checkIfEnemyIsAt ((Vector2)transform.position - Vector2.right + Vector2.up, null)) {
				turnDone = true;
				Debug.Log ("Enemy Next to an Enemy");
			}
			if (pf2.Path.Count > 0 && tilesTraveled < blocksToTravel && !playerIsNear) {
				move ();
			} else if (tilesTraveled == blocksToTravel || pf2.Path.Count == 0) 
				turnDone = true;
			if (Vector2.Distance ((Vector2)transform.position, goal) < 0.1f)
				GameManager.instance.gameOver ();
			if (playerIsNear && !turnDone) {
				Debug.Log ("Attacking Player");
				GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerMovement> ().loseHealth (1);
				turnDone = true;
			}
		}
		else if (dead) turnDone = true;
	}

	bool isPlayerNear () {
		playerIsNear = false;
		if (Vector2.Distance (GameObject.FindGameObjectWithTag ("Player").transform.position, transform.position) <= 1.2)
			playerIsNear = true;
		return playerIsNear;
	}

	void move()
	{
		Vector3 p = pf2.Path[0];
		p = new Vector3 (Mathf.Floor (p.x), Mathf.Floor (p.y), -4);
		transform.position = Vector3.MoveTowards(transform.position, p, speed);
		if (Vector2.Distance(transform.position, p) < accuracy) {
			transform.position = p;
			pf2.Path.RemoveAt(0);
			tilesTraveled++;
		}
	}

	public void lowerHealth (int healthLost) {
		health -= healthLost;
		if (health == 0) {
			dead = true;
			GetComponent <Animator> ().SetBool ("isDead", true);
			GetComponent <Collider2D> ().enabled = false;
			GameManager.areAllEnemiesDead ();
			Destroy (gameObject, 5);
		}
		GameObject partcles = (GameObject)Instantiate (bloodParticles, new Vector3 (transform.position.x + 0.5f, transform.position.y + 0.5f, -5), Quaternion.identity);
		Destroy (partcles, 5);
		turnDone = false;
	}

	public void resetTurn () {
		turnDone = false;
		isTurnCalculated = false;
		tilesTraveled = 0;
	}
}
